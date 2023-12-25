using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YBS.Service.Utils;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
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
        public async Task<bool> ChangeStatus(Guid id, string name)
        {
            var existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                                                    .Find(membershipPackage => membershipPackage.Id == id)
                                                    .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                throw new APIException(HttpStatusCode.NotFound, "Membership Package not found.");
            }
            if (!Enum.IsDefined(typeof(EnumMembershipPackageStatus), name))
            {
                throw new APIException(HttpStatusCode.BadRequest, "Membership Package status is not defined");
            }
            existingMembershipPackage.Status = (EnumMembershipPackageStatus)Enum.Parse(typeof(EnumMembershipPackageStatus), name);
            _unitOfWork.MembershipPackageRepository.Update(existingMembershipPackage);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<MembershipPackageDto?> Create(MembershipPackageInputDto inputDto)
        {
            //validate membership package field 
            await CheckDupplicate(inputDto);
            MembershipPackage membershipPackage = _mapper.Map<MembershipPackage>(inputDto);
            membershipPackage.Status = EnumMembershipPackageStatus.Active;
            _unitOfWork.MembershipPackageRepository.Add(membershipPackage);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<MembershipPackageDto>(membershipPackage);
        }

        public Task<bool> Delete(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll(MembershipPackagePageRequest pageRequest)
        {
            IQueryable<MembershipPackage> query = _unitOfWork.MembershipPackageRepository.GetAll();
            ClaimsPrincipal claims = JWTUtils.GetClaim(_httpContextAccessor, _configuration);
            if (claims != null)
            {
                string role = claims.FindFirstValue(ClaimTypes.Role);
                if (role != nameof(EnumRole.Admin))
                {
                    query = query.Where(membershipPackage => membershipPackage.Status == EnumMembershipPackageStatus.Active);
                }
            }
            query = FilterGetAll(query, pageRequest);

            var totalCount = query.Count();

            var pageCount = totalCount / pageRequest.PageSize + 1;
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
            MembershipPackage? membershipPackage = await _unitOfWork.MembershipPackageRepository
                                                                        .Find(membershipPackage => membershipPackage.Id == id)
                                                                        .FirstOrDefaultAsync();
            if (membershipPackage != null)
            {
                ClaimsPrincipal claims = JWTUtils.GetClaim(_httpContextAccessor, _configuration);
                if (claims != null)
                {
                    string role = claims.FindFirstValue(ClaimTypes.Role);
                    if (role != nameof(EnumRole.Admin))
                    {
                        if (membershipPackage.Status == EnumMembershipPackageStatus.Inactive)
                        {
                            throw new APIException(HttpStatusCode.BadRequest, "This membership package is currently inactive, please choose another membership package.");
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
                throw new APIException(HttpStatusCode.NotFound, "Membership Package not found");
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
        private async Task CheckDupplicate(MembershipPackageInputDto inputDto)
        {
            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
            .Find(membershipPackage => membershipPackage.Name == inputDto.Name)
            .FirstOrDefaultAsync();
            if (existingMembershipPackage != null)
            {
                string message = "MembershipPackage with name: " + inputDto.Name + " already exist, please choose another name.";
                throw new APIException(HttpStatusCode.BadRequest, message);
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
            query = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                    ? query.SortBy(pageRequest.OrderBy, pageRequest.IsAscending)
                    : pageRequest.IsAscending
                    ? query.OrderBy(membershipPackage => membershipPackage.Id)
                    : query.OrderByDescending(membershipPackage => membershipPackage.Id);
            return query;
        }


    }
}