namespace HospitalSystem
{
    /// <summary>
    /// Represents a patient in the hospital and inherits from the User class.
    /// </summary>
    public class Patient : User
    {
        // Indicates whether the patiet is currently checked in to the hospital
        public bool CheckedIn { get; set; } = false;

        // Represents the hospital room assigned to the patient
        public Room? AssignedRoom { get; set; }

        // Holds the details for the patient's surgery
        public SurgerySchedule? ScheduledSurgery { get; set; }

        // Constructor to initialise a new patient
        public Patient(string name, int age, string mobile, string email, string password)
            : base(name, age, mobile, email, password) { }

        /// <summary>
        /// Checks a patient in or out. Displays a message if the patient cannot do so.
        /// </summary>
        /// <returns>A string of the relevant message.</returns>
        public string CheckInOrOut()
        {
            // Check if the patient cannot check out due to pending surgery or cannot check in due to completed surgery
            if (CannotCheckOut() || CannotCheckIn())
            {
                return CheckedIn? "You are unable to check out at this time." : "You are unable to check in at this time.";
            }

            // Change the check-in status and return the appropriate message
            CheckedIn = !CheckedIn;
            return CheckedIn ? $"Patient {Name} has been checked in." : $"Patient {Name} has been checked out.";
        }

        /// <summary>
        /// Returns the details of the room assigned to the patient.
        /// Returns an error message if no room is assigned.
        /// </summary>
        /// <returns>A string with the room details or an error message.</returns>
        public string GetRoomDetails()
        {
            return AssignedRoom == null
                ? "You do not have an assigned room."
                : $"Your room is number {AssignedRoom.RoomNumber} on floor {AssignedRoom.FloorNumber}.";
        }

        /// <summary>
        /// Returns the name of the surgeon assigned to the patient. 
        /// Returns an error message if no surgeon is assigned.
        /// </summary>
        /// <returns>A string with the surgeon's name or an error message.</returns>
        public string GetSurgeonDetails()
        {
            return ScheduledSurgery == null 
                ? "You do not have an assigned surgeon."
                : $"Your surgeon is {ScheduledSurgery.AssignedSurgeon.Name}.";
        }

        /// <summary>
        /// Returns the details of the patient's scheduled surgery.
        /// Returns an error message if no surgery is scheduled.
        /// </summary>
        /// <param name="dateTimeFormat">The datetime format to adhere to.</param>
        /// <returns>A string with the formatted DateTime or an error message.</returns>
        public string GetSurgeryDetails(string dateTimeFormat)
        {
            return ScheduledSurgery == null || ScheduledSurgery.SurgeryCompleted
                ? "You do not have assigned surgery."
                : $"Your surgery time is {ScheduledSurgery.SurgeryDateTime.ToString(dateTimeFormat)}.";
        }

        // Private helper methods

        /// <summary>
        /// Checks whether the patient can check out.
        /// Patients cannot check out if they have not completed their surgery after checking in.
        /// </summary>
        /// <returns>True if they cannnot check out, false otherwise.</returns>                          
        private bool CannotCheckOut() => CheckedIn && (ScheduledSurgery == null || !ScheduledSurgery.SurgeryCompleted);

        /// <summary>
        /// Checks whether the patient can check in.
        /// Patients cannot check in if they have already checked in or have completed surgery.
        /// </summary>
        /// <returns>True if they cannnot check in, false otherwise.</returns>
        private bool CannotCheckIn() => !CheckedIn && ScheduledSurgery != null && ScheduledSurgery.SurgeryCompleted;
    }
}
