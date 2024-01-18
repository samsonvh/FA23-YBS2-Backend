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
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;

namespace YBS2.Service.Services.Implements
{
    public class TourActivityService : ITourActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TourActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<TourActivityDto?> Create(TourActivityInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TourActivityDto>> CreateOfTour(List<TourActivityInputDto> inputDtos, Guid id, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Company? existingCompany = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == companyId)
                .FirstOrDefaultAsync();
            if (existingCompany == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.company = $"Company with Id {companyId} not found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
            }
            Tour? existingTour = await _unitOfWork.TourRepository
                .Find(tour => tour.Id == id && tour.CompanyId == companyId)
                .Include(tour => tour.TourActivities)
                .FirstOrDefaultAsync();
            if (existingTour == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.tour = $"Tour with Id {id} not found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.tour, Errors);
            }

            List<TourActivity> tourActivities = _mapper.Map<List<TourActivity>>(inputDtos);
            foreach (TourActivity tourActivity in tourActivities)
            {
                tourActivity.Status = EnumActivityStatus.Active;
            }
            existingTour.TourActivities = tourActivities;
            _unitOfWork.TourRepository.Update(existingTour);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<List<TourActivityDto>>(tourActivities);
        }


        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DefaultPageResponse<TourActivityListingDto>> GetAll(TourActivityPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<TourActivityListingDto>> GetAllOfTour(Guid id, TourActivityPageRequest pageRequest, ClaimsPrincipal claims)
        {
            Tour? existingTour = await _unitOfWork.TourRepository
                .Find(tour => tour.Id == id)
                .FirstOrDefaultAsync();
            if (existingTour == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.tour = $"Tour with Id {id} not found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.tour, Errors);
            }
            IQueryable<TourActivity> query = _unitOfWork.TourActivityRepository.Find(activity => activity.TourId == id && activity.Status == EnumActivityStatus.Active);
            if (claims != null)
            {
                string role = claims.FindFirstValue(ClaimTypes.Role);
                if (role.Equals(nameof(EnumRole.Company)))
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    Company? existingCompany = await _unitOfWork.CompanyRepository
                        .Find(company => company.Id == companyId)
                        .FirstOrDefaultAsync();
                    if (existingCompany == null)
                    {
                        dynamic Errors = new ExpandoObject();
                        Errors.company = $"Company with Id {companyId} not found";
                        throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
                    }
                    query = _unitOfWork.TourActivityRepository.Find(activity => activity.TourId == id);
                }
            }
            List<TourActivityListingDto> list = await query
                .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(company => _mapper.Map<TourActivityListingDto>(company))
                .ToListAsync();
            int totalResults = list.Count;
            int pageCount = totalResults / pageRequest.PageSize + 1;

            return new DefaultPageResponse<TourActivityListingDto>
            {
                Data = list,
                PageCount = pageCount,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize,
                TotalItem = totalResults
            };
        }

        public Task<TourActivityDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TourActivityDto?> GetDetails(Guid id, Guid tourActivityId, ClaimsPrincipal claims)
        {
            Tour? existingTour = await _unitOfWork.TourRepository.Find(tour => tour.Id == id).FirstOrDefaultAsync();
            if (existingTour == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.tour = $"Tour with Id {id} not found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.tour, Errors);
            }
            IQueryable<TourActivity> query = _unitOfWork.TourActivityRepository.Find(tourActivity => tourActivity.Id == tourActivityId && tourActivity.Status == EnumActivityStatus.Active);
            if (claims != null)
            {
                string role = claims.FindFirstValue(ClaimTypes.Role);
                if (role.Equals(nameof(EnumRole.Company)))
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    Company? existingCompany = await _unitOfWork.CompanyRepository
                        .Find(company => company.Id == companyId)
                        .FirstOrDefaultAsync();
                    if (existingCompany == null)
                    {
                        dynamic Errors = new ExpandoObject();
                        Errors.company = $"Company with Id {companyId} not found";
                        throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
                    }
                    query = _unitOfWork.TourActivityRepository.Find(tourActivity => tourActivity.Id == tourActivityId);
                }
            }

            TourActivity tourActivity = await query.FirstOrDefaultAsync();
            if (tourActivity == null)
            {
                return null;
            }
            return _mapper.Map<TourActivityDto>(tourActivity);
        }

        public Task<TourActivityDto?> Update(Guid id, TourActivityInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        private IQueryable<TourActivity> Filter(IQueryable<TourActivity> query, TourActivityPageRequest pageRequest)
        {
            if (pageRequest.Name != null)
            {
                query = query.Where(company => company.Name.ToLower().Contains(pageRequest.Name.Trim().ToLower()));
            }

            if (pageRequest.Location != null)
            {
                query = query.Where(company => company.Location.ToLower().Contains(pageRequest.Location.Trim().ToLower()));
            }

            if (string.IsNullOrEmpty(pageRequest.OrderBy))
            {
                pageRequest.OrderBy = "Id";
            }
            query = query.SortBy(pageRequest.OrderBy, pageRequest.IsDescending);
            return query;
        }
    }
}