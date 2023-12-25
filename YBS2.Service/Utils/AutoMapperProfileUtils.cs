using AutoMapper;
using YBS2.Data.Models;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;

namespace YBS2.Service.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            //  Account
            CreateMap<Account, AccountDto>();

            //  Company
            CreateMap<CompanyInputDto, Company>();
            CreateMap<Company, CompanyDto>();
            CreateMap<Company, CompanyListingDto>();
            //Membership Package
            CreateMap<MembershipPackage,MembershipPackageListingDto>();
            CreateMap<MembershipPackage,MembershipPackageDto>();
            CreateMap<MembershipPackageInputDto,MembershipPackage>();
        }
    }
}