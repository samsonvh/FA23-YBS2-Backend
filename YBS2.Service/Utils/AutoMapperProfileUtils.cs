using AutoMapper;
using YBS2.Data.Models;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Details;
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
            CreateMap<Company, CompanyDto>()
                .ForMember(companyDto => companyDto.Username, options => options.MapFrom(company => company.Account.Username))
                .ForMember(companyDto => companyDto.Email, options => options.MapFrom(company => company.Account.Email))
                .ForMember(companyDto => companyDto.Status, options => options.MapFrom(company => MapDefaultStatus(company.Account.Status)));
            CreateMap<Company, CompanyListingDto>()
                .ForMember(companyListingDto => companyListingDto.Status, options => options.MapFrom(company => MapDefaultStatus(company.Account.Status)));
            // Member 
            CreateMap<Member,MemberListingDto>();
            CreateMap<Member,MemberDto>();
            CreateMap<MemberInputDto,Member>()
                .ForMember(member => member.AvatarURL,options => options.Ignore());
            CreateMap<MemberInputDto,Account>();
            //Dock
            CreateMap<Dock,DockListingDto>();
            CreateMap<Dock,DockDto>();
            CreateMap<DockInputDto,Dock>();
            //Yacht
            CreateMap<Yacht,YachtListingDto>();
            CreateMap<Yacht,YachtDto>();
            CreateMap<YachtInputDto,Yacht>();


            //  Update Request
            CreateMap<UpdateRequestInputDto, UpdateRequest>();
            CreateMap<UpdateRequest, UpdateRequestDto>()
                .ForMember(updateRequestDto => updateRequestDto.Status, options => options.MapFrom(updateRequest => MapDefaultStatus(updateRequest.Status)));
            CreateMap<UpdateRequest, UpdateRequestListingDto>()
                .ForMember(updateRequestDto => updateRequestDto.Status, options => options.MapFrom(updateRequest => MapDefaultStatus(updateRequest.Status)));

            //Membership Package
            CreateMap<MembershipPackage, MembershipPackageListingDto>();
            CreateMap<MembershipPackage, MembershipPackageDto>();
            CreateMap<MembershipPackageInputDto, MembershipPackage>();

            //Tour
            CreateMap<Tour, TourListingDto>();
            CreateMap<Tour, TourDto>();
            CreateMap<TourInputDto, Tour>();
        }

        private static string MapDefaultStatus(Enum status)
        {
            if (status is null)
            {
                throw new ArgumentNullException(nameof(status));
            }

            return status.ToString().ToUpper();
        }
    }
}