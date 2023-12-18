using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using YBS2.Data.Models;

namespace YBS2.Service.Utils
{
    public class AutoMapperProfileUtil : Profile
    {
        public AutoMapperProfileUtil()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}