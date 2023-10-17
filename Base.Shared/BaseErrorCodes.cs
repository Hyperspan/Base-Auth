namespace Base.Shared
{
    public static class BaseErrorCodes
    {

        public static IDictionary<string, string> ErrorMessages { get; set; } = new Dictionary<string, string>
        {
            { UnknownSystemException, "Some Unknown Error occurred" },
            { NotImplemented, "Method not implemented" },
            { ArgumentNull, "The argument passed was null." },
            { NullValue, "The value is null" },

            { EmailTaken, "This email address is already taken" },
            { PasswordNotStrong, "Password is not strong enough. Please try another password." },
            { IncorrectCredentials, "Credentials provided are not correct." },
            { UserNotFound, "The User was not found." },
            { EmailNotVerified, "The email address is not verified." },
            { MobileNotVerified, "Mobile No. is not verified." },
            { RoleExists, "The Role already exists in the database." },
            { IdentityError, "Something went wrong. While processing the request." },

            { NullConnectionString, "The connection string passed was null." }
        };


        #region 00 System
        public const string UnknownSystemException = "00SYS001";
        public const string NotImplemented = "00SYS002";
        public const string ArgumentNull = "00SYS003";
        public const string NullValue = "00SYS004";
        #endregion

        #region 01 Authentication
        public const string EmailTaken = "01AU001";
        public const string PasswordNotStrong = "01AU002";
        public const string IncorrectCredentials = "01AU003";
        public const string UserNotFound = "01AU004";
        public const string EmailNotVerified = "01AU005";
        public const string MobileNotVerified = "01AU006";
        public const string RoleExists = "01AU007";
        public const string IdentityError = "01AU008";
        #endregion

        #region 02 Database

        public const string NullConnectionString = "02DB001";
        
        #endregion

    }
}
