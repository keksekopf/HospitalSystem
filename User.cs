namespace HospitalSystem
{
    /// <summary>
    /// Base user class.
    /// </summary>
    public abstract class User
    {
        // Public properties viewable by any class, but can only be changed by this class.
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; private set; }
        public string Password { get; private set; }


        /// <summary>
        /// Initialises an instance of User class with the specified details.
        /// </summary>
        /// <param name="name">The user's name</param>
        /// <param name="age">The user's age</param>
        /// <param name="email">The user's email</param>
        /// <param name="mobile">The user's mobile</param>
        /// <param name="password">The user's password</param>
        public User(string name, int age, string mobile, string email, string password)
        {
            Name = name;
            Age = age;
            Mobile = mobile;
            Email = email;
            Password = password;
        }

        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="newPassword">The new password to set.</param>
        public void ChangePassword(string newPassword) // Protected so only derived classes can change their password field
        {
            Password = newPassword;
        }

        /// <summary>
        /// Displays the user's details.
        /// This method can be overriddenn by derived classes to customise the display.
        /// </summary>
        public virtual List<string> GetDetails()
        {
            return new List<string>
            {
                "Your details.",
                $"Name: {Name}",
                $"Age: {Age}",
                $"Mobile phone: {Mobile}",
                $"Email: {Email}"
            };
        }
    }
}
