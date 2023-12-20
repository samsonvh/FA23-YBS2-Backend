using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IDefaultService<P, L, T, I>
    {
        Task<DefaultPageResponse<L>> GetAll(P pageRequest);
        Task<T?> GetDetails(string name);
        Task<T?> Create(I inputDto);
        Task<T?> Update(I inputDto);
        Task<bool> ChangeStatus(string name);
        Task<bool> Delete(string name);
    }
}
