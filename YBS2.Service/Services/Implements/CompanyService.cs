using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services.Implements
{
    public class CompanyService : ICompanyService
    {
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

        public Task<DefaultPageResponse<CompanyListingDto>> GetAll(CompanyPageRequest pageRequest)
        {
            throw new NotImplementedException();
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
