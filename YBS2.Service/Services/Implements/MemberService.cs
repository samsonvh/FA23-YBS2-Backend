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
        private readonly IFirebaseCMService _cloudMessagingService;
        private readonly IVNPayService _vnpayService;
        public MemberService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper, IFirebaseStorageService storageService, IFirebaseCMService cloudMessagingService, IVNPayService vnpayService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _storageService = storageService;
            _cloudMessagingService = cloudMessagingService;
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
                    existingMember.Status = EnumMemberStatus.Ban;
                    break;
                case nameof(EnumAccountStatus.Active):
                    existingMember.Account.Status = EnumAccountStatus.Active;
                    existingMember.Status = EnumMemberStatus.Active;
                    break;
                case nameof(EnumAccountStatus.Inactive):
                    existingMember.Account.Status = EnumAccountStatus.Inactive;
                    existingMember.Status = EnumMemberStatus.Inactive;
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

        public async Task<CreateMemberDto> Create(MemberRegistration registration, HttpContext context)
        {
            //validate member input field
            await CheckExistence(null, registration);
            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == registration.MembershipPackageId)
                .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.membershipPackage = "Membership Package Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.membershipPackage, Errors);
            }
            Account account = new Account
            {
                Email = registration.Email.Trim().ToLower(),
                Username = registration.Username.Trim().ToLower(),
                Password = PasswordUtils.HashPassword(registration.Password),
                Role = nameof(EnumRole.Member).ToUpper(),
                Status = EnumAccountStatus.Inactive
            };
            Member member = _mapper.Map<Member>(registration);
            member.Account = account;
            member.Status = EnumMemberStatus.Inactive;
            _unitOfWork.MemberRepository.Add(member);
            MembershipRegistration membershipRegistration = new MembershipRegistration
            {
                MemberId = member.Id,
                DeviceToken = registration.DeviceToken,
                Status = EnumMembershipRegistrationStatus.Inactive
            };
            _unitOfWork.MembershipRegistrationRepository.Add(membershipRegistration);
            await _unitOfWork.SaveChangesAsync();
            if (registration.Avatar != null)
            {
                string avatarName = member.Id.ToString();
                Uri avatarUri = await _storageService.UploadFile(avatarName, registration.Avatar);
                member.AvatarURL = avatarUri.ToString();
                _unitOfWork.MemberRepository.Update(member);
                await _unitOfWork.SaveChangesAsync();
            }
            string url = await _vnpayService.CreateRegisterRequestURL(membershipRegistration, existingMembershipPackage, context);
            return new CreateMemberDto
            {
                membershipRegistrationId = membershipRegistration.Id,
                paymentURL = url
            };
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
                    await FirebaseUtil.DeleteFile(existingMember.AvatarURL, _configuration, _storageService);
                    List<IFormFile> imgList = new List<IFormFile>();
                    imgList.Add(inputDto.Avatar);
                    string avatarURL = await FirebaseUtil.UpLoadFile(imgList, existingMember.Id, _storageService);
                    existingMember.AvatarURL = avatarURL;
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
        private async Task CheckExistence(MemberInputDto? inputDto, MemberRegistration? registration)
        {
            List<string> props = new List<string>();
            if (registration != null)
            {
                string username = registration.Username.ToLower();
                string email = registration.Email.ToLower();
                Account? existingAccount = await _unitOfWork.AccountRepository
                    .Find(account => account.Username == username || account.Email == email)
                    .FirstOrDefaultAsync();
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
            }
            if (inputDto != null)
            {
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
        }

        public async Task<bool> ActivateMember(IQueryCollection collections)
        {
            VNPayRegisterResponse vnpayResponse = await _vnpayService.CallBackRegisterPayment(collections);

            MembershipRegistration? existingMembershipRegistration = await _unitOfWork.MembershipRegistrationRepository
                .Find(membershipRegistration => membershipRegistration.Id == vnpayResponse.MembershipRegistrationId && membershipRegistration.Status == EnumMembershipRegistrationStatus.Inactive)
                .Include(membershipRegistration => membershipRegistration.Member)
                .Include(membershipRegistration => membershipRegistration.Member.Account)
                .FirstOrDefaultAsync();

            if (existingMembershipRegistration == null)
            {
                return false;
            }
            if (existingMembershipRegistration.Member == null)
            {
                return false;
            }
            if (existingMembershipRegistration.Member.Account == null)
            {
                return false;
            }
            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == vnpayResponse.MembershipPackageId)
                .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                return false;
            }
            Wallet wallet = new Wallet
            {
                Point = existingMembershipPackage.Point
            };

            DateTime now = DateTime.UtcNow.AddHours(7);
            existingMembershipRegistration.MembershipPackage = existingMembershipPackage;
            existingMembershipRegistration.Member.MemberSinceDate = now;
            existingMembershipRegistration.Member.Account.Status = EnumAccountStatus.Active;
            existingMembershipRegistration.Member.Status = EnumMemberStatus.Active;
            existingMembershipRegistration.Member.Wallet = wallet;
            existingMembershipRegistration.Name = existingMembershipRegistration.MembershipPackage.Name;
            existingMembershipRegistration.DiscountPercent = existingMembershipRegistration.MembershipPackage.DiscountPercent;
            existingMembershipRegistration.MembershipStartDate = now;
            existingMembershipRegistration.Status = EnumMembershipRegistrationStatus.Active;


            switch (existingMembershipRegistration.MembershipPackage.DurationUnit)
            {
                case EnumTimeUnit.Days:
                    existingMembershipRegistration.MembershipExpireDate = DateTime.UtcNow.AddHours(7).AddDays(existingMembershipRegistration.MembershipPackage.Duration);
                    break;
                case EnumTimeUnit.Months:
                    existingMembershipRegistration.MembershipExpireDate = DateTime.UtcNow.AddHours(7).AddMonths(existingMembershipRegistration.MembershipPackage.Duration);
                    break;
                case EnumTimeUnit.Years:
                    existingMembershipRegistration.MembershipExpireDate = DateTime.UtcNow.AddHours(7).AddYears(existingMembershipRegistration.MembershipPackage.Duration);
                    break;
            }
            Transaction transaction = _mapper.Map<Transaction>(vnpayResponse);
            transaction.Type = EnumTransactionType.Register;
            existingMembershipRegistration.Transaction = transaction;
            _unitOfWork.MembershipRegistrationRepository.Update(existingMembershipRegistration);

            await _unitOfWork.SaveChangesAsync();

            // FCMMessageRequest notiRequest = new FCMMessageRequest
            // {
            //     Title = existingMembershipRegistration.Id.ToString(),
            //     Body = "true",
            //     DeviceToken = existingMembershipRegistration.DeviceToken
            // };
            // await _cloudMessagingService.SendTransactionNotification(notiRequest);
            return true;
        }

        public Task<MemberDto?> Update(Guid id, MemberInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateRegisterPaymentURL(CreateRegisterPaymentURLInputDto inputDto, HttpContext context)
        {
            MembershipRegistration? existingMembershipRegistration = await _unitOfWork.MembershipRegistrationRepository
                .Find(membershipRegistration => membershipRegistration.Id == inputDto.MembershipRegistrationId)
                .Include(membershipRegistration => membershipRegistration.Member)
                .FirstOrDefaultAsync();
            if (existingMembershipRegistration == null)
            {
                dynamic errors = new ExpandoObject();
                errors.membershipRegistration = $"Membership registration with ID {inputDto.MembershipRegistrationId} not found";
                throw new APIException(HttpStatusCode.BadRequest, errors.membershipRegistration, errors);
            }
            if (existingMembershipRegistration.Member == null)
            {
                dynamic errors = new ExpandoObject();
                errors.member = "Member Not Found";
                throw new APIException(HttpStatusCode.BadRequest, errors.member, errors);
            }
            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == inputDto.MembershipPackageId)
                .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                dynamic errors = new ExpandoObject();
                errors.membershipPackage = $"Membership Package with ID {inputDto.MembershipPackageId} not found";
                throw new APIException(HttpStatusCode.BadRequest, errors.membershipPackage, errors);
            }
            string paymentURL = await _vnpayService.CreateRegisterRequestURL(existingMembershipRegistration, existingMembershipPackage, context);
            if (paymentURL == null)
            {
                dynamic errors = new ExpandoObject();
                errors.paymentURL = "Error while payment";
                throw new APIException(HttpStatusCode.BadRequest, errors.paymentURL, errors);
            }
            return paymentURL;
        }

        public async Task<string> CreateExtendMembershipRequestURL(ClaimsPrincipal claims, Guid membershipPackageId, HttpContext context)
        {
            Guid memberId = Guid.Parse(claims.FindFirstValue("MemberId"));
            Member? member = await _unitOfWork.MemberRepository
                .Find(member => member.Id == memberId)
                .FirstOrDefaultAsync();
            MembershipPackage? existingMembershipPackage = await _unitOfWork.MembershipPackageRepository
                .Find(membershipPackage => membershipPackage.Id == membershipPackageId && membershipPackage.Status == EnumMembershipPackageStatus.Active)
                .FirstOrDefaultAsync();
            if (existingMembershipPackage == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.membershipPackage = "Membership Package Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.membershipPackage,Errors);
            } 
            MembershipRegistration membershipRegistration = new MembershipRegistration
            {
                MemberId = memberId,
                Status = EnumMembershipRegistrationStatus.Inactive

            };
            _unitOfWork.MembershipRegistrationRepository.Add(membershipRegistration);
            await _unitOfWork.SaveChangesAsync();
            string paymentURL = await _vnpayService.CreateExtendMembershipRequestURL(member, existingMembershipPackage, context);
            return paymentURL;
        }
    }
}