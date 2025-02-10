namespace HospitalSystem
{
    /// <summary>
    /// Represents the menu for a patient.
    /// </summary>
    public class PatientMenu : Menu
    {
        public PatientMenu(IAccountServices accountServices, IUserInterface userInterface, IInputValidator inputValidator)
            : base(accountServices, userInterface, inputValidator) {}

        /// <summary>
        /// Displays the patient menu.
        /// </summary>
        /// <returns>True until the user logs out.</returns>
        protected override bool DisplayMenu()
        {
            // Get the current user as a Patient
            Patient? currentUser = accountServices.CurrentUser as Patient;

            // Patient menu strings
            const string PATIENT_MENU_STR = "Patient Menu.\nPlease choose from the menu below:";
            const string DISPLAY_DETAILS_STR = "Display my details";
            const string CHANGE_PASSWORD_STR = "Change password";
            const string VIEW_ROOM_STR = "See room";
            const string VIEW_SURGEON_STR = "See surgeon";
            const string VIEW_SURGERY_STR = "See surgery date and time";
            const string LOGOUT_STR = "Log out";

            // Int for each option above
            const int DISPLAY_DETAILS_INT = 0, CHANGE_PASSWORD_INT = 1, CHECK_IN_INT = 2, VIEW_ROOM_INT = 3, 
                VIEW_SURGEON_INT = 4, VIEW_SURGERY_INT = 5, LOGOUT_INT = 6;

            ui.DisplayMessage();

            // Dynamic string that displays check in/check out option based on patient's status
            string checkInOutStr = currentUser?.CheckedIn == true ? "Check out" : "Check in";

            // Display the menu
            int menuOption = ui.GetOption(PATIENT_MENU_STR, true, 
                DISPLAY_DETAILS_STR, CHANGE_PASSWORD_STR, checkInOutStr, VIEW_ROOM_STR, VIEW_SURGEON_STR, VIEW_SURGERY_STR, LOGOUT_STR);

            // Make a selection based on user input
            string? message = menuOption switch
            {
                DISPLAY_DETAILS_INT => string.Join("\n", currentUser.GetDetails()), // Display details by joining the list of strings
                CHANGE_PASSWORD_INT => ChangePassword(currentUser),
                CHECK_IN_INT => currentUser.CheckInOrOut(), // Decides which message to return depending on the state of the patient's checkedIn boolean
                VIEW_ROOM_INT => currentUser.GetRoomDetails(),
                VIEW_SURGEON_INT => currentUser.GetSurgeonDetails(),
                VIEW_SURGERY_INT => currentUser.GetSurgeryDetails(FormatConsts.DATETIME_FORMAT),
                LOGOUT_INT => Logout(),
                _ => InvalidMenuOption(),
            };

            ui.DisplayMessage(message);

            // Return true to keep the menu running, unless the user chooses to log out
            return menuOption != LOGOUT_INT;
        }
    }
}