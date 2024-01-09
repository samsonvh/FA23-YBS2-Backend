namespace YBS2
{
    public static class APIEndPoints
    {
        //  Default Route
        public const string API_VERSION_V1 = "/api/v1";
        public const string DEFAULT_ROUTE = "/[controller]";

        //  Authentication
        public const string AUTHENTICATION_V1 = API_VERSION_V1 + "/authentication";
        public const string AUTHENTICATION_GOOGLE_V1 = AUTHENTICATION_V1 + "/google";
        public const string AUTHENTICATION_CREDENTIALS_V1 = AUTHENTICATION_V1 + "/credentials";

        //  Companies
        public const string COMPANIES_V1 = API_VERSION_V1 + "/companies";
        public const string COMPANIES_ID_V1 = COMPANIES_V1 + "/{id}";

        // Membership Packages
        public const string MEMBERSHIP_PACKAGES_V1 = API_VERSION_V1 + "/membership-packages";
        public const string MEMBERSHIP_PACKAGES_ID_V1 = MEMBERSHIP_PACKAGES_V1 + "/{id}";

        //Members
        public const string MEMBER_V1 = API_VERSION_V1 + "/members";
        public const string MEMBER_ID_V1 = MEMBER_V1 + "/{id}";
        public const string MEMBER_ACTIVATE_V1 = MEMBER_V1 + "/activate";
        public const string MEMBER_CREATE_REGISTER_PAYMENT_URL = MEMBER_ID_V1 + "/create-payment-url";

        //  Update Requests
        public const string UPDATE_REQUESTS_V1 = API_VERSION_V1 + "/update-requests";
        public const string UPDATE_REQUESTS_ID_V1 = UPDATE_REQUESTS_V1 + "/{id}";
        public const string UPDATE_REQUESTS_OF_COMPANY_ID_V1 = COMPANIES_ID_V1 + "/update-requests";
        public const string UPDATE_REQUESTS_ID_OF_COMPANY_ID_V1 = UPDATE_REQUESTS_OF_COMPANY_ID_V1 + "/{updateRequestId}";


        //Docks
        public const string DOCK_V1 = API_VERSION_V1 + "/docks";
        public const string DOCK_ID_V1 = DOCK_V1 + "/{id}";
        //Yachts
        public const string YACHT_V1 = API_VERSION_V1 + "/yachts";
        public const string YACHT_ID_V1 = YACHT_V1 + "/{id}";
        //Tours
        public const string TOUR_V1 = API_VERSION_V1 + "/tours";
        public const string TOUR_ID_V1 = TOUR_V1 + "/{id}";
        
        //Bookings
        public const string BOOKING_V1 = API_VERSION_V1 + "/bookings";
        public const string BOOKING_ID_V1 = BOOKING_V1 + "/{id}";
        public const string BOOKING_CONFIRM = BOOKING_V1 + "/confirm";
        public const string BOOKING_CREATE_PAYMENT_URL = BOOKING_ID_V1 + "/create-payment-url";

        //Transactions
        public const string TRANSACTION_V1 = API_VERSION_V1 + "/transactions";
        public const string TRANSACTION_ID_V1 = TRANSACTION_V1 + "/{id}";
    }
}