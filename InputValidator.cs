using System.Text.RegularExpressions;

namespace HospitalSystem
{
    /// <summary>
    /// Checks for valid user input.
    /// </summary>
    public class InputValidator : IInputValidator
    {
        // The interface to interact with the user
        private readonly IUserInterface ui;

        public InputValidator(IUserInterface userInterface)
        {
            ui = userInterface;
        }

        /// <summary>
        /// Continuously prompts the user until a valid name is selected.
        /// </summary>
        /// <returns>A valid name.</returns>
        public string ValidName()
        {
            string name;
            while (true)
            {
                ui.DisplayMessage("Please enter in your name:");
                name = ui.GetString();

                if (!Regex.IsMatch(name.Trim(), FormatConsts.VALID_NAME_REGEX))
                {
                    ui.DisplayErrorAgain("Supplied name is invalid");
                    continue;
                }
                return name;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid age is selected.
        /// </summary>
        /// <param name="ageRange">The valid age range.</param>
        /// <returns>A valid age.</returns>
        public int ValidAge((int minAge, int maxAge) ageRange)
        {
            int age;
            while (true)
            {
                age = ui.GetInt("Please enter in your age:");
                if (age < ageRange.minAge || age > ageRange.maxAge)
                {
                    ui.DisplayErrorAgain("Supplied age is invalid");
                    continue;
                }
                return age;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid mobile is selected.
        /// </summary>
        /// <returns>A valid mobile.</returns>
        public string ValidMobile()
        {
            string mobile;
            while (true)
            {
                mobile = ui.GetString("Please enter in your mobile number:");
                if (!Regex.IsMatch(mobile, FormatConsts.VALID_MOBILE_REGEX)) // Mobile must begin with 0 and contain 10 digits
                {
                    ui.DisplayErrorAgain("Supplied mobile number is invalid");
                    continue;
                }
                return mobile;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid email is selected.
        /// </summary>
        /// <returns>A valid email.</returns>
        public string ValidEmail()
        {
            string email;
            while (true)
            {
                email = ui.GetString("Please enter in your email:");
                if (!Regex.IsMatch(email, FormatConsts.VALID_EMAIL_REGEX))
                {
                    ui.DisplayErrorAgain("Supplied email is invalid");
                    continue;
                }
                return email;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid password is selected.
        /// </summary>
        /// <returns>A valid password.</returns>
        public string ValidPassword()
        {
            return GetValidPassword("Please enter in your password:");
        }

        /// <summary>
        /// Continuously prompts the user until a valid new password is selected.
        /// </summary>
        /// <returns>A valid password.</returns>
        public string ValidNewPassword()
        {
            return GetValidPassword("Enter new password:");
        }

        /// <summary>
        /// Cotinuously prompts the user until a valid staff ID is selected.
        /// </summary>
        /// <returns>A valid staff ID.</returns>
        public int ValidStaffId()
        {
            int staffId;
            while (true)
            {
                staffId = ui.GetInt("Please enter in your staff ID:");
                if (staffId < 100 || staffId > 999)
                {
                    ui.DisplayErrorAgain("Supplied staff identification number is invalid");
                    continue;
                }
                return staffId;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid floor number is selected.
        /// </summary>
        /// <param name="maxFloors">The maximum number of floors in the hospital.</param>
        /// <returns>A valid floor number.</returns>
        public int ValidFloor(int maxFloors = 6)
        {
            int floor;
            while (true)
            {
                floor = ui.GetInt("Please enter in your floor number:");
                if (floor < 1 || floor > 6)
                {
                    ui.DisplayErrorAgain("Supplied floor is invalid");
                    continue;
                }
                return floor;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid room number is selected.
        /// </summary>
        /// <returns>A valid room number.</returns>
        public int ValidRoomNumber()
        {
            int roomNumber;
            while (true)
            {
                roomNumber = ui.GetInt("Please enter your room (1-10) :");
                if (roomNumber < 1 || roomNumber > 10)
                {
                    ui.DisplayErrorAgain("Supplied value is out of range");
                    continue;
                }
                return roomNumber;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid date is selected.
        /// </summary>
        /// <returns>A valid DateTime.</returns>
        public string ValidDate()
        {
            string date = null;
            while (true)
            {
                if (!DateTime.TryParseExact(date, FormatConsts.DATETIME_FORMAT,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out _))
                {
                    ui.DisplayError("Invalid date");
                    continue;
                }
                return date;
            }
        }

        /// <summary>
        /// Continuously prompts the user until a valid speciality is selected.
        /// </summary>
        /// <returns>A valid speciality.</returns>
        public string ValidSpeciality()
        {
            const string SPECIALTY_PROMPT = "Please choose your speciality:";
            // The specialities available to choose from
            var specialties = new Dictionary<int, string>
            {
                { 0, "General Surgeon" },
                { 1, "Orthopaedic Surgeon" },
                { 2, "Cardiothoracic Surgeon" },
                { 3, "Neurosurgeon" }
            };

            // Keep prompting the user until they select a valid speciality
            while (true)
            {
                int selection = ui.GetOption(SPECIALTY_PROMPT, false, specialties.Values.ToArray());
                if (specialties.TryGetValue(selection, out string speciality))
                {
                    return speciality;
                }
                ui.DisplayErrorAgain("Non-valid speciality type");
            }
        }

        /// <summary>
        /// Helper method to get a valid password.
        /// </summary>
        /// <param name="prompt">The message to prompt the user with.</param>
        private string GetValidPassword(string prompt)
        {
            string password;
            while (true)
            {
                password = ui.GetString(prompt);
                if (!Regex.IsMatch(password, FormatConsts.VALID_PASSWORD_REGEX))
                {
                    ui.DisplayErrorAgain("Supplied password is invalid");
                    continue;
                }
                return password;
            }
        }

    }
}