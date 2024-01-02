using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionDto?> Create(TransactionInputDto inputDto)
        {
            if (inputDto.BookingId == null && (inputDto.MemberId == null || inputDto.MembershipPackageId == null))
            {
                dynamic Errors = new ExpandoObject();
                Errors.Id = "Requirement Id null";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Id, Errors);
            }
            Transaction transaction = _mapper.Map<Transaction>(inputDto);
            if (inputDto.BookingId != null)
            {
                //check existing booking
                Booking? existingBooking = await _unitOfWork.BookingRepository
                    .Find(booking => booking.Id == inputDto.BookingId)
                    .FirstOrDefaultAsync();
                if (existingBooking == null)
                {
                    dynamic Errors = new ExpandoObject();
                    Errors.BookingId = $"Booking with Id {inputDto.BookingId} not found.";
                    throw new APIException(HttpStatusCode.BadRequest, Errors.BookingId, Errors);
                }
                //update existing booking 
                existingBooking.Status = EnumBookingStatus.Approved;
                _unitOfWork.BookingRepository.Update(existingBooking);
                //
                transaction.BookingId = existingBooking.Id;
                transaction.Type = EnumTransactionType.Booking;

            }
            else if (inputDto.MemberId != null && inputDto.MembershipPackageId != null)
            {
                //check exsitingMember
                Member? existingMember = await _unitOfWork.MemberRepository
                    .Find(member => member.Id == inputDto.MemberId)
                    .FirstOrDefaultAsync();
                if (existingMember == null)
                {
                    dynamic Errors = new ExpandoObject();
                    Errors.MemberId = $"Member with Id {inputDto.MemberId} not found.";
                    throw new APIException(HttpStatusCode.BadRequest, Errors.MemberId, Errors);
                }
                //check exsitingMembershipPackage
                MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                    .Find(membershipPackage => membershipPackage.Id == inputDto.MembershipPackageId)
                    .FirstOrDefaultAsync();
                if (existingMembershipPackage == null)
                {
                    dynamic Errors = new ExpandoObject();
                    Errors.MembershipPackageId = $"Membership Package with Id {inputDto.MembershipPackageId} not found.";
                    throw new APIException(HttpStatusCode.BadRequest, Errors.MembershipPackageId, Errors);
                }
                DateTime now = DateTime.UtcNow.AddHours(7);
                //update Member
                existingMember.MemberSinceDate = now;
                _unitOfWork.MemberRepository.Update(existingMember);
                //Add Membership Registration
                MembershipRegistration membershipRegistration = new MembershipRegistration
                {
                    Member = existingMember,
                    MembershipPackage = existingMembershipPackage,
                    Name = existingMembershipPackage.Name,
                    DiscountPercent = existingMembershipPackage.DiscountPercent,
                    MembershipStartDate = now,
                    Status = EnumMembershipRegistrationStatus.Active,
                };
                switch (existingMembershipPackage.DurationUnit)
                {
                    case "ngày":
                        membershipRegistration.MembershipExpireDate = now.AddDays(existingMembershipPackage.Duration);
                        break;
                    case "tháng":
                        membershipRegistration.MembershipExpireDate = now.AddMonths(existingMembershipPackage.Duration);
                        break;
                    case "năm":
                        membershipRegistration.MembershipExpireDate = now.AddYears(existingMembershipPackage.Duration);
                        break;
                }
                _unitOfWork.MembershipRegistrationRepository.Add(membershipRegistration);
                //add Wallet
                Wallet wallet = new Wallet
                {
                    Member = existingMember,
                    Point = existingMembershipPackage.Point
                };
                _unitOfWork.WalletRepository.Add(wallet);
                //
                transaction.MembershipRegistration = membershipRegistration;
                transaction.Type = EnumTransactionType.Register;
            }
            _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DefaultPageResponse<TransactionListingDto>> GetAll(TransactionPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionDto?> Update(Guid id, TransactionInputDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}