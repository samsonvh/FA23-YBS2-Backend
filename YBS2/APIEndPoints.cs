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

        //  Update Requests
        public const string UPDATE_REQUESTS_V1 = API_VERSION_V1 + "/update-requests";
        public const string UPDATE_REQUESTS_ID_V1 = UPDATE_REQUESTS_V1 + "/{id}";
        public const string UPDATE_REQUESTS_OF_COMPANY_ID_V1 = COMPANIES_ID_V1 + "/update-requests";
        public const string UPDATE_REQUESTS_ID_OF_COMPANY_ID_V1 = UPDATE_REQUESTS_OF_COMPANY_ID_V1 + "/{updateRequestId}";

        //VNPay
        public const string VNPAY_REGISTER_V1 = API_VERSION_V1 + "/vnpays/register";

    }
}