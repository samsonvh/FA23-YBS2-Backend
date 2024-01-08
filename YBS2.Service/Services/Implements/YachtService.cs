using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YBS.Service.Utils;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services.Implements
{
    public class YachtService : IYachtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public YachtService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<YachtDto?> Create(YachtInputDto inputDto)
        {
            throw new NotImplementedException();
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
            IQueryable<Yacht> query = _unitOfWork.YachtRepository.GetAll();
            if (claims != null)
            {
                if (claims.FindFirstValue("CompanyId") != null)
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    query = query.Where(yacht => yacht.CompanyId == companyId);
                }
            }

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
            return _mapper.Map<YachtDto>
            (await _unitOfWork.YachtRepository.GetByID(id));
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