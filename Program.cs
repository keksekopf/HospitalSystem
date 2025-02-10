namespace HospitalSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set the number of floors and rooms per floor in the hospital
            const int NUM_OF_FLOORS = 6;
            const int ROOMS_PER_FLOOR = 10;

            // Create an instance of the hospital containing 6 floors with 10 rooms each
            IHospital hospital = new Hospital(NUM_OF_FLOORS, ROOMS_PER_FLOOR);

            // Instantiate the account services with the hospital dependency
            IAccountServices accountServices = new AccountServices(hospital);

            // Instantiate the user interface
            IUserInterface ui = new CmdLineUI();

            // Instantiate the input validator with the user interface dependency
            IInputValidator inputValidator = new InputValidator(ui);

            // Create a menu instance with all the necessary dependencies
            MainMenu mainMenu = new MainMenu(accountServices, ui, inputValidator);

            // Run the menu
            mainMenu.Run();
        }
    }
}
