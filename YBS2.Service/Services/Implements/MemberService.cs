using System.Dynamic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using AutoMapper;
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
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFirebaseStorageService _storageService;
        private readonly IVNPayService _vnpayService;
        public MemberService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper, IFirebaseStorageService storageService, IVNPayService vnpayService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _storageService = storageService;
            _vnpayService = vnpayService;
        }
        public async Task<bool> ChangeStatus(Guid id, string status)
        {
            Member? existingMember = await _unitOfWork.MemberRepository.Find(member => member.Id == id)
                                                                        .Include(member => member.Account)
                                                                        .FirstOrDefaultAsync();
            if (existingMember == null)
            {
                dynamic errors = new ExpandoObject();
                errors.MemberId = $"Member with ID {id} not found";
                throw new APIException(HttpStatusCode.BadRequest, errors.MemberId, errors);
            }
            if (existingMember.Account.Status.ToString().ToUpper() == status.ToUpper())
            {
                return false;
            }
            switch (TextUtils.Capitalize(status))
            {
                case nameof(EnumAccountStatus.Ban):
                    existingMember.Account.Status = EnumAccountStatus.Ban;
                    break;
                case nameof(EnumAccountStatus.Active):
                    existingMember.Account.Status = EnumAccountStatus.Active;
                    break;
                case nameof(EnumAccountStatus.Inactive):
                    existingMember.Account.Status = EnumAccountStatus.Inactive;
                    break;
                default:
                    dynamic errors = new ExpandoObject();
                    errors.MemberStatus = "Invalid status";
                    throw new APIException(HttpStatusCode.BadRequest, errors.MemberStatus, errors);
            }
            _unitOfWork.AccountRepository.Update(existingMember.Account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<MemberDto?> Create(MemberInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Create(MemberInputDto inputDto, HttpContext context)
        {
            //validate member input field
            await CheckExistence(inputDto);
            Account account = new Account
            {
                Email = inputDto.Email.Trim().ToLower(),
                Username = inputDto.Username.Trim().ToLower(),
                Password = PasswordUtils.HashPassword(inputDto.Password),
                Role = nameof(EnumRole.Member).ToUpper(),
                Status = EnumAccountStatus.Inactive
            };
            Member member = _mapper.Map<Member>(inputDto);
            member.Account = account;
            member.Status = EnumMemberStatus.Inactive;
            _unitOfWork.MemberRepository.Add(member);
            await _unitOfWork.SaveChangesAsync();
            if (inputDto.Avatar != null)
            {
                string avatarName = member.Id.ToString();
                Uri avatarUri = await _storageService.UploadFile(avatarName, inputDto.Avatar);
                member.AvatarURL = avatarUri.ToString();
                _unitOfWork.MemberRepository.Update(member);
                await _unitOfWork.SaveChangesAsync();
            }
            string url = await _vnpayService.CreateRegisterRequestURL(inputDto.MembershipPackageId, member.Id, context);
            dynamic memberDto = new ExpandoObject();
            memberDto.Id = member.Id;
            memberDto.FullName = member.FullName;
            memberDto.AvatarURL = member.AvatarURL;
            memberDto.DOB = member.DOB;
            memberDto.Address = member.Address;
            memberDto.PhoneNumber = member.PhoneNumber;
            memberDto.IdentityNumber = member.IdentityNumber;
            memberDto.Gender = member.Gender.ToString();
            memberDto.Nationality = member.Nationality;
            memberDto.MemberSinceDate = member.MemberSinceDate;
            memberDto.Status = member.Status.ToString();
            memberDto.CreatedDate = member.CreatedDate;
            memberDto.PaymentURL = url;
            return memberDto;
        }


        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<MemberListingDto>> GetAll(MemberPageRequest pageRequest)
        {
            IQueryable<Member> query = _unitOfWork.MemberRepository.GetAll();
            query = FilterGetAll(query, pageRequest);

            int totalCount = query.Count();

            int pageCount = totalCount / pageRequest.PageSize + 1;
            List<MemberListingDto> list = await query
                                                    .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                                                    .Take(pageRequest.PageSize)
                                                    .Select(member => _mapper.Map<MemberListingDto>(member))
                                                    .ToListAsync();

            DefaultPageResponse<MemberListingDto> pageResponse =
            new DefaultPageResponse<MemberListingDto>
            {
                Data = list,
                PageCount = pageCount,
                TotalItem = totalCount,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize
            };
            return pageResponse;
        }

        public async Task<MemberDto?> GetDetails(Guid id)
        {
            MemberDto? memberDto = await _unitOfWork.MemberRepository.Find(member => member.Id == id)
                                                                    .Select(member => _mapper.Map<MemberDto>(member))
                                                                    .FirstOrDefaultAsync();
            return memberDto;
        }

        public async Task<MemberDto?> Update(MemberInputDto inputDto, ClaimsPrincipal claims)
        {
            Guid id = Guid.Parse(claims.FindFirstValue("MemberId"));
            Member? existingMember = await _unitOfWork.MemberRepository.Find(member => member.Id == id && member.Account.Status == EnumAccountStatus.Active)
                                                        .FirstOrDefaultAsync();
            if (existingMember != null)
            {
                if (inputDto.Address != null)
                {
                    existingMember.Address = inputDto.Address;
                }
                existingMember.IdentityNumber = inputDto.IdentityNumber;
                existingMember.PhoneNumber = inputDto.PhoneNumber;
                existingMember.DOB = inputDto.DOB;
                existingMember.FullName = inputDto.FullName;
                existingMember.Nationality = inputDto.Nationality;

                if (inputDto.Avatar != null)
                {
                    string avatarName = existingMember.Id.ToString();
                    Uri avatarUri = await _storageService.UploadFile(avatarName, inputDto.Avatar);
                    existingMember.AvatarURL = avatarName.ToString();
                }

                _unitOfWork.MemberRepository.Update(existingMember);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<MemberDto>(existingMember);

            }

            return null;
        }

        private IQueryable<Member> FilterGetAll(IQueryable<Member> query, MemberPageRequest pageRequest)
        {
            if (pageRequest.FullName != null)
            {
                query = query.Where(member => member.FullName.Trim().ToLower().Contains(pageRequest.FullName.ToLower()));
            }

            if (pageRequest.PhoneNumber != null)
            {
                query = query.Where(member => member.PhoneNumber.Contains(pageRequest.PhoneNumber));
            }

            if (pageRequest.Status != null && Enum.IsDefined(typeof(EnumMemberStatus), pageRequest.Status))
            {
                query = query.Where(member => member.Status == pageRequest.Status);
            }
            query = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
                    ? query.SortBy(pageRequest.OrderBy, pageRequest.IsDescending)
                    : pageRequest.IsDescending
                    ? query.OrderBy(member => member.Id)
                    : query.OrderByDescending(member => member.Id);
            return query;
        }
        private async Task CheckExistence(MemberInputDto inputDto)
        {
            string username = inputDto.Username.ToLower();
            string email = inputDto.Email.ToLower();
            Account? existingAccount = await _unitOfWork.AccountRepository
                .Find(account => account.Username == username || account.Email == email)
                .FirstOrDefaultAsync();
            List<string> props = new List<string>();
            if (existingAccount != null)
            {
                dynamic errors = new ExpandoObject();
                string message = " is unavailable";
                if (existingAccount.Email == email)
                {
                    props.Add("Email");
                    errors.Email = "Email" + message;
                }
                if (existingAccount.Username == username)
                {
                    props.Add("Username");
                    errors.Username = "Username" + message;
                }
                message = string.Join(",", props) + message;
                throw new APIException(HttpStatusCode.BadRequest, message, errors);
            }
            Member? existingMember = await _unitOfWork.MemberRepository
                .Find(member => member.PhoneNumber == inputDto.PhoneNumber || member.IdentityNumber == inputDto.IdentityNumber)
                .FirstOrDefaultAsync();
            if (existingMember != null)
            {
                dynamic errors = new ExpandoObject();
                string message = " is unavailable";
                if (existingMember.PhoneNumber == inputDto.PhoneNumber)
                {
                    props.Add("PhoneNumber");
                    errors.PhoneNumber = "PhoneNumber" + message;
                }
                if (existingMember.IdentityNumber == inputDto.IdentityNumber)
                {
                    props.Add("IdentityNumber");
                    errors.IdentityNumber = "IdentityNumber" + message;
                }
                message = string.Join(",", props) + message;
                throw new APIException(HttpStatusCode.BadRequest, message, errors);
            }
        }

        public async Task<bool> ActivateMember(IQueryCollection collections)
        {
            VNPayRegisterResponse vnpayResponse = await _vnpayService.CallBackRegisterPayment(collections);

            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == vnpayResponse.MembershipPackageId && membershipPackage.Status == EnumMembershipPackageStatus.Active)
                .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                return false;
            }
            Member? existingMember = await _unitOfWork.MemberRepository
                .Find(member => member.Id == vnpayResponse.MemberId)
                .Include(member => member.Account)
                .FirstOrDefaultAsync();
            if (existingMember == null)
            {
                return false;
            }
            Wallet wallet = new Wallet
            {
                Point = existingMembershipPackage.Point
            };

            DateTime now = DateTime.UtcNow.AddHours(7);
            existingMember.MemberSinceDate = now;
            existingMember.Account.Status = EnumAccountStatus.Active;
            existingMember.Status = EnumMemberStatus.Active;
            existingMember.Wallet = wallet;
            _unitOfWork.MemberRepository.Update(existingMember);

            MembershipRegistration membershipRegistration = new MembershipRegistration
            {
                Member = existingMember,
                MembershipPackage = existingMembershipPackage,
                MembershipStartDate = DateTime.UtcNow.AddHours(7),
                DiscountPercent = existingMembershipPackage.DiscountPercent,
                Name = existingMembershipPackage.Name,
                Status = EnumMembershipRegistrationStatus.Active
            };
            switch (existingMembershipPackage.DurationUnit)
            {
                case EnumTimeUnit.Days:
                    membershipRegistration.MembershipExpireDate = DateTime.UtcNow.AddHours(7).AddDays(existingMembershipPackage.Duration);
                    break;
                case EnumTimeUnit.Months:
                    membershipRegistration.MembershipExpireDate = DateTime.UtcNow.AddHours(7).AddMonths(existingMembershipPackage.Duration);
                    break;
                case EnumTimeUnit.Years:
                    membershipRegistration.MembershipExpireDate = DateTime.UtcNow.AddHours(7).AddYears(existingMembershipPackage.Duration);
                    break;
            }
            _unitOfWork.MembershipRegistrationRepository.Add(membershipRegistration);


            Transaction transaction = _mapper.Map<Transaction>(vnpayResponse);
            transaction.MembershipRegistration = membershipRegistration;
            transaction.Type = EnumTransactionType.Register;
            _unitOfWork.TransactionRepository.Add(transaction);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public Task<MemberDto?> Update(Guid id, MemberInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateRegisterPaymentURL(RegisterPaymentInputDto inputDto, HttpContext context)
        {
            Member? existingMember = await _unitOfWork.MemberRepository
                .Find(member => member.Id == inputDto.MemberId)
                .FirstOrDefaultAsync();
            if (existingMember == null)
            {
                dynamic errors = new ExpandoObject();
                errors.MemberId = $"Member with ID {existingMember.Id} not found";
                throw new APIException(HttpStatusCode.BadRequest, errors.MemberId, errors);
            }
            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == inputDto.MembershipPackageId && membershipPackage.Status == EnumMembershipPackageStatus.Active)
                .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                dynamic errors = new ExpandoObject();
                errors.MembershipPackageId = $"Membership Package with ID {existingMembershipPackage.Id} not found";
                throw new APIException(HttpStatusCode.BadRequest, errors.MembershipPackageId, errors);
            }
            string paymentURL = await _vnpayService.CreateRegisterRequestURL(existingMembershipPackage.Id, existingMember.Id, context);
            if (paymentURL == null)
            {
                dynamic errors = new ExpandoObject();
                errors.paymentURL = "Error while payment";
                throw new APIException(HttpStatusCode.BadRequest, errors.PaymentURL, errors);
            }
            return paymentURL;
        }
    }
}