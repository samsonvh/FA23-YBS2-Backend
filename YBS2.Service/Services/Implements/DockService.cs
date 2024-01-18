using System.Dynamic;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class DockService : IDockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _firebaseStorageService;
        private readonly IConfiguration _configuration;
        public DockService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseStorageService firebaseStorageService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
            _configuration = configuration;
        }
        public async Task<bool> ChangeStatus(Guid id, string status)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangeStatus(Guid id, string status, ClaimsPrincipal claims)
        {
            status = TextUtils.Capitalize(status);
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
            Dock? existingDock = await _unitOfWork.DockRepository
                .Find(dock => dock.Id == id && dock.CompanyId == companyId)
                .FirstOrDefaultAsync();
            if (existingDock == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Dock = "Dock Not Found";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
            }
            if (!Enum.IsDefined(typeof(EnumDockStatus), status))
            {
                dynamic Errors = new ExpandoObject();
                Errors.Status = $"Status {status} Is Invalid";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Status, Errors);
            }
            existingDock.Status = Enum.Parse<EnumDockStatus>(status);
            _unitOfWork.DockRepository.Update(existingDock);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public Task<DockDto?> Create(DockInputDto inputDto)
        {
            throw new NotImplementedException();
        }

        public async Task<DockDto?> Create(DockInputDto inputDto, ClaimsPrincipal claims)
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
            Dock dock = _mapper.Map<Dock>(inputDto);
            dock.Company = existingCompany;
            dock.Status = EnumDockStatus.Active;
            _unitOfWork.DockRepository.Add(dock);

            if (inputDto.Images.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Dock must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            string imageURL = await FirebaseUtil.UpLoadFile(inputDto.Images, dock.Id, _firebaseStorageService);
            if (imageURL == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Error while uploading file.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            dock.ImageURL = imageURL;
            await _unitOfWork.SaveChangesAsync();
            DockDto dockDto = _mapper.Map<DockDto>(dock);
            dockDto.ImageURLs = imageURL.Split(",");
            return dockDto;
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<DockListingDto>> GetAll(DockPageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<DockListingDto>> GetAll(DockPageRequest pageRequest, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            IQueryable<Dock> query = _unitOfWork.DockRepository.Find(dock => dock.CompanyId == companyId);
            query = Filter(query, pageRequest);
            List<Dock> list = await query
                .Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();
            List<DockListingDto> resultList = new List<DockListingDto>();
            foreach (Dock dock in list)
            {
                DockListingDto dockListingDto = _mapper.Map<DockListingDto>(dock);
                dockListingDto.ImageURL = dock.ImageURL.Split(',')[0];
                resultList.Add(dockListingDto);
            }
            int totalResults = await query.CountAsync();
            return new DefaultPageResponse<DockListingDto>
            {
                Data = resultList,
                PageCount = totalResults / pageRequest.PageSize + 1,
                PageIndex = pageRequest.PageIndex,
                PageSize = pageRequest.PageSize,
                TotalItem = totalResults
            };
        }

        public async Task<DockDto?> GetDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DockDto> GetDetails(Guid id, ClaimsPrincipal claims)
        {
            Guid companyId = Guid.Parse(claims.FindFirstValue("CompanyId"));
            Dock? dock = await _unitOfWork.DockRepository
                .Find(dock => dock.Id == id && dock.CompanyId == companyId)
                .FirstOrDefaultAsync();
            if (dock == null)
            {
                return null;
            }
            DockDto dockDto = _mapper.Map<DockDto>(dock);
            dockDto.ImageURLs = dock.ImageURL.Split(',');
            return dockDto;
        }

        public async Task<DockDto?> Update(Guid id, DockInputDto inputDto)
        {
            Dock? existingDock = await _unitOfWork.DockRepository
                .Find(dock => dock.Id == id && dock.Status == EnumDockStatus.Active)
                .FirstOrDefaultAsync();
            if (existingDock == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.Dock = "Dock Not Found.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.Dock, Errors);
            }
            if (inputDto.Images.Count == 0)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Tour must have at least 1 image.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            existingDock.Name = inputDto.Name;
            existingDock.Address = inputDto.Address;
            existingDock.Description = inputDto.Description;
            await FirebaseUtil.DeleteFile(existingDock.ImageURL, _configuration, _firebaseStorageService);

            string imageURL = await FirebaseUtil.UpLoadFile(inputDto.Images, existingDock.Id, _firebaseStorageService);
            if (imageURL == null)
            {
                dynamic Errors = new ExpandoObject();
                Errors.ImageURL = "Error while uploading file.";
                throw new APIException(HttpStatusCode.BadRequest, Errors.ImageURL, Errors);
            }
            existingDock.ImageURL = imageURL;
            _unitOfWork.DockRepository.Update(existingDock);
            await _unitOfWork.SaveChangesAsync();
            DockDto dockDto = _mapper.Map<DockDto>(existingDock);
            dockDto.ImageURLs = imageURL.Split(",");
            return dockDto;
        }

        private IQueryable<Dock> Filter(IQueryable<Dock> query, DockPageRequest pageRequest)
        {
            if (pageRequest.Name != null)
            {
                query = query.Where(dock => dock.Name.ToLower().Contains(pageRequest.Name.Trim().ToLower()));
            }
            if (pageRequest.Status.HasValue)
            {
                query = query.Where(dock => dock.Status == pageRequest.Status);
            }

            query = query.SortBy(pageRequest.OrderBy, pageRequest.IsDescending);

            return query;
        }

    }
}