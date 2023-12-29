using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using System.Net;
using System.Security.Claims;
using YBS.Service.Utils;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;
using YBS2.Service.Utils;

namespace YBS2.Service.Services.Implements
{
    public class MembershipPackageService : IMembershipPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MembershipPackageService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> ChangeStatus(Guid id, string status)
        {
            var existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                                                    .Find(membershipPackage => membershipPackage.Id == id)
                                                    .FirstOrDefaultAsync();
            string? message;
            if (existingMembershipPackage != null)
            {
                dynamic errors = new ExpandoObject();
                errors.MembershipPackageId = $"Membership Package with ID {id} not found";
                throw new APIException(HttpStatusCode.NotFound, errors.MembershipPackageId, errors);
            }
            if (!Enum.IsDefined(typeof(EnumMembershipPackageStatus), status))
            {
                dynamic errors = new ExpandoObject();
                errors.Status = $"Status {status} is invalid";
                throw new APIException(HttpStatusCode.BadRequest, errors.Status, errors);
            }
            existingMembershipPackage.Status = Enum.Parse<EnumMembershipPackageStatus>(status);

            _unitOfWork.MembershipPackageRepository.Update(existingMembershipPackage);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<MembershipPackageDto?> Create(MembershipPackageInputDto inputDto)
        {
            await CheckDuplicate(inputDto);

            MembershipPackage membershipPackage = _mapper.Map<MembershipPackage>(inputDto);
            membershipPackage.Status = EnumMembershipPackageStatus.Active;

            _unitOfWork.MembershipPackageRepository.Add(membershipPackage);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MembershipPackageDto>(membershipPackage);
        }


        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll(MembershipPackagePageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll(MembershipPackagePageRequest pageRequest, ClaimsPrincipal claims)
        {
            IQueryable<MembershipPackage> query = _unitOfWork.MembershipPackageRepository.GetAll();
            if (claims != null)
            {
                string role = claims.FindFirstValue(ClaimTypes.Role);
                if (role != nameof(EnumRole.Admin))
                {
                    query = query.Where(membershipPackage => membershipPackage.Status == EnumMembershipPackageStatus.Active);
                }
            }
            query = FilterGetAll(query, pageRequest);

            int totalCount = query.Count();

            int pageCount = totalCount / pageRequest.PageSize + 1;
            List<MembershipPackageListingDto> list = await query
                                                    .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                                                    .Take(pageRequest.PageSize)
                                                    .Select(membershipPackage => _mapper.Map<MembershipPackageListingDto>(membershipPackage))
                                                    .ToListAsync();

            DefaultPageResponse<MembershipPackageListingDto> pageResponse =
            new DefaultPageResponse<MembershipPackageListingDto>
            {
                Data = list,
                PageCount = pageCount,
                TotalItem = totalCount,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize
            };
            return pageResponse;
        }

        public async Task<MembershipPackageDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<MembershipPackageDto?> GetDetails(Guid id, ClaimsPrincipal claims)
        {
            MembershipPackage? membershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == id)
                .FirstOrDefaultAsync();
            if (membershipPackage != null)
            {
                if (claims != null)
                {
                    string role = claims.FindFirstValue(ClaimTypes.Role);
                    if (role != nameof(EnumRole.Admin))
                    {
                        if (membershipPackage.Status == EnumMembershipPackageStatus.Inactive)
                        {
                            dynamic errors = new ExpandoObject();
                            errors.MembershipPackage = "This membership package is currently inactive";
                            throw new APIException(HttpStatusCode.BadRequest, errors.MembershipPackage, errors);
                        }
                    }
                }
                return _mapper.Map<MembershipPackageDto>(membershipPackage);
            }
            return null;
        }

        public async Task<MembershipPackageDto?> Update(Guid id, MembershipPackageInputDto inputDto)
        {
            //validate membership package field 
            MembershipPackage? membershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == id)
                .FirstOrDefaultAsync();
            if (membershipPackage == null)
            {
                dynamic errors = new ExpandoObject();
                errors.MembershipPackageId = $"Membership Package with ID {id} not found";
                throw new APIException(HttpStatusCode.NotFound, errors.MembershipPackageId, errors);
            }
            membershipPackage.Name = inputDto.Name;
            membershipPackage.Price = inputDto.Price;
            membershipPackage.Point = inputDto.Point;
            membershipPackage.Duration = inputDto.Duration;
            membershipPackage.DurationUnit = inputDto.DurationUnit;
            membershipPackage.DiscountPercent = inputDto.DiscountPercent;
            membershipPackage.Description = inputDto.Description;

            _unitOfWork.MembershipPackageRepository.Update(membershipPackage);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MembershipPackageDto>(membershipPackage);
        }

        private async Task CheckDuplicate(MembershipPackageInputDto inputDto)
        {
            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
            .Find(membershipPackage => membershipPackage.Name == inputDto.Name)
            .FirstOrDefaultAsync();
            if (existingMembershipPackage != null)
            {
                dynamic errors = new ExpandoObject();
                errors.Name = $"MembershipPackage with name: {inputDto.Name} already exist";
                throw new APIException(HttpStatusCode.BadRequest, errors.Name, errors);
            }
        }

        private IQueryable<MembershipPackage> FilterGetAll(IQueryable<MembershipPackage> query, MembershipPackagePageRequest pageRequest)
        {
            if (pageRequest.Name != null)
            {
                query = query.Where(membershipPackage => membershipPackage.Name.Trim().ToLower().Contains(pageRequest.Name.ToLower()));
            }
            if (pageRequest.MinPrice > 0 && pageRequest.MaxPrice > pageRequest.MinPrice)
            {
                query = query.Where(membershipPackage => membershipPackage.Price <= pageRequest.MaxPrice && membershipPackage.Price >= pageRequest.MinPrice);
            }
            if (pageRequest.Status != null && Enum.IsDefined(typeof(EnumMembershipPackageStatus), pageRequest.Status))
            {
                query = query.Where(membershipPackage => membershipPackage.Status == pageRequest.Status);
            }
            query = query.SortBy(pageRequest.OrderBy, pageRequest.IsDescending);
            return query;
        }


    }
}