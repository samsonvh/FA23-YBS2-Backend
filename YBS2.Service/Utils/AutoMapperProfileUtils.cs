using AutoMapper;
using YBS2.Data.Models;
using YBS2.Service.Dtos.Listings;

namespace YBS2.Service.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            CreateMap<Account, AccountDto>();
            //company
            CreateMap<Company, CompanyListingDto>();
        }
    }
}