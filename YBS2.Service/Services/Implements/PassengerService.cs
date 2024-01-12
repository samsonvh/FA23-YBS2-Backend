using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class PassengerService : IPassengerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PassengerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public Task<PassengerDto?> Create(PassengerInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DefaultPageResponse<PassengerListingDto>> GetAll(PassengerPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<PassengerListingDto>> GetAllOfBooking(Guid bookingId, PassengerPageRequest pageRequest, ClaimsPrincipal claims)
        {
            Booking? existingBooking = await _unitOfWork.BookingRepository
                .Find(booking => booking.Id == bookingId)
                .Include(booking => booking.Passengers)
                .FirstOrDefaultAsync();
            if (existingBooking == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.booking = "Booking Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.booking, Errors);
            }
            List<PassengerListingDto> passengerList = _mapper.Map<List<PassengerListingDto>>(existingBooking.Passengers);
            int totalResults = passengerList.Count;
            return new DefaultPageResponse<PassengerListingDto>
            {
                Data = passengerList,
                PageCount = totalResults / pageRequest.PageSize + 1,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize,
                TotalItem = totalResults
            };
        }

        public Task<PassengerDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PassengerDto?> Update(Guid id, PassengerInputDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}