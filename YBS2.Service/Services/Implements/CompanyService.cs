using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using YBS2.Data.Enums;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
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

        public Task<bool> ChangeStatus(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<CompanyDto?> Create(CompanyInputDto inputDto)
        {
            await CheckExistence(inputDto);

            string newPassword = "password";
            Account account = new Account
            {
                Email = inputDto.Email,
                Username = inputDto.Username,
                Password = PasswordUtils.HashPassword(newPassword),
                Role = nameof(EnumRole.Company),
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

        public Task<bool> Delete(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<CompanyListingDto>> GetAll(CompanyPageRequest pageRequest)
        {
            List<CompanyListingDto> list = await _unitOfWork.CompanyRepository
                .GetAll()
                .Select(company => _mapper.Map<CompanyListingDto>(company))
                .ToListAsync();
            DefaultPageResponse<CompanyListingDto> pageResponse = new DefaultPageResponse<CompanyListingDto>
            {
                Data = list,
                PageCount = list.Count,
                PageIndex = 0,
                PageSize = list.Count,
                TotalItem = list.Count
            };
            return pageResponse;
            // IQueryable<Company> query = _unitOfWork.CompanyRepository.GetAll();
            // if (pageRequest.Name != null)
            // {
            //     query = query.Where(company => company.Name.Trim().ToLower().Contains(pageRequest.Name.Trim().ToLower()));
            // }
            // if (pageRequest.HotLine != null)
            // {
            //     query = query.Where(company => company.HotLine.Trim().ToLower().Contains(pageRequest.HotLine.Trim().ToLower()));
            // }
            // var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
            // ? query.SortBy(pageRequest.OrderBy, pageRequest.IsAscending) : query.OrderBy(company => company.Id);
            // var totalItem = data.Count();
            // var pageCount = totalItem / pageRequest.PageSize + 1;
            // var dataPaging = await data.Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
            //                             .Take(pageRequest.PageSize)
            //                             .Select(company => _mapper.Map<CompanyListingDto>(company))
            //                             .ToListAsync();
            // if (dataPaging.Count > 0)
            // {
            //     DefaultPageResponse<CompanyListingDto> defaultPageResponse = new DefaultPageResponse<CompanyListingDto>
            //     {
            //         Data = dataPaging,
            //         PageCount = pageCount,
            //         PageIndex = pageRequest.PageIndex,
            //         PageSize = pageRequest.PageSize,
            //         TotalItem = dataPaging.Count
            //     };
            //     return defaultPageResponse;
            // }
            // return null;
        }

        public Task<CompanyDto?> GetDetails(Guid id, string name)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto?> Update(Guid id, CompanyInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        private async Task CheckExistence(CompanyInputDto inputDto)
        {
            Account? existingAccount = await _unitOfWork.AccountRepository
                .Find(account => account.Username == inputDto.Username || account.Email == inputDto.Email)
                .FirstOrDefaultAsync();
            if (existingAccount != null)
            {
                string message = "is unavailable";
                if (existingAccount.Email == inputDto.Email)
                {
                    message = "Email " + message;
                }
                if (existingAccount.Username == inputDto.Username)
                {
                    message = "Username " + message;
                }
                throw new APIException(HttpStatusCode.OK, message);
            }
            Company? existingCompany = await _unitOfWork.CompanyRepository
                .Find(company => company.Name == inputDto.Name)
                .FirstOrDefaultAsync();
            if (existingCompany != null)
            {
                string message = "is unavailable";
                if (existingCompany.Name == inputDto.Name)
                {
                    message = "Name " + message;
                }
                throw new APIException(HttpStatusCode.OK, message);
            }
        }
    }
}
