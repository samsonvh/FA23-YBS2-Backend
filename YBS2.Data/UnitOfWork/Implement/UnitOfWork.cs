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
        private readonly IGenericRepositoty<Member> _memberRepository;
        private readonly IGenericRepositoty<Role> _roleRepository;
        private readonly IGenericRepositoty<Company> _companyRepository;
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

        public IGenericRepositoty<Role> RoleRepository
        {
            get
            {
                if (_roleRepository is not null)
                {
                    return _roleRepository;
                }
                return new GenericRepository<Role>(_context);
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

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}