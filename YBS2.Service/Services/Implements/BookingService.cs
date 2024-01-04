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
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<BookingDto?> Create(BookingInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingDto?> Create(BookingInputDto inputDto, ClaimsPrincipal claims)
        {
            Tour? existingTour = await _unitOfWork.TourRepository
                .Find(tour => tour.Id == inputDto.TourId && tour.Status == EnumTourStatus.Active && tour.Yacht != null)
                .FirstOrDefaultAsync();
            if (existingTour == null)
            {
                dynamic errors = new ExpandoObject();
                errors.TourId = "Tour Not Found";
                throw new APIException(HttpStatusCode.BadRequest, errors.TourId, errors);
            }
            Booking booking = _mapper.Map<Booking>(inputDto);
            int totalPassengers = 0;
            List<Passenger> passengerList = _mapper.Map<List<Passenger>>(inputDto.Passengers);
            if (claims != null)
            {
                if (!inputDto.isIncludeBooker && passengerList.Count == 0)
                {
                    dynamic errors = new ExpandoObject();
                    errors.PassengerList = "Passenger List must not be null";
                    throw new APIException(HttpStatusCode.BadRequest, errors.PassengerList, errors);
                }
                string role = claims.FindFirstValue(ClaimTypes.Role);
                if (role == nameof(EnumRole.Member))
                {
                    Guid memberId = Guid.Parse(claims.FindFirstValue("MemberId"));
                    booking.MemberId = memberId;
                    if (inputDto.isIncludeBooker)
                    {
                        Member? member = await _unitOfWork.MemberRepository
                            .Find(member => member.Id == memberId && member.Account.Status == EnumAccountStatus.Active)
                            .FirstOrDefaultAsync();
                        if (member == null)
                        {
                            dynamic errors = new ExpandoObject();
                            errors.MemberId = $"Member with Id {memberId} does not exist";
                            throw new APIException(HttpStatusCode.BadRequest, errors.MemberId, errors);
                        }
                        Passenger memberInclude = new Passenger
                        {
                            DOB = member.DOB,
                            FullName = member.FullName,
                            Gender = member.Gender,
                            IdentityNumber = member.IdentityNumber
                        };
                        passengerList.Add(memberInclude);
                    }
                }
            }
            else
            {
                if (passengerList.Count == 0)
                {
                    dynamic errors = new ExpandoObject();
                    errors.PassengerList = "Passenger List must not be null";
                    throw new APIException(HttpStatusCode.BadRequest, errors.PassengerList, errors);
                }
            }
            totalPassengers += passengerList.Count();
            booking.Passengers = passengerList;
            booking.TotalPassengers = totalPassengers;
            booking.Status = EnumBookingStatus.Pending;
            booking.Tour = existingTour;
            booking.CreatedDate = DateTime.UtcNow.AddHours(7);
            _unitOfWork.BookingRepository.Add(booking);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BookingDto>(booking);
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DefaultPageResponse<BookingListingDto>> GetAll(BookingPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<BookingListingDto>> GetAll(BookingPageRequest pageRequest, ClaimsPrincipal claims)
        {
            IQueryable<Booking> query = _unitOfWork.BookingRepository.GetAll();
            // string role = claims.FindFirstValue(ClaimTypes.Role);
            // if (role == nameof(EnumRole.Company))
            // {
            //     Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            //     query = query.Where(booking => booking.Tour.CompanyId == companyId);
            // }
            // if (role == nameof(EnumRole.Member))
            // {
            //     Guid memberId = Guid.Parse(claims.FindFirstValue("MemberId"));
            //     query = query.Where(booking => booking.MemberId == memberId);
            // }
            query = Filter(query, pageRequest);
            int totalResults = await query.CountAsync();
            int pageCount = totalResults / pageRequest.PageSize + 1;
            List<BookingListingDto> list = await query
                                                    .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                                                    .Take(pageRequest.PageSize)
                                                    .Select(booking => _mapper.Map<BookingListingDto>(booking))
                                                    .ToListAsync();
            return new DefaultPageResponse<BookingListingDto>
            {
                Data = list,
                PageCount = pageCount,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize,
                TotalItem = totalResults
            };
        }

        public Task<BookingDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<BookingDto?> GetDetails(Guid id, ClaimsPrincipal claims)
        {
            Booking? booking = await _unitOfWork.BookingRepository
                .Find(booking => booking.Id == id)
                .Include(booking => booking.Member)
                .Include(booking => booking.Tour.Company)
                .FirstOrDefaultAsync();
            if (booking == null)
            {
                return null;
            }
            if (claims != null)
            {
                string role = claims.FindFirstValue(ClaimTypes.Role);
                if (role == nameof(EnumRole.Company) && booking.Tour.Company == null)
                {
                    return null;
                }
                if (role == nameof(EnumRole.Member) && booking.Member == null)
                {
                    return null;
                }
            }
            else
            {
                if (booking.Member != null)
                {
                    return null;
                }
            }

            return _mapper.Map<BookingDto>(booking);
        }

        public Task<BookingDto?> Update(Guid id, BookingInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Booking> Filter(IQueryable<Booking> query, BookingPageRequest pageRequest)
        {
            if (pageRequest.MinDate != null && pageRequest.MaxDate != null && DateTime.Compare((DateTime)pageRequest.MaxDate, (DateTime)pageRequest.MinDate) > 0)
            {
                query = query.Where(booking => booking.BookingDate <= pageRequest.MaxDate && booking.BookingDate >= pageRequest.MinDate);
            }
            if (pageRequest.Status.HasValue)
            {
                query = query.Where(booking => booking.Status == pageRequest.Status);
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