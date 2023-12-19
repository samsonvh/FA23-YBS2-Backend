using AutoMapper;
using YBS2.Data.Models;

namespace YBS2.Service.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            CreateMap<Account, AccountDto>();
        }
    }
}