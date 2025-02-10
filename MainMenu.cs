namespace HospitalSystem
{
    /// <summary>
    /// The main menu of the program.
    /// </summary>
    public class MainMenu : Menu
    {
        private readonly PatientMenu patientMenu;
        private readonly FloorManagerMenu floorManagerMenu;
        private readonly SurgeonMenu surgeonMenu;

        /// <summary>
        /// Creates an instance of the menu class
        /// </summary>
        /// <param name="hospital">The hospital which the menu is used for</param>
        public MainMenu(IAccountServices accountServices, IUserInterface ui, IInputValidator inputValidator)
            : base(accountServices, ui, inputValidator) 
        {
            patientMenu = new PatientMenu(accountServices, ui, inputValidator);
            floorManagerMenu = new FloorManagerMenu(accountServices, ui, inputValidator);
            surgeonMenu = new SurgeonMenu(accountServices, ui, inputValidator);
        }

        /// <summary>
        /// Display the header and runs the menu.
        /// </summary>
        public override void Run()
        {
            DisplayHeader();
            while (DisplayMenu())
            {
                // Continue running if display menu is true
            }
        }

        /// <summary>
        /// Displays the main heading.
        /// </summary>
        private void DisplayHeader()
        {
            ui.DisplayMessage("=================================");
            ui.DisplayMessage("Welcome to Gardens Point Hospital");
            ui.DisplayMessage("=================================");
        }

        protected override bool DisplayMenu()
        {            
            ui.DisplayMessage();

            // The main menu strings
            const string MAINMENU_STR = "Please choose from the menu below:"; // Main menu prompt
            const string LOGIN_STR = "Login as a registered user"; // MainMenu option 1
            const string REGISTER_STR = "Register as a new user"; // MainMenu option 2
            const string EXIT_STR = "Exit"; // MainMenu option 3

            // Int for each option above
            const int LOGIN_INT = 0, REGISTER_INT = 1, EXIT_INT = 2;

            // Display the menu and retrieve the user's choice
            int menuSelection = ui.GetOption(MAINMENU_STR, false, LOGIN_STR, REGISTER_STR, EXIT_STR);

            Action selectedMenu = menuSelection switch
            {
                LOGIN_INT => Login,
                REGISTER_INT => RegisterUser,
                EXIT_INT => () => Exit(),
                _ => () => InvalidMenuOption(),
            };

            selectedMenu.Invoke();

            return menuSelection != EXIT_INT;
        }

        /// <summary>
        /// Allows a user to authenticate themselves and log into the system.
        /// </summary>
        private void Login()
        {
            // Display the header
            ui.DisplayMessage();
            ui.DisplayMessage("Login Menu.");

            // Check if anyone is registered
            if (!accountServices.AreUsersRegistered())
            {
                ui.DisplayError("There are no people registered.");
                return;
            }

            // Ask user for their email and check if it exists in the system
            string email = ui.GetString("Please enter in your email:");
            if (!accountServices.IsEmailRegistered(email))
            {
                ui.DisplayError("Email is not registered.");
                return;
            }
            else
            {
                // Prompt for password if email is valid
                string password = ui.GetString("Please enter in your password:");
                // Check if password is correct
                if (!accountServices.AuthenticateUser(email, password, out string loginSuccess))
                {
                    // If incorrect, display error message and exit the method
                    ui.DisplayError(loginSuccess);
                    return; // Exit the method
                }
                else
                {
                    // If correct, display success message
                    ui.DisplayMessage(loginSuccess);
                }
            }

            // Run the appropriate menu based on the user's type
            Action userMenu = accountServices.CurrentUser switch
            {
                Patient patient => patientMenu.Run,
                FloorManager floorManager => floorManagerMenu.Run,
                Surgeon surgeon => surgeonMenu.Run,
                _ => () => ui.DisplayError("Unexpected user type.") // shouldn't happen
            };

            // Invoke the user's menu
            userMenu.Invoke();
        }

        /// <summary>
        /// Prompts the user for which type of account they would like to register.
        /// </summary>
        private void RegisterUser()
        {
            ui.DisplayMessage();
            // The registration menu strings
            const string REGISTER_MENU_STR = "Register as which type of user:"; // Registration menu prompt
            const string REGISTER_PATIENT_STR = "Patient"; // MainMenu option 1
            const string REGISTER_STAFF_STR = "Staff"; // MainMenu option 2
            const string RETURN_STR = "Return to the first menu"; // MainMenu option 3

            // Int for each option above
            const int REGISTER_PATIENT_INT = 0, REGISTER_STAFF_INT = 1, RETURN_INT = 2;

            // Display the menu
            int menuSelection = ui.GetOption(REGISTER_MENU_STR, false, REGISTER_PATIENT_STR, REGISTER_STAFF_STR, RETURN_STR);

            // Make selection based on user input
            Action selectedMenu = menuSelection switch
            {
                REGISTER_PATIENT_INT => RegisterPatient,
                REGISTER_STAFF_INT => RegisterStaff,
                RETURN_INT => Return,
                _ => () => InvalidMenuOption(),
            };

            // Invoke the selected action
            selectedMenu.Invoke();
        }

        /// <summary>
        /// Prompts the user for which type of staff account to register as.
        /// </summary>
        private void RegisterStaff()
        {
            ui.DisplayMessage();
            // Staff registration prompts
            const string STAFF_CHOICE = "Register as which type of staff:";
            const string FLOORMNGR = "Floor manager";
            const string SURGEON_STR = "Surgeon";
            const string RETURN_STR = "Return to the first menu";

            // Int for each option above
            const int FLOORMNGR_INT = 0, SURGEON_INT = 1, RETURN_INT = 2;
            
            // Display the menu
            int menuSelection = ui.GetOption(STAFF_CHOICE, false, FLOORMNGR, SURGEON_STR, RETURN_STR);

            // Make selection based on user input
            Action selectedMenu = menuSelection switch
            {
                FLOORMNGR_INT => RegisterFloorManager,
                SURGEON_INT => RegisterSurgeon,
                RETURN_INT => Return,
                _ => () => InvalidMenuOption(),
            };
            selectedMenu.Invoke();
        }


        /// <summary>
        /// Registers a patient account.
        /// </summary>
        private void RegisterPatient()
        {
            ui.DisplayMessage("Registering as a patient.");

            string name = inputValidator.ValidName();
            int age = inputValidator.ValidAge((0, 100));
            string mobile = inputValidator.ValidMobile();
            string email = GetUniqueEmail();
            string password = inputValidator.ValidPassword();

            string result = accountServices.RegisterPatient(name, age, mobile, email, password);
            ui.DisplayMessage(result);
        }

        /// <summary>
        /// Registers a floor manager account.
        /// </summary>
        private void RegisterFloorManager()
        {
            // Check if there are any floors available
            if (!accountServices.AreAnyFloorsAvailable())
            {
                ui.DisplayError("All floors are assigned.");
                return;
            }

            ui.DisplayMessage("Registering as a floor manager.");

            string name = inputValidator.ValidName();
            int age = inputValidator.ValidAge((21, 70));
            string mobile = inputValidator.ValidMobile();
            string email = GetUniqueEmail();
            string password = inputValidator.ValidPassword();
            int staffId = GetUniqueStaffId();
            Floor selectedFloor = GetAvailableFloor();

            string result = accountServices.RegisterFloorManager(name, age, mobile, email, password, staffId, selectedFloor);
            ui.DisplayMessage(result);
        }

        /// <summary>
        /// Registers a surgeon account.
        /// </summary>
        private void RegisterSurgeon()
        {
            ui.DisplayMessage("Registering as a surgeon.");

            string name = inputValidator.ValidName();
            int age = inputValidator.ValidAge((30, 75));
            string mobile = inputValidator.ValidMobile();
            string email = GetUniqueEmail();
            string password = inputValidator.ValidPassword();
            int staffId = GetUniqueStaffId();
            string speciality = inputValidator.ValidSpeciality();

            string result = accountServices.RegisterSurgeon(name, age, mobile, email, password, staffId, speciality);
            ui.DisplayMessage(result);
        }

        /// <summary>
        /// Exits the program.
        /// </summary>
        /// <returns></returns>
        private bool Exit()
        {
            ui.DisplayMessage("Goodbye. Please stay safe.");
            return false;
        }

        /// <summary>
        /// Returns to the main menu.
        /// </summary>
        private void Return()
        {
            // Do nothing, just return to the main menu
            return;
        }

        // Helper methods

        /// <summary>
        /// Prompts the user to enter a unique email.
        /// </summary>
        /// <returns>A unique email.</returns>
        private string GetUniqueEmail()
        {
            string email;
            do
            {
                email = inputValidator.ValidEmail();
                if (accountServices.IsEmailRegistered(email))
                {
                    ui.DisplayErrorAgain("Email is already registered");
                }
            } while (accountServices.IsEmailRegistered(email));

            return email;
        }

        /// <summary>
        /// Prompts the user to enter a unique staff ID.
        /// </summary>
        /// <returns>A unique staff ID.</returns>
        private int GetUniqueStaffId()
        {
            int staffId;
            do
            {
                staffId = inputValidator.ValidStaffId();
                if (!accountServices.IsStaffIdUnique(staffId))
                {
                    ui.DisplayErrorAgain("Staff ID is already registered");
                }
            } while (!accountServices.IsStaffIdUnique(staffId));

            return staffId;
        }

        /// <summary>
        /// Prompts the user to select an available floor.
        /// </summary>
        /// <returns>An available floor.</returns>
        private Floor GetAvailableFloor()
        {
            int floorNumber;
            do
            {
                floorNumber = inputValidator.ValidFloor();
                if (!accountServices.IsSpecifiedFloorAvailable(floorNumber))
                {
                    ui.DisplayErrorAgain("Floor has been assigned to another floor manager");
                }
            } while (!accountServices.IsSpecifiedFloorAvailable(floorNumber));

            return accountServices.GetFloor(floorNumber);
        }
    }
}
