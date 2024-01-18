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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                Errors.company = "Company Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.company, Errors);
            }
            if (inputDto.Docks.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.dock = "Tour must have at least 1 dock.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.dock, Errors);
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
            else
            {
                if (inputDto.Duration == null && inputDto.DurationUnit == null)
                {
                    dynamic Errors = new ExpandoObject();
                    Errors.Tour = "Duration and duration unit must not be null with ManyDays tour type";
                    throw new APIException(HttpStatusCode.BadRequest, Errors.Tour, Errors);
                }
            }
            List<TourDock> tourDocks = await _unitOfWork.DockRepository
                .Find(dock => inputDto.Docks.Contains(dock.Id) && dock.Status == EnumDockStatus.Active && dock.CompanyId == companyId)
                .Select(dock => _mapper.Map<TourDock>(dock))
                .ToListAsync();

            if (tourDocks.Count < inputDto.Docks.Count)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Dock = "Error while adding docks";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
            }
            List<TourActivityInputDto>? tourActivitiesInputDtos = JsonConvert.DeserializeObject<List<TourActivityInputDto>>(inputDto.Activities);
            if (tourActivitiesInputDtos.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.tourActivities = "List activity of tour is null";
                throw new APIException(HttpStatusCode.BadRequest, Errors.tourActivities, Errors);
            }
            List<TourActivity> tourActivities = _mapper.Map<List<TourActivity>>(tourActivitiesInputDtos);
            Tour tour = _mapper.Map<Tour>(inputDto);
            tour.TourActivities = tourActivities;
            tour.TourDocks = tourDocks;
            tour.MaximumGuest = existingYacht.TotalPassengers;
            tour.Status = EnumTourStatus.Active;
            tour.CompanyId = companyId;
            _unitOfWork.TourRepository.Add(tour);

            if (inputDto.Images.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Tour must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            string imageURL = await FirebaseUtil.UpLoadFile(inputDto.Images, tour.Id, _firebaseStorageService);
            if (imageURL == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.UploadFile = "Error while uploading file";
                throw new APIException(HttpStatusCode.BadRequest, Errors.UploadFile, Errors);
            }
            tour.ImageURL = imageURL;
            await _unitOfWork.SaveChangesAsync();
            TourDto tourDto = _mapper.Map<TourDto>(tour);
            tourDto.ImageURLs = imageURL.Split(",");
            return tourDto;
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
            MembershipRegistration? membershipRegistration = null;
            if (claims != null)
            {
                string role = TextUtils.Capitalize(claims.FindFirstValue(ClaimTypes.Role));
                if (role == nameof(EnumRole.Company))
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    query = _unitOfWork.TourRepository.Find(tour => tour.CompanyId == companyId);
                }
                else if (role == nameof(EnumRole.Member))
                {
                    Guid memberId = Guid.Parse(claims.FindFirstValue("MemberId"));
                    Member? member = await _unitOfWork.MemberRepository
                        .Find(member => member.Id == memberId)
                        .Include(member => member.MembershipRegistrations)
                        .FirstOrDefaultAsync();
                    membershipRegistration = member.MembershipRegistrations
                    .FirstOrDefault(membershipRegistration => membershipRegistration.Status == EnumMembershipRegistrationStatus.Active);
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
                if (membershipRegistration != null)
                {
                    tourListingDto.DiscountPrice = (float)(tour.Price - tour.Price * membershipRegistration.DiscountPercent / 100);
                }
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
            IQueryable<Tour> query = _unitOfWork.TourRepository.Find(tour => tour.Id == id && tour.Status == EnumTourStatus.Active);
            MembershipRegistration? membershipRegistration = null;
            if (claims != null)
            {
                string role = TextUtils.Capitalize(claims.FindFirstValue(ClaimTypes.Role));
                if (role == nameof(EnumRole.Company))
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    query = _unitOfWork.TourRepository.Find(tour => tour.CompanyId == companyId);
                }
                else if (role == nameof(EnumRole.Member))
                {
                    Guid memberId = Guid.Parse(claims.FindFirstValue("MemberId"));
                    Member? member = await _unitOfWork.MemberRepository
                        .Find(member => member.Id == memberId)
                        .Include(member => member.MembershipRegistrations)
                        .FirstOrDefaultAsync();
                    membershipRegistration = member.MembershipRegistrations
                    .FirstOrDefault(membershipRegistration => membershipRegistration.Status == EnumMembershipRegistrationStatus.Active);
                }
            }
            Tour? tour = await query.FirstOrDefaultAsync();
            if (tour == null)
            {
                return null;
            }
            TourDto tourDto = _mapper.Map<TourDto>(tour);
            if (membershipRegistration != null)
            {
                tourDto.DiscountPrice = (float)(tour.Price - tour.Price * membershipRegistration.DiscountPercent / 100);
            }
            tourDto.ImageURLs = tour.ImageURL.Split(',');
            return tourDto;
        }

        public async Task<TourDto?> Update(Guid id, TourInputDto inputDto, ClaimsPrincipal claims)
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
            Tour? existingTour = await _unitOfWork.TourRepository
                .Find(tour => tour.Id == id && tour.Status == EnumTourStatus.Active && tour.CompanyId == companyId)
                .Include(tour => tour.Bookings)
                .Include(tour => tour.TourDocks)
                .Include(tour => tour.TourActivities)
                .FirstOrDefaultAsync();
            if (existingTour == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Tour = "Tour Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Tour, Errors);
            }
            if (CountNotYetStartedBooking(existingTour) > 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.tour = "Tour have booking which is not yet started";
                throw new APIException(HttpStatusCode.BadRequest, Errors.tour, Errors);
            }
            Yacht? existingYacht = await _unitOfWork.YachtRepository
                .Find(yacht => yacht.Id == inputDto.YachtId && yacht.CompanyId == companyId)
                .FirstOrDefaultAsync();
            if (existingYacht == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Yacht = "Yacht Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Yacht, Errors);
            }
            if (inputDto.Type == EnumTourType.In_Day)
            {
                inputDto.DurationUnit = EnumTimeUnit.Hours;
                inputDto.Duration = (inputDto.EndTime - inputDto.StartTime).Hours;
            }
            else
            {
                if (inputDto.Duration == null && inputDto.DurationUnit == null)
                {
                    dynamic Errors = new ExpandoObject();
                    Errors.Tour = "Duration and duration unit must not be null with ManyDays tour type";
                    throw new APIException(HttpStatusCode.BadRequest, Errors.Tour, Errors);
                }
            }
            List<TourDock> updatedTourDock = new List<TourDock>();
            if (inputDto.Docks.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Dock = "Tour must have at least 1 docks";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
            }
            foreach (Guid dockId in inputDto.Docks)
            {
                Dock? existingDock = await _unitOfWork.DockRepository
                    .Find(dock => dock.Id == dockId && dock.CompanyId == companyId)
                    .FirstOrDefaultAsync();
                if (existingDock == null)
                {
                    dynamic Errors = new ExpandoObject();
                    Errors.Dock = "Dock Not Found";
                    throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
                }
                TourDock tourDock = _mapper.Map<TourDock>(existingDock);

                updatedTourDock.Add(tourDock);
            }

            if (updatedTourDock.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Dock = "Error while updating tour docks";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
            }

            if (inputDto.Images.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Tour must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            List<TourActivityInputDto>? tourActivitiesInputDtos = JsonConvert.DeserializeObject<List<TourActivityInputDto>>(inputDto.Activities);
            if (tourActivitiesInputDtos.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.tourActivities = "List activity of tour is null";
                throw new APIException(HttpStatusCode.BadRequest, Errors.tourActivities, Errors);
            }
            List<TourActivity> tourActivities = _mapper.Map<List<TourActivity>>(tourActivitiesInputDtos);
            // await FirebaseUtil.DeleteFile(existingTour.ImageURL, _configuration, _firebaseStorageService);

            string imageURL = await FirebaseUtil.UpLoadFile(inputDto.Images, existingTour.Id, _firebaseStorageService);
            if (imageURL == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Error while uploading file.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            existingTour.TourDocks = updatedTourDock;
            existingTour.ImageURL = imageURL;
            existingTour.YachtId = inputDto.YachtId;
            existingTour.Name = inputDto.Name;
            existingTour.Price = inputDto.Price;
            existingTour.Priority = inputDto.Priority;
            existingTour.Location = inputDto.Location;
            existingTour.StartTime = inputDto.StartTime;
            existingTour.EndTime = inputDto.EndTime;
            existingTour.Duration = (int)inputDto.Duration;
            existingTour.DurationUnit = (EnumTimeUnit)inputDto.DurationUnit;
            existingTour.MaximumGuest = existingYacht.TotalPassengers;
            existingTour.Type = inputDto.Type;
            existingTour.Description = inputDto.Description;
            existingTour.TourActivities = tourActivities;
            _unitOfWork.TourRepository.Update(existingTour);
            await _unitOfWork.SaveChangesAsync();
            TourDto tourDto = _mapper.Map<TourDto>(existingTour);
            tourDto.ImageURLs = imageURL.Split(",");
            return tourDto;
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
            Tour? existingTour = await _unitOfWork.TourRepository
                .Find(tour => tour.Id == id && tour.CompanyId == companyId)
                .Include(tour => tour.Bookings)
                .FirstOrDefaultAsync();
            if (existingTour == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Tour = "Tour Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Tour, Errors);
            }
            if (CountNotYetStartedBooking(existingTour) > 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.tour = "Tour have booking which is not yet started";
                throw new APIException(HttpStatusCode.BadRequest, Errors.tour, Errors);
            }
            status = TextUtils.Capitalize(status);
            if (!Enum.IsDefined(typeof(EnumTourStatus), status))
            {
                dynamic Errors = new ExpandoObject();
                Errors.Status = $"Status {status} is invalid";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Status, Errors);
            }
            existingTour.Status = Enum.Parse<EnumTourStatus>(status);
            _unitOfWork.TourRepository.Update(existingTour);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public Task<TourDto?> Update(Guid id, TourInputDto inputDto)
        {
            throw new NotImplementedException();
        }
        private int CountNotYetStartedBooking(Tour tour)
        {
            return tour.Bookings
                .Where(booking => booking.BookingDate.Date
                .Add(new TimeSpan(tour.StartTime.Hours, tour.StartTime.Minutes, tour.StartTime.Seconds))
                .CompareTo(DateTime.Now) > 0).ToList().Count();
        }
    }
}