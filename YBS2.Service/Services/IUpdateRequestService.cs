using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IUpdateRequestService : IDefaultService<UpdateRequestPageRequest, UpdateRequestListingDto, UpdateRequestDto, UpdateRequestInputDto>
    {
        Task<UpdateRequestDto?> Create(UpdateRequestInputDto inputDto, Guid companyId);
        Task<DefaultPageResponse<UpdateRequestListingDto>> GetAll(UpdateRequestPageRequest pageRequest, Guid companyId);
        Task<UpdateRequestDto?> GetDetails(Guid id, Guid companyId);
    }
}
