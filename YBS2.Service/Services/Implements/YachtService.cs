using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class YachtService : IYachtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _storageService;
        public YachtService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storageService = storageService;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<YachtDto?> Create(YachtInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<YachtDto?> Create(YachtInputDto inputDto, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Company? existingCompany = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == companyId)
                .FirstOrDefaultAsync();
            if (existingCompany == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.company = "Company Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
            }

            Yacht yacht = _mapper.Map<Yacht>(inputDto);
            if (inputDto.Images.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.yacht = "Yacht must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.yacht, Errors);
            }
            yacht.CompanyId = companyId;
            yacht.Status = EnumYachtStatus.Available;
            _unitOfWork.YachtRepository.Add(yacht);
            string imageURL = await FirebaseUtil.UpLoadFile(inputDto.Images, yacht.Id, _storageService);
            yacht.ImageURL = imageURL;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<YachtDto>(yacht);
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<YachtListingDto>> GetAll(YachtPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<YachtListingDto>> GetAll(YachtPageRequest pageRequest, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Company? existingCompany = await _unitOfWork.CompanyRepository.Find(company => company.Id == companyId).FirstOrDefaultAsync();
            if (existingCompany == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.company = "Company Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
            }
            IQueryable<Yacht> query = _unitOfWork.YachtRepository.Find(yacht => yacht.CompanyId == companyId);

            query = Filter(query, pageRequest);

            List<Yacht> list = await query
                .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            List<YachtListingDto> resultList = new List<YachtListingDto>();
            foreach (Yacht yacht in list)
            {
                YachtListingDto yachtListingDto = _mapper.Map<YachtListingDto>(yacht);
                yachtListingDto.ImageURL = yacht.ImageURL.Split(',')[0];
                resultList.Add(yachtListingDto);
            }
            int totalResults = await query.CountAsync();

            return new DefaultPageResponse<YachtListingDto>
            {
                Data = resultList,
                PageCount = totalResults / pageRequest.PageSize + 1,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize,
                TotalItem = totalResults
            };
        }

        public async Task<YachtDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<YachtDto?> GetDetails(Guid id, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Company? existingCompany = await _unitOfWork.CompanyRepository.Find(company => company.Id == companyId).FirstOrDefaultAsync();
            if (existingCompany == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.company = "Company Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
            }
            
            Yacht? existingYacht = await _unitOfWork.YachtRepository
                .Find(yacht => yacht.Id == id && yacht.CompanyId == companyId)
                .FirstOrDefaultAsync();
            YachtDto yachtDto = _mapper.Map<YachtDto>(existingYacht);
            if (existingYacht == null)
            {
                return null;
            }
            yachtDto.ImageURL = existingYacht.ImageURL.Split(",");
            return yachtDto;
        }

        public Task<YachtDto?> Update(Guid id, YachtInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Yacht> Filter(IQueryable<Yacht> query, YachtPageRequest pageRequest)
        {
            if (pageRequest.Name != null)
            {
                query = query.Where(yacht => yacht.Name.ToLower().Contains(pageRequest.Name.Trim().ToLower()));
            }

            if (pageRequest.MinPassengers >= 0 && pageRequest.MaxPassengers > pageRequest.MinPassengers)
            {
                query = query.Where(yacht => yacht.TotalPassenger >= pageRequest.MinPassengers && yacht.TotalPassenger <= pageRequest.MaxPassengers);
            }

            if (pageRequest.MinCrew >= 0 && pageRequest.MaxCrew > pageRequest.MinCrew)
            {
                query = query.Where(yacht => yacht.TotalCrew >= pageRequest.MinCrew && yacht.TotalCrew <= pageRequest.MaxCrew);
            }

            if (pageRequest.MinCabin >= 0 && pageRequest.MaxCabin > pageRequest.MinCabin)
            {
                query = query.Where(yacht => yacht.Cabin >= pageRequest.MinCabin && yacht.Cabin <= pageRequest.MaxCabin);
            }

            query = query.SortBy(pageRequest.OrderBy, pageRequest.IsDescending);

            return query;
        }

    }
}