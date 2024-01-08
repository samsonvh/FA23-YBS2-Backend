using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YBS.Service.Utils;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;
using YBS2.Service.Utils;

namespace YBS2.Service.Services.Implements
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IConfiguration _configuration;
        public TourService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _configuration = configuration;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<TourDto?> Create(TourInputDto inputDto, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Company? existingCompany = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == companyId && company.Account.Status == EnumAccountStatus.Active)
                .FirstOrDefaultAsync();
            if (existingCompany == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Company = "Company Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Company, Errors);
            }
            if (inputDto.Docks.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Dock = "Tour must have at least 1 dock.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
            }
            Yacht? existingYacht = await _unitOfWork.YachtRepository
                .Find(yacht => yacht.Id == inputDto.YachtId && yacht.CompanyId == companyId)
                .FirstOrDefaultAsync();
            if (existingYacht == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.YachtId = $"Yacht with Id {inputDto.YachtId} does not exist.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.YachtId, Errors);
            }

            if (inputDto.Type == EnumTourType.In_Day)
            {
                inputDto.DurationUnit = EnumTimeUnit.Hours;
                inputDto.Duration = (inputDto.EndTime - inputDto.StartTime).Hours;
            }

            List<TourDock> tourDocks = await _unitOfWork.DockRepository
                .Find(dock => inputDto.Docks.Contains(dock.Id) && dock.Status == EnumDockStatus.Active && dock.CompanyId == companyId)
                .Select(dock => _mapper.Map<TourDock>(dock))
                .ToListAsync();

            if (tourDocks.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Dock = "No Dock Found.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
            }
            Tour tour = _mapper.Map<Tour>(inputDto);
            tour.TourDocks = tourDocks;
            tour.MaximumGuest = existingYacht.TotalPassenger;
            tour.Status = EnumTourStatus.Active;
            tour.CompanyId = companyId;
            _unitOfWork.TourRepository.Add(tour);

            if (inputDto.ImageURLs.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Tour must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            string imageURL = await FirebaseUtil.UpLoadFile(inputDto.ImageURLs, tour.Id, _firebaseStorageService);
            if (imageURL == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.UploadFile = "Error while uploading file";
                throw new APIException(HttpStatusCode.BadRequest, Errors.UploadFile, Errors);
            }
            tour.ImageURL = imageURL;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TourDto>(tour);
        }

        public Task<TourDto?> Create(TourInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<TourListingDto>> GetAll(TourPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<TourListingDto>> GetAll(TourPageRequest pageRequest, ClaimsPrincipal claims)
        {
            IQueryable<Tour> query = _unitOfWork.TourRepository.Find(tour => tour.Status == EnumTourStatus.Active);
            if (claims != null)
            {
                if (claims.FindFirstValue("CompanyId") != null)
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    query = _unitOfWork.TourRepository.Find(tour => tour.CompanyId == companyId);
                }
            }
            query = Filter(query, pageRequest);
            query = query.OrderByDescending(tour => tour.Priority);
            List<Tour> list = await query
                .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();
            List<TourListingDto> resultList = new List<TourListingDto>();
            foreach (Tour tour in list)
            {
                TourListingDto tourListingDto = _mapper.Map<TourListingDto>(tour);
                tourListingDto.ImageURL = tour.ImageURL.Split(',')[0];
                resultList.Add(tourListingDto);
            }
            int totalResults = await query.CountAsync();
            return new DefaultPageResponse<TourListingDto>
            {
                Data = resultList,
                PageCount = totalResults / pageRequest.PageSize + 1,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize,
                TotalItem = totalResults
            };
        }

        public async Task<TourDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<TourDto?> GetDetails(Guid id, ClaimsPrincipal claims)
        {
            IQueryable<Tour> query = _unitOfWork.TourRepository.Find(tour => tour.Id == id);
            if (claims != null)
            {
                if (claims.FindFirstValue("CompanyId") != null)
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    query = query.Where(tour => tour.CompanyId == companyId);
                }
            }
            Tour? tour = await query.FirstOrDefaultAsync();
            if (tour == null)
            {
                return null;
            }
            TourDto tourDto = _mapper.Map<TourDto>(tour);
            tourDto.ImageURLs = tour.ImageURL.Split(',');
            return tourDto;
        }

        public async Task<TourDto?> Update(Guid id, TourInputDto inputDto)
        {   
            Tour? existingTour = await _unitOfWork.TourRepository
                .Find(tour => tour.Id == id && tour.Status == EnumTourStatus.Active)
                .FirstOrDefaultAsync();
            if (existingTour == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Tour = "Tour Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Tour, Errors);
            }
            Yacht? existingYacht = await _unitOfWork.YachtRepository
                .Find(yacht => yacht.Id == inputDto.YachtId)
                .FirstOrDefaultAsync();
            if (existingYacht == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Yacht = "Yacht Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Yacht, Errors);
            }
            await FirebaseUtil.DeleteFile(existingTour.ImageURL, _configuration, _firebaseStorageService);
            if (inputDto.ImageURLs.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Tour must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            string imageURL = await FirebaseUtil.UpLoadFile(inputDto.ImageURLs, existingTour.Id, _firebaseStorageService);
            if (imageURL == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Error while uploading file.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            existingTour.ImageURL = imageURL;
            existingTour.YachtId = inputDto.YachtId;
            existingTour.Name = inputDto.Name;
            existingTour.Price = inputDto.Price;
            existingTour.Priority = inputDto.Priority;
            existingTour.Location = inputDto.Location;
            existingTour.StartTime = inputDto.StartTime;
            existingTour.EndTime = inputDto.EndTime;
            if (inputDto.Type == EnumTourType.Many_Days)
            {
                existingTour.Duration = (int)inputDto.Duration;
                existingTour.DurationUnit = (EnumTimeUnit)inputDto.DurationUnit;
            }
            else
            {
                inputDto.DurationUnit = EnumTimeUnit.Hours;
                inputDto.Duration = DateTime.Now.Add(inputDto.EndTime - inputDto.StartTime).Hour;
            }
            existingTour.MaximumGuest = existingYacht.TotalPassenger;
            existingTour.Type = inputDto.Type;
            existingTour.Description = inputDto.Description;
            _unitOfWork.TourRepository.Update(existingTour);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TourDto>(existingTour);
        }

        private IQueryable<Tour> Filter(IQueryable<Tour> query, TourPageRequest pageRequest)
        {
            if (pageRequest.Name != null)
            {
                query = query.Where(tour => tour.Name.ToLower().Contains(pageRequest.Name.Trim().ToLower()));
            }

            if (pageRequest.MinPrice >= 0 && pageRequest.MaxPrice > pageRequest.MinPrice)
            {
                query = query.Where(tour => tour.MaximumGuest >= pageRequest.MinPrice && tour.MaximumGuest <= pageRequest.MaxPrice);
            }

            if (pageRequest.Location != null)
            {
                query = query.Where(tour => tour.Location.ToLower().Contains(pageRequest.Location.Trim().ToLower()));
            }

            if (pageRequest.MinGuest >= 0 && pageRequest.MaxGuest > pageRequest.MinGuest)
            {
                query = query.Where(tour => tour.MaximumGuest >= pageRequest.MinGuest && tour.MaximumGuest <= pageRequest.MaxGuest);
            }

            query = query.SortBy(pageRequest.OrderBy, pageRequest.IsDescending);

            return query;
        }

        public async Task<bool> ChangeStatus(Guid id, string status, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Company? existingCompany = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == companyId && company.Account.Status == EnumAccountStatus.Active)
                .FirstOrDefaultAsync();
            if (existingCompany == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.company = "Company Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
            }
            Tour? exisitingTour = await _unitOfWork.TourRepository
                .Find(tour => tour.Id == id && tour.CompanyId == companyId)
                .FirstOrDefaultAsync();
            if (exisitingTour == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Tour = "Tour Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Tour, Errors);
            }
            status = TextUtils.Capitalize(status);
            if (!Enum.IsDefined(typeof(EnumTourStatus), status))
            {
                dynamic Errors = new ExpandoObject();
                Errors.Status = $"Status {status} is invalid";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Status, Errors);
            }
            exisitingTour.Status = Enum.Parse<EnumTourStatus>(status);
            _unitOfWork.TourRepository.Update(exisitingTour);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}