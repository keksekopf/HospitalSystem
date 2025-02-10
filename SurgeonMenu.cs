namespace HospitalSystem
{
    /// <summary>
    /// Represents the menu for a surgeon.
    /// </summary>
    public class SurgeonMenu : Menu
    {
        public SurgeonMenu(IAccountServices accountServices, IUserInterface userInterface, IInputValidator inputValidator)
            : base(accountServices, userInterface, inputValidator) {}

        /// <summary>
        /// Displays the surgeon menu.
        /// </summary>
        /// <returns>True until the user logs out.</returns>
        protected override bool DisplayMenu()
        {
            Surgeon? currentUser = accountServices.CurrentUser as Surgeon;

            // Surgeon menu strings
            const string SURGEON_MENU = "Surgeon Menu.\nPlease choose from the menu below:";
            const string DISPLAY_DETAILS_STR = "Display my details";
            const string CHANGE_PASSWORD_STR = "Change password";
            const string VIEW_PATIENTS = "See your list of patients";
            const string VIEW_SCHEDULE = "See your schedule";
            const string PERFORM_SURGERY = "Perform surgery";
            const string LOGOUT_STR = "Log out";

            // Int for each option above
            const int DISPLAY_DETAILS_INT = 0, CHANGE_PASSWORD_INT = 1, VIEW_PATIENTS_INT = 2, 
                VIEW_SCHEDULE_INT = 3, PERFORM_SURGERY_INT = 4, LOGOUT_INT = 5;

            ui.DisplayMessage();

            // Display the menu
            int menuOption = ui.GetOption(SURGEON_MENU, true, DISPLAY_DETAILS_STR, CHANGE_PASSWORD_STR, VIEW_PATIENTS, VIEW_SCHEDULE, PERFORM_SURGERY, LOGOUT_STR);

            // Retrieve the list of patients
            List<Patient> patients = accountServices.GetPatients();

            // Make a selection based on user input
            string? message = menuOption switch
            {
                DISPLAY_DETAILS_INT => string.Join("\n", currentUser.GetDetails()), // Display details by joining the list of strings
                CHANGE_PASSWORD_INT => ChangePassword(currentUser),
                VIEW_PATIENTS_INT => string.Join("\n", currentUser.ViewPatients(patients)),
                VIEW_SCHEDULE_INT => string.Join("\n", currentUser.ViewSurgerySchedule(patients, FormatConsts.DATETIME_FORMAT)),
                PERFORM_SURGERY_INT => PerformSurgery(currentUser, patients),
                LOGOUT_INT => Logout(),
                _ => InvalidMenuOption(),
            };

            ui.DisplayMessage(message);

            // Return true to keep the menu running, unless the user chooses to log out
            return menuOption != LOGOUT_INT;
        }

        /// <summary>
        /// Handles the process of selecting a patient for performing surgery.
        /// </summary>
        /// <param name="surgeon">The surgeon performing the surgery.</param>
        /// <param name="patients">The patient receiving the surgery.</param>
        /// <returns>A message indicating the success of the operation.</returns>
        private string PerformSurgery(Surgeon surgeon, List<Patient> patients)
        {
            // Filter patients to find those with scheduled surgeries assigned to this surgeon
            List<Patient> patientsReadyForSurgery = patients
                .Where(patient => patient.ScheduledSurgery != null && patient.ScheduledSurgery.AssignedSurgeon == surgeon && !patient.ScheduledSurgery.SurgeryCompleted)
                .ToList();

            // Check if there are any patients ready for surgery
            if (!patientsReadyForSurgery.Any())
            {
                return "There are no patients ready for surgery.";
            }

            // Ask the user to select a patient
            int patientIndex = ui.GetOption("Please select your patient:", true, patientsReadyForSurgery.Select(patient => patient.Name).ToArray());

            // Retrieve the selected patient from the list
            Patient selectedPatient = patientsReadyForSurgery[patientIndex];
            
            // Perform the surgery
            return surgeon.PerformSurgery(selectedPatient);
        }
    }
}