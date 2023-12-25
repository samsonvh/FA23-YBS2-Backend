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

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}