using YBS2.Data.Context;
using YBS2.Data.Models;
using YBS2.Data.Repository;
using YBS2.Data.Repository.Implement;

namespace YBS2.Data.UnitOfWork.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly YBS2Context _context;
        private readonly IGenericRepositoty<Account> _accountRepository;
        private readonly IGenericRepositoty<Company> _companyRepository;
        private readonly IGenericRepositoty<Member> _memberRepository;
        private readonly IGenericRepositoty<MembershipPackage> _membershipPackageRepository;
        private readonly IGenericRepositoty<MembershipRegistration> _membershipRegistrationRepository;
        private readonly IGenericRepositoty<UpdateRequest> _updateRequestRepository;
        private readonly IGenericRepositoty<Dock> _dockRepository;
        private readonly IGenericRepositoty<Yacht> _yachtRepository;
        private readonly IGenericRepositoty<Tour> _tourRepository;
        private readonly IGenericRepositoty<Booking> _bookingRepository;
        private readonly IGenericRepositoty<Transaction> _transactionRepository;
        private readonly IGenericRepositoty<Wallet> _walletRepository;
        public UnitOfWork(YBS2Context context)
        {
            _context = context;
        }
        public IGenericRepositoty<Account> AccountRepository
        {
            get
            {
                if (_accountRepository is not null)
                {
                    return _accountRepository;
                }
                return new GenericRepository<Account>(_context);
            }
        }

        public IGenericRepositoty<Company> CompanyRepository
        {
            get
            {
                if (_companyRepository is not null)
                {
                    return _companyRepository;
                }
                return new GenericRepository<Company>(_context);
            }
        }

        public IGenericRepositoty<Member> MemberRepository
        {
            get
            {
                if (_memberRepository is not null)
                {
                    return _memberRepository;
                }
                return new GenericRepository<Member>(_context);
            }
        }

        public IGenericRepositoty<MembershipPackage> MembershipPackageRepository
        {
            get
            {
                if (_membershipPackageRepository is not null)
                {
                    return _membershipPackageRepository;
                }
                return new GenericRepository<MembershipPackage>(_context);
            }
        }

        public IGenericRepositoty<MembershipRegistration> MembershipRegistrationRepository
        {
            get
            {
                if (_membershipRegistrationRepository is not null)
                {
                    return _membershipRegistrationRepository;
                }
                return new GenericRepository<MembershipRegistration>(_context);
            }
        }

        public IGenericRepositoty<UpdateRequest> UpdateRequestRepository
        {
            get
            {
                if (_updateRequestRepository is not null)
                {
                    return _updateRequestRepository;
                }
                return new GenericRepository<UpdateRequest>(_context);
            }
        }

        public IGenericRepositoty<Dock> DockRepository
        {
            get
            {
                if (_dockRepository is not null)
                {
                    return _dockRepository;
                }
                return new GenericRepository<Dock>(_context);
            }
        }

        public IGenericRepositoty<Yacht> YachtRepository
        {
            get
            {
                if (_yachtRepository is not null)
                {
                    return _yachtRepository;
                }
                return new GenericRepository<Yacht>(_context);
            }
        }

        public IGenericRepositoty<Tour> TourRepository
        {
            get
            {
                if (_tourRepository is not null)
                {
                    return _tourRepository;
                }
                return new GenericRepository<Tour>(_context);
            }
        }

        public IGenericRepositoty<Booking> BookingRepository
        {
            get
            {
                if (_bookingRepository is not null)
                {
                    return _bookingRepository;
                }
                return new GenericRepository<Booking>(_context);
            }
        }

        public IGenericRepositoty<Transaction> TransactionRepository
        {
            get
            {
                if (_transactionRepository is not null)
                {
                    return _transactionRepository;
                }
                return new GenericRepository<Transaction>(_context);
            }
        }

        public IGenericRepositoty<Wallet> WalletRepository
        {
            get
            {
                if (_walletRepository is not null)
                {
                    return _walletRepository;
                }
                return new GenericRepository<Wallet>(_context);
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}