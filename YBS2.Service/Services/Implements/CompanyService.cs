using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Net;
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
using YBS2.Service.Utils;

namespace YBS2.Service.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFirebaseStorageService _storageService;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork unitOfWork, IFirebaseStorageService storageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _mapper = mapper;
        }

        public async Task<bool> ChangeStatus(Guid id, string status)
        {
            Company? existingCompany = await _unitOfWork.CompanyRepository
                .Find(company => company.Id == id)
                .Include(company => company.Account)
                .FirstOrDefaultAsync();
            if (existingCompany == null)
            {   
                dynamic errors = new ExpandoObject();
                errors.CompanyId = $"Company with ID {existingCompany.Id} found";
                throw new APIException(HttpStatusCode.BadRequest, errors.CompanyId, errors);
            }

            status = TextUtils.Capitalize(status);
            if (!Enum.IsDefined(typeof(EnumAccountStatus), status))
            {
                dynamic errors = new ExpandoObject();
                errors.Status = $"Status {status} is invalid";
                throw new APIException(HttpStatusCode.BadRequest, errors.Status, errors);
            }
            existingCompany.Account.Status = Enum.Parse<EnumAccountStatus>(status);

            _unitOfWork.AccountRepository.Update(existingCompany.Account);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<CompanyDto?> Create(CompanyInputDto inputDto)
        {
            await CheckExistence(inputDto);

            string newPassword = "password";
            Account account = new Account
            {
                Email = inputDto.Email.ToLower(),
                Username = inputDto.ShortName,
                Password = PasswordUtils.HashPassword(newPassword),
                Role = nameof(EnumRole.Company).ToUpper(),
                Status = EnumAccountStatus.Inactive
            };
            Company company = _mapper.Map<Company>(inputDto);
            company.Account = account;
            _unitOfWork.CompanyRepository.Add(company);
            await _unitOfWork.SaveChangesAsync();

            string logoName = company.Id.ToString();
            Uri logoUri = await _storageService.UploadFile(logoName, inputDto.Logo);
            company.LogoURL = logoUri.ToString();
            _unitOfWork.CompanyRepository.Update(company);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CompanyDto>(company);
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<CompanyListingDto>> GetAll(CompanyPageRequest pageRequest)
        {
            IQueryable<Company> query = _unitOfWork.CompanyRepository.GetAll().Include(company => company.Account);
            query = Filter(query, pageRequest);

            int totalResults = await query.CountAsync();

            List<CompanyListingDto> list = await query
                .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(company => _mapper.Map<CompanyListingDto>(company))
                .ToListAsync();

            return new DefaultPageResponse<CompanyListingDto>
            {
                Data = list,
                PageIndex = pageRequest.PageIndex,
                PageCount = totalResults / pageRequest.PageSize + 1,
                PageSize = pageRequest.PageSize,
                TotalItem = totalResults
            };
        }

        public async Task<CompanyDto?> GetDetails(Guid id)
        {
            return await _unitOfWork.CompanyRepository
                .Find(company => company.Id == id)
                .Include(company => company.Account)
                .Select(company => _mapper.Map<CompanyDto>(company))
                .FirstOrDefaultAsync();
        }

        public Task<CompanyDto?> Update(Guid id, CompanyInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        private async Task CheckExistence(CompanyInputDto inputDto)
        {
            Account? existingAccount = await _unitOfWork.AccountRepository
                .Find(account => account.Username == inputDto.ShortName || account.Email == inputDto.Email)
                .FirstOrDefaultAsync();
            if (existingAccount != null)
            {
                dynamic errors = new ExpandoObject();
                List<string> props = new List<string>();
                if (existingAccount.Email == inputDto.Email)
                {
                    props.Add("Email");
                    errors.Email = new string[] { "Email is unavailable" };
                }
                if (existingAccount.Username == inputDto.ShortName)
                {
                    props.Add("Username");
                    errors.Username = new string[] { "Username is unavailable" };
                }
                string message = string.Join(", ", props) + " is unavailable";
                throw new APIException(HttpStatusCode.BadRequest, message, errors);
            }

            Company? existingCompany = await _unitOfWork.CompanyRepository
                .Find(company => company.Name == inputDto.Name)
                .FirstOrDefaultAsync();
            if (existingCompany != null)
            {
                dynamic errors = new ExpandoObject();
                string message = "is unavailable";
                if (existingCompany.Name == inputDto.Name)
                {
                    message = "Name " + message;
                    errors.Name = new string[] { "Name is unavailable" };
                }
                throw new APIException(HttpStatusCode.BadRequest, message, errors);
            }
        }

        private IQueryable<Company> Filter(IQueryable<Company> query, CompanyPageRequest pageRequest)
        {
            if (pageRequest.Name != null)
            {
                query = query.Where(company => company.Name.ToLower().Contains(pageRequest.Name.Trim().ToLower()));
            }

            if (pageRequest.Status != null && Enum.IsDefined(typeof(EnumAccountStatus), pageRequest.Status))
            {
                query = query.Where(company => company.Account.Status == pageRequest.Status);
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
