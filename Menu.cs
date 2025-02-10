namespace HospitalSystem
{
    /// <summary>
    /// Abstract base class representing a menu in the hospital system.
    /// </summary>
    public abstract class Menu
    {
        // Protected fields for dependencies, accessible by derived classes
        protected readonly IUserInterface ui;
        protected readonly IAccountServices accountServices;
        protected readonly IInputValidator inputValidator;

        // Constructor to initialise the menu with the necessary dependencies
        protected Menu(IAccountServices accountServices, IUserInterface userInterface, IInputValidator inputValidator)
        {
            // Initialise the following services with shared instances
            this.accountServices = accountServices;
            ui = userInterface;
            this.inputValidator = inputValidator;
        }

        /// <summary>
        /// Runs the menu.
        /// Continuously displays the menu until the user chooses to exit.
        /// </summary>
        public virtual void Run()
        {
            while (DisplayMenu()) {}
        }

        /// <summary>
        /// Abstract method to display the menu.
        /// Must be implemented by derived classes.
        /// </summary>
        /// <returns>True to keep the menu running, false to exit.</returns>
        protected abstract bool DisplayMenu();

        /// <summary>
        /// Changes the password for a given user.
        /// </summary>
        /// <param name="user">The user whose password is being changed.</param>
        /// <returns>A confirmation message indicating the password has been changed.</returns>
        protected string ChangePassword(User user)
        {
            // Change the user's password by asking for a valid new password
            user.ChangePassword(inputValidator.ValidNewPassword());
            return "Password has been changed.";
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A message indicating the user has logged out.</returns>
        protected string Logout()
        {
            return accountServices.Logout();
        }

        /// <summary>
        /// Handles invalid menu options.
        /// </summary>
        /// <returns>Null, aftering displaying an error message for an invalid option.</returns>
        protected string InvalidMenuOption()
        {
            ui.DisplayErrorAgain("Invalid Menu Option");
            return string.Empty;
        }
    }
}