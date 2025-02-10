namespace HospitalSystem
{
    /// <summary>
    /// Contains functionalities common to all staff members.
    /// </summary>
    public abstract class Staff : User
    {
        // Fields common to all staff members
        public int StaffId { get; private set; }

        /// <summary>
        /// Allows staff users to register with their staff ID.
        /// </summary>
        /// <param name="staffId">The user's staff ID.</param>
        protected Staff(string name, int age, string email, string mobile, string password, int staffId)
            :base(name, age, email, mobile, password)
        {
            StaffId = staffId;
        }

        /// <summary>
        /// Displays the user's details, including their staff ID.
        /// </summary>
        public override List<string> GetDetails()
        {
            // Get the base details from the user class
            List<string> details = base.GetDetails();

            // Add additional details specific to staff
            details.Add($"Staff ID: {StaffId}");
            
            return details;
        }
    }
}
