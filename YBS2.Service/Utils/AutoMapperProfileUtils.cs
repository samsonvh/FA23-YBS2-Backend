using AutoMapper;
using YBS2.Data.Models;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Utils
{
    public class AutoMapperProfileUtils : Profile
    {
        public AutoMapperProfileUtils()
        {
            //  Account
            CreateMap<Account, AccountDto>();

            //  Company
            CreateMap<CompanyInputDto, Company>()
                .ForPath(company => company.Account.Username, options => options.MapFrom(companyInputDto => companyInputDto.ShortName));
            CreateMap<Company, CompanyDto>()
                .ForMember(companyDto => companyDto.Username, options => options.MapFrom(company => company.Account.Username))
                .ForMember(companyDto => companyDto.Email, options => options.MapFrom(company => company.Account.Email))
                .ForMember(companyDto => companyDto.Status, options => options.MapFrom(company => MapDefaultStatus(company.Account.Status)));
            CreateMap<Company, CompanyListingDto>()
                .ForMember(companyListingDto => companyListingDto.Status, options => options.MapFrom(company => MapDefaultStatus(company.Account.Status)));

            // Member 
            CreateMap<Member, MemberListingDto>();
            CreateMap<Member, MemberDto>();
            CreateMap<MemberInputDto, Member>()
                .ForMember(member => member.AvatarURL, options => options.Ignore());
            CreateMap<MemberRegistration, Member>();
            CreateMap<MemberInputDto, Account>();

            //Dock
            CreateMap<Dock, DockListingDto>();
            CreateMap<Dock, DockDto>();
            CreateMap<DockInputDto, Dock>()
                .ForMember(dock => dock.ImageURL, options => options.Ignore());
            CreateMap<Dock, TourDock>()
                .ForMember(tourDock => tourDock.Id, options => options.Ignore())
                .ForMember(tourDock => tourDock.DockId, options => options.MapFrom(dock => dock.Id));

            //Yacht
            CreateMap<Yacht, YachtListingDto>();
            CreateMap<Yacht, YachtDto>()
                .ForMember(yacht => yacht.ImageURL, options => options.Ignore());
            CreateMap<YachtInputDto, Yacht>()
                .ForMember(yacht => yacht.ImageURL, options => options.Ignore());;

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
            CreateMap<Tour, TourListingDto>()
                .ForMember(tourListingDto => tourListingDto.ImageURL, options => options.Ignore());
            CreateMap<Tour, TourDto>()
                .ForMember(tourDto => tourDto.ImageURLs, options => options.Ignore());
            CreateMap<TourInputDto, Tour>();

            //Booking
            CreateMap<Booking, BookingListingDto>();
            CreateMap<Booking, BookingDto>();
            CreateMap<BookingInputDto, Booking>()
                .ForMember(booking => booking.Passengers, options => options.Ignore());

            //Passenger
            CreateMap<PassengerInputDto, Passenger>();
            CreateMap<Passenger, PassengerDto>();
            CreateMap<Passenger, PassengerListingDto>();

            //Transaction 
            CreateMap<Transaction, TransactionListingDto>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionInputDto, Transaction>();

            //VNPayResponse 
            CreateMap<VNPayRegisterResponse,Transaction>();
            CreateMap<VNPayBookingResponse,Transaction>();

            //TourActivity
            CreateMap<TourActivityInputDto, TourActivity>();
            CreateMap<TourActivity, TourActivityDto>();
            CreateMap<TourActivity, TourActivityListingDto>();

            //BookingActivity
            CreateMap<TourActivity,BookingActivity>()
                .ForMember(bookingActivity => bookingActivity.Id, options => options.Ignore())
                .ForMember(bookingActivity => bookingActivity.BookingId, options => options.Ignore());
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