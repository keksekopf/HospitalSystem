namespace HospitalSystem
{
    /// <summary>
    /// Represents the menu for a floor manager.
    /// </summary>
    public class FloorManagerMenu : Menu
    {
        public FloorManagerMenu(IAccountServices accountServices, IUserInterface userInterface, IInputValidator inputValidator)
            : base(accountServices, userInterface, inputValidator) {}
        
        /// <summary>
        /// Displays the floor mannager menu.
        /// </summary>
        /// <returns>True until the user logs out.</returns>
        protected override bool DisplayMenu()
        {
            FloorManager? currentUser = accountServices.CurrentUser as FloorManager;

            // Floor manager menu strings
            const string FLOORMNGR_MENU = "Floor Manager Menu.\nPlease choose from the menu below:";
            const string DISPLAY_DETAILS_STR = "Display my details";
            const string CHANGE_PASSWORD_STR = "Change password";
            const string ASSIGN_ROOM = "Assign room to patient";
            const string ASSIGN_SURGERY = "Assign surgery";
            const string UNASSIGN_ROOM = "Unassign room";
            const string LOGOUT_STR = "Log out";

            // Int for each option above
            const int DISPLAY_DETAILS_INT = 0, CHANGE_PASSWORD_INT = 1, ASSIGN_ROOM_INT = 2, 
                ASSIGN_SURGERY_INT = 3, UNASSIGN_ROOM_INT = 4, LOGOUT_INT = 5;

            ui.DisplayMessage();

            // Display the menu
            int menuOption = ui.GetOption(FLOORMNGR_MENU, true, DISPLAY_DETAILS_STR, CHANGE_PASSWORD_STR, ASSIGN_ROOM, ASSIGN_SURGERY, UNASSIGN_ROOM, LOGOUT_STR);

            // Make a selection based on user input
            string? message = menuOption switch
            {
                DISPLAY_DETAILS_INT => string.Join("\n", currentUser.GetDetails()), // Display details by joining the list of strings
                CHANGE_PASSWORD_INT => ChangePassword(currentUser),
                ASSIGN_ROOM_INT => AssignRoomToPatient(currentUser),
                ASSIGN_SURGERY_INT => AssignSurgeryToPatient(currentUser),
                UNASSIGN_ROOM_INT => UnassignRoomFromPatient(currentUser),
                LOGOUT_INT => Logout(),
                _ => InvalidMenuOption(),
            };

            if (message != null)
            {
                ui.DisplayMessage(message);
            }

            return menuOption != LOGOUT_INT;
        }

        /// <summary>
        /// Handles the process of selecting a patient and a room for assignment.
        /// </summary>
        /// <param name="floorManager">The floor manager assigning the room.</param>
        /// <returns>A message indicating the success of the operation.</returns>
        private string AssignRoomToPatient(FloorManager floorManager)
        {
            // Check if the floor is full
            if (floorManager.FloorManaged.IsFloorFull())
            {
                ui.DisplayError("All rooms on this floor are assigned.");
                return null;
            }

            // Check if there are any registered patients
            if(!accountServices.GetPatients().Any())
            {
                return "There are no registered patients.";
            }

            // Get a list of patients that are checked in and do not have an assigned room
            List<Patient> availablePatients = accountServices.GetPatients()
                .Where(patient => patient.CheckedIn && patient.AssignedRoom == null)
                .ToList();

            // Check if there are any checked in patients
            if (!availablePatients.Any())
            {
                return "There are no checked in patients.";
            }

            // Prompt the user to select a patient
            int patientIndex = ui.GetOption("Please select your patient:", true, availablePatients
            .Select(patient => patient.Name).ToArray());
            Patient selectedPatient = availablePatients[patientIndex];

            // Continuously prompt the user to select a room until a valid room is selected
            while (true)
            {
                int selectedRoom = inputValidator.ValidRoomNumber();

                string message = floorManager.AssignPatientToRoom(selectedPatient, selectedRoom);

                Room roomToAssign = floorManager.FloorManaged.GetRoom(selectedRoom);
                if (!roomToAssign.AssignPatient(selectedPatient))
                {
                    ui.DisplayErrorAgain("Room has been assigned to another patient");
                    continue;
                }
                return floorManager.AssignPatientToRoom(selectedPatient, selectedRoom);
            }
        }

        /// <summary>
        /// Handles the process of selecting a patient and a surgeon for surgery assignment.
        /// </summary>
        /// <param name="floorManager">The floor manager assigning the surgery.</param>
        /// <returns>A message indicating the result of the operation.</returns>
        private string AssignSurgeryToPatient(FloorManager floorManager)
        {
            // Check if there are any registered patients
            if (!accountServices.GetPatients().Any())
            {
                return "There are no registered patients.";
            }
            
            // Checks if there are any patients ready for surgery
            if (!accountServices.GetPatients().Any(patient => patient.CheckedIn && patient.AssignedRoom != null && patient.ScheduledSurgery == null))
            {
                return "There are no patients ready for surgery.";
            }

            // Get a list of patients that are checked in, have an assigned room, and do not have a scheduled surgery
            List<Patient> patientsReadyForSurgery = accountServices.GetPatients()
                .Where(patient => patient.CheckedIn && patient.AssignedRoom != null && patient.ScheduledSurgery == null)
                .ToList();

            // Prompt the user to select a patient
            int patientIndex = ui.GetOption("Please select your patient:", true, patientsReadyForSurgery.Select(p => p.Name).ToArray());
            Patient selectedPatient = patientsReadyForSurgery[patientIndex];

            // Get a list of available surgeons
            List<Surgeon> availableSurgeons = accountServices.GetSurgeons();
            // If there are no available surgeons, return a message
            if (!availableSurgeons.Any())
            {
                return "No surgeons available.";
            }

            // Prompt the user to select a surgeon
            int surgeonIndex = ui.GetOption("Please select your surgeon:", true, availableSurgeons.Select(s => s.Name).ToArray());
            Surgeon selectedSurgeon = availableSurgeons[surgeonIndex];

            // Prompt the user to select a surgery date
            DateTime surgeryDate = ui.GetDatetime();

            // Call the FloorManager's method to schedule the surgery
            return floorManager.ScheduleSurgery(selectedPatient, selectedSurgeon, surgeryDate, FormatConsts.DATETIME_FORMAT);
        }

        /// <summary>
        /// Handles the process of unassigning a patient from their room.
        /// </summary>
        /// <param name="floorManager">The floor manager unassigning the room.</param>
        /// <returns>A message indicating the success of the operation.</returns>
        private string UnassignRoomFromPatient(FloorManager floorManager)
        {
            // Check if there are any patients with assigned rooms or have completed surgery
            // Patients with assigned surgeries must have rooms, so no need to check for both in the second statement
            if (!accountServices.GetPatients().Any(patient => patient.AssignedRoom != null) 
                || !accountServices.GetPatients().Any(patient => patient.ScheduledSurgery != null && patient.ScheduledSurgery.SurgeryCompleted))
            {
                return "There are no patients ready to have their rooms unassigned.";
            }

            // Get a list of patients with assigned rooms
            List<Patient> patientsWithRooms = accountServices.GetPatients()
                .Where(patient => patient.AssignedRoom != null)
                .ToList();

            // Prompt the user to select a patient
            int patientIndex = ui.GetOption("Please select your patient:", true, patientsWithRooms.Select(p => p.Name).ToArray());
            Patient selectedPatient = patientsWithRooms[patientIndex];

            // Call the FloorManager's method to unassign the patient from the room
            return floorManager.UnassignPatientFromRoom(selectedPatient);
        }
    }
}