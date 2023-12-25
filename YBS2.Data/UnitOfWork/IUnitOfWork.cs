using YBS2.Data.Models;
using YBS2.Data.Repository;

namespace YBS2.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepositoty<Account> AccountRepository { get; }
        IGenericRepositoty<Company> CompanyRepository { get; }
        IGenericRepositoty<Member> MemberRepository { get; }
        IGenericRepositoty<MembershipPackage> MembershipPackageRepository { get; }
        IGenericRepositoty<MembershipRegistration> MembershipRegistrationRepository { get; }
        IGenericRepositoty<UpdateRequest> UpdateRequestRepository { get; }
        Task<int> SaveChangesAsync();
    }
}