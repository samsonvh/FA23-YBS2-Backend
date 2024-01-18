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
        IGenericRepositoty<Dock> DockRepository { get; }
        IGenericRepositoty<Yacht> YachtRepository { get; }
        IGenericRepositoty<Tour> TourRepository { get; }
        IGenericRepositoty<Booking> BookingRepository { get; }
        IGenericRepositoty<Transaction> TransactionRepository { get; }
        IGenericRepositoty<Wallet> WalletRepository { get; }
        IGenericRepositoty<TourActivity> TourActivityRepository { get; }
        IGenericRepositoty<BookingActivity> BookingActivityRepository { get; }
        IGenericRepositoty<Service> ServiceRepository { get; }
        IGenericRepositoty<ServicePackage> ServicePackageRepository { get; }
        IGenericRepositoty<ServicePackageItem> ServicePackageItemRepository { get; }
        Task<int> SaveChangesAsync();
    }
}