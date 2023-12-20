using YBS2.Data.Models;
using YBS2.Data.Repository;

namespace YBS2.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepositoty<Account> AccountRepository { get; }
        IGenericRepositoty<Company> CompanyRepository { get; }
        Task<int> SaveChangesAsync();
    }
}