namespace HospitalSystem
{
    /// <summary>
    /// A set of constants to be used within the program.
    /// </summary>
    public static class FormatConsts
    {
        // The format for the date and time strings.
        public const string DATETIME_FORMAT = "HH:mm dd/MM/yyyy";

        // The regex format for valid input.
        public const string VALID_NAME_REGEX = @"[^\s-][A-Za-z\s]+$";
        public const string VALID_MOBILE_REGEX = @"^0\d{9}$";
        public const string VALID_EMAIL_REGEX = @"^[^@\s]+@+[^@\s]+$";
        public const string VALID_PASSWORD_REGEX = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";
    }
}
