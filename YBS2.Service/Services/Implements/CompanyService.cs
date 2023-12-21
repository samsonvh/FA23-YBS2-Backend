using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YBS.Service.Utils;
using YBS2.Data.Models;
using YBS2.Data.UnitOfWork;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services.Implements
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CompanyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<bool> ChangeStatus(string name)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto?> Create(CompanyIndutDto inputDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<DefaultPageResponse<CompanyListingDto>?> GetAll(CompanyPageRequest pageRequest)
        {
            IQueryable<Company> query = _unitOfWork.CompanyRepository.GetAll();
            if (pageRequest.Name != null)
            {
                query = query.Where(company => company.Name.Trim().ToLower().Contains(pageRequest.Name.Trim().ToLower()));
            }
            if (pageRequest.HotLine != null)
            {
                query = query.Where(company => company.HotLine.Trim().ToLower().Contains(pageRequest.HotLine.Trim().ToLower()));
            }
            var data = !string.IsNullOrWhiteSpace(pageRequest.OrderBy)
            ? query.SortBy(pageRequest.OrderBy, pageRequest.IsAscending) : query.OrderBy(company => company.Id);
            var totalItem = data.Count();
            var pageCount = totalItem / pageRequest.PageSize + 1;
            var dataPaging = await data.Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                                        .Take(pageRequest.PageSize)
                                        .Select(company => _mapper.Map<CompanyListingDto>(company))
                                        .ToListAsync();
            if (dataPaging.Count > 0)
            {
                DefaultPageResponse<CompanyListingDto> defaultPageResponse = new DefaultPageResponse<CompanyListingDto>
                {
                    Data = dataPaging,
                    PageCount = pageCount,
                    PageIndex = pageRequest.PageIndex,
                    PageSize = pageRequest.PageSize,
                    TotalItem = dataPaging.Count
                };
                return defaultPageResponse;
            }
            return null;
        }

        public Task<CompanyDto?> GetDetails(string name)
        {
            throw new NotImplementedException();
        }

        public Task<CompanyDto?> Update(CompanyIndutDto inputDto)
        {
            throw new NotImplementedException();
        }
    }
}
