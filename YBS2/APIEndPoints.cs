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
    }
}