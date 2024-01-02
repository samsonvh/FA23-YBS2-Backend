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
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;

namespace YBS2.Service.Services.Implements
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _firebaseStorageService;
        public TourService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<TourDto?> Create(TourInputDto inputDto, ClaimsPrincipal claims)
        {
            Guid tourId = Guid.Parse(claims.FindFirstValue("TourId"));
            Yacht? existingYacht = await _unitOfWork.YachtRepository
                .Find(yacht => yacht.Id == inputDto.YachtId)
                .FirstOrDefaultAsync();
            if (existingYacht == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.YachtId = $"Yacht with Id {inputDto.YachtId} does not exist.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.YachtId, Errors);
            }

            if (inputDto.Type == EnumTourType.In_Day)
            {
                inputDto.DurationUnit = "Hours";
                inputDto.Duration = DateTime.Now.Add(inputDto.EndTime - inputDto.StartTime).Hour;
            }

            Tour tour = _mapper.Map<Tour>(inputDto);
            tour.MaximumGuest = existingYacht.TotalPassenger;
            tour.Status = EnumTourStatus.Active;
            tour.Id = tourId;
            _unitOfWork.TourRepository.Add(tour);
            await _unitOfWork.SaveChangesAsync();

            if (inputDto.ImageURL.Count > 0)
            {
                string imageURL = "";
                foreach (var image in inputDto.ImageURL)
                {
                    string imageName = tour.Id.ToString();
                    Uri imageUri = await _firebaseStorageService.UploadFile(imageName, image);
                    imageURL += imageUri.ToString() + ",";
                }
                imageURL = imageURL.Remove(imageURL.Length - 1, 1);
                tour.ImageURL = imageURL;
                _unitOfWork.TourRepository.Update(tour);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = $"Tour must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
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
            return await query.Select(tour => _mapper.Map<TourDto>(tour)).FirstOrDefaultAsync();
        }

        public async Task<TourDto?> Update(Guid id, TourInputDto inputDto)
        {
            throw new NotImplementedException();
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
    }
}