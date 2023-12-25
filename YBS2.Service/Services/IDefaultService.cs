using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IDefaultService<P, L, T, I>
    {
        Task<DefaultPageResponse<L>> GetAll(P pageRequest);
        Task<T?> GetDetails(Guid id, string name);
        Task<T?> Create(I inputDto);
        Task<T?> Update(Guid id, I inputDto);
        Task<bool> ChangeStatus(Guid id, string name);
        Task<bool> Delete(Guid id, string name);
    }
}
