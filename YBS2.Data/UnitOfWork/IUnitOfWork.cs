using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Models;
using YBS2.Data.Repository;

namespace YBS2.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepositoty<Account> AccountRepository { get; }
        IGenericRepositoty<Member> MemberRepository { get; }
        IGenericRepositoty<Role> RoleRepository { get; }
        IGenericRepositoty<Company> CompanyRepository { get; }
        Task<int> SaveChangesAsync();
    }
}