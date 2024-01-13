using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using YBS2.Service.Utils;

namespace YBS2.Service.Services.Implements
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IVNPayService _vnpayService;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IVNPayService vnpayService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _vnpayService = vnpayService;
        }
        public async Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<BookingDto?> Create(BookingInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Create(BookingInputDto inputDto, ClaimsPrincipal claims, HttpContext context)
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

            List<Booking> bookingList = await _unitOfWork.BookingRepository
                .Find(booking => booking.BookingDate.Date.Equals(inputDto.BookingDate.Date) && booking.TourId == existingTour.Id && booking.Status == EnumBookingStatus.Approved)
                .ToListAsync();
            if (bookingList.FindAll(booking => booking.Type == EnumBookingType.Private_Tour).Count > 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.privateTour = $"This tour already have private booking in date: {inputDto.BookingDate.Date.ToString("MM/dd/yyyy")}";
                throw new APIException(HttpStatusCode.BadRequest, Errors.privateTour, Errors);
            }


            Booking booking = await AddPassengerInBooking(inputDto, claims, existingTour);
            List<Booking> publicBookingList = bookingList.FindAll(booking => booking.Type == EnumBookingType.Group_Tour);
            int count = booking.TotalPassengers;
            foreach (Booking previousBooking in publicBookingList)
            {
                count += previousBooking.TotalPassengers;
            }
            if (count > existingTour.MaximumGuest)
            {
                dynamic Errors = new ExpandoObject();
                Errors.outOfSeat = $"This tour is out of seat in date: {inputDto.BookingDate.Date.ToString("MM/dd/yyyy")}";
                throw new APIException(HttpStatusCode.BadRequest, Errors.outOfSeat, Errors);
            }
            booking.Status = EnumBookingStatus.Pending;
            booking.PaymentStatus = EnumPaymentStatus.NotYet;
            booking.Tour = existingTour;
            _unitOfWork.BookingRepository.Add(booking);
            await _unitOfWork.SaveChangesAsync();
            dynamic bookingDto = new ExpandoObject();
            bookingDto.Id = booking.Id;
            if (booking.MemberId != null)
            {
                bookingDto.MemberId = booking.MemberId;
            }
            bookingDto.tourId = booking.TourId;
            bookingDto.bookingDate = booking.BookingDate;
            bookingDto.totalAmount = booking.TotalAmount;
            bookingDto.totalPassengers = booking.TotalPassengers;
            bookingDto.note = booking.Note;
            bookingDto.isIncludeBooker = booking.isIncludeBooker;
            bookingDto.type = booking.Type.ToString();
            bookingDto.status = booking.Status.ToString();
            bookingDto.paymentStatus = booking.PaymentStatus.ToString();
            bookingDto.paymentMethod = booking.PaymentMethod.ToString();
            bookingDto.createdDate = booking.CreatedDate;
            if (claims != null && booking.Type != EnumBookingType.Private_Tour && booking.PaymentMethod == EnumPaymentMethod.Transfer)
            {
                bookingDto.PaymentURL = await _vnpayService.CreateBookingRequestURL(booking.Id, context);
            }
            return bookingDto;
        }

        private async Task<Booking> AddPassengerInBooking(BookingInputDto inputDto, ClaimsPrincipal claims, Tour existingTour)
        {
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
                    booking.PaymentMethod = inputDto.PaymentMethod;
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
                booking.PaymentMethod = EnumPaymentMethod.Cash;
            }
            float totalAmount = existingTour.Price * passengerList.Count();
            booking.TotalPassengers = passengerList.Count();
            booking.Passengers = passengerList;
            booking.TotalAmount = totalAmount;
            return booking;
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
            string role = TextUtils.Capitalize(claims.FindFirstValue(ClaimTypes.Role));
            if (role == nameof(EnumRole.Company))
            {
                Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                query = query.Where(booking => booking.Tour.CompanyId == companyId);
            }
            if (role == nameof(EnumRole.Member))
            {
                Guid memberId = Guid.Parse(claims.FindFirstValue("MemberId"));
                query = query.Where(booking => booking.MemberId == memberId);
            }
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
            IQueryable<Booking> query = _unitOfWork.BookingRepository.Find(booking => booking.Id == id);
            if (claims != null)
            {
                string role = TextUtils.Capitalize(claims.FindFirstValue(ClaimTypes.Role));
                if (role == nameof(EnumRole.Company))
                {
                    Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
                    query = query.Where(booking => booking.Tour.CompanyId == companyId);
                }
                else if (role == nameof(EnumRole.Member))
                {
                    Guid memberId = Guid.Parse(claims.FindFirstValue("MemberId"));
                    query = query.Where(booking => booking.MemberId == memberId);
                }
            }
            Booking booking = await query.FirstOrDefaultAsync();
            if (booking == null)
            {
                return null;
            }
            else if (claims == null && booking.MemberId != null)
            {
                return null;
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

        public async Task<BookingDto> ConfirmBooking(IQueryCollection collections)
        {
            VNPayBookingResponse vnpayResponse = await _vnpayService.CallBackBookingPayment(collections);
            Member? existingMember = await _unitOfWork.MemberRepository
                .Find(member => member.Id == vnpayResponse.MemberId && member.Status == EnumMemberStatus.Active)
                .FirstOrDefaultAsync();
            if (existingMember == null)
            {
                dynamic errors = new ExpandoObject();
                errors.Member = "Member Not Found";
                throw new APIException(HttpStatusCode.BadRequest, errors.Member, errors);
            }
            Booking? existingBooking = await _unitOfWork.BookingRepository
                .Find(booking => booking.Id == vnpayResponse.BookingId)
                .FirstOrDefaultAsync();
            if (existingBooking == null)
            {
                dynamic errors = new ExpandoObject();
                errors.Booking = "Booking Not Found";
                throw new APIException(HttpStatusCode.BadRequest, errors.Booking, errors);
            }
            DateTime now = DateTime.UtcNow.AddHours(7);
            existingBooking.PaymentStatus = EnumPaymentStatus.Done;
            _unitOfWork.BookingRepository.Update(existingBooking);
            Transaction transaction = _mapper.Map<Transaction>(vnpayResponse);
            transaction.Type = EnumTransactionType.Booking;
            _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BookingDto>(existingBooking);
        }

        public async Task<string> CreateBookingPaymentURL(Guid id, HttpContext context)
        {
            Booking? existingBooking = await _unitOfWork.BookingRepository
                .Find(booking => booking.Id == id)
                .FirstOrDefaultAsync();
            if (existingBooking == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Booking = "Booking Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Booking, Errors);
            }
            if (existingBooking.Status != EnumBookingStatus.Pending || existingBooking.PaymentStatus != EnumPaymentStatus.NotYet)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Booking = "Booking is paid already";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Booking, Errors);
            }
            if (existingBooking.PaymentMethod == EnumPaymentMethod.Cash)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Booking = "Payment method of booking is by cash, can not pay by transfer";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Booking, Errors);
            }
            string paymentURL = await _vnpayService.CreateBookingRequestURL(id, context);
            if (paymentURL == null)
            {
                dynamic errors = new ExpandoObject();
                errors.paymentURL = "Error while payment";
                throw new APIException(HttpStatusCode.BadRequest, errors.PaymentURL, errors);
            }
            return paymentURL;
        }

        public async Task<bool> ChangeStatus(Guid id, string status, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Company? existingCompany = await _unitOfWork.CompanyRepository.Find(company => company.Id == companyId).FirstOrDefaultAsync();
            if (existingCompany == null)
            {
                dynamic errors = new ExpandoObject();
                errors.company = $"Company with ID {companyId} found";
                throw new APIException(HttpStatusCode.BadRequest, errors.company, errors);
            }
            Booking? existingBooking = await _unitOfWork.BookingRepository
            .Find(booking => booking.Id == id && booking.Tour.CompanyId == companyId)
            .FirstOrDefaultAsync();
            if (existingBooking == null)
            {
                dynamic errors = new ExpandoObject();
                errors.booking = $"Booking with ID {id} found";
                throw new APIException(HttpStatusCode.BadRequest, errors.booking, errors);
            }

            status = TextUtils.Capitalize(status);
            if (!Enum.IsDefined(typeof(EnumBookingStatus), status))
            {
                dynamic errors = new ExpandoObject();
                errors.status = $"Status {status} is invalid";
                throw new APIException(HttpStatusCode.BadRequest, errors.status, errors);
            }
            existingBooking.Status = Enum.Parse<EnumBookingStatus>(status);

            _unitOfWork.BookingRepository.Update(existingBooking);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }


    }
}