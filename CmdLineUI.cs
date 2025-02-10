using System.Globalization;
namespace HospitalSystem
{
    /// <summary>
    /// Contains methods for handling command line inputs and outputs.
    /// </summary>
    public class CmdLineUI : IUserInterface
    {
        public void DisplayMessage(string message = "")
        {
            Console.WriteLine(message);
        }

        public void DisplayError(string errorMessage)
        {
            Console.WriteLine($"#####\n#Error - {errorMessage}\n#####");
        }

        /// <summary>
        /// Displays an error message and asks the user to try again.
        /// </summary>
        /// <param name="msg"></param>
        public void DisplayErrorAgain(string msg)
        {
            Console.WriteLine($"#####\n#Error - {msg}, please try again.\n#####");
        }

        /// <summary>
        /// Returns a string from the console after displaying a message.
        /// </summary>
        /// <param name="msg">The message to display.</param>
        /// <returns>A string.</returns>
        public string GetString(string msg = "")
        {
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        /// <summary>
        /// Returns an integer from the console.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>An int.</returns>
        public int GetInt(string msg)
        {
            while (true)
            {
                Console.WriteLine(msg);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result;
                }
                else
                {
                    DisplayError("Supplied value is not an integer, please try again.");
                }
            }
        }

        /// <summary>
        /// Gets an option from the console.
        /// </summary>
        /// <param name="prompt">The prompt to display.</param>
        /// <param name="reprompt">Decides whether to continuously prompt the user for a valid choice.</param>
        /// <param name="options">The options the user can select.</param>
        /// <returns>An int representning the selected option.</returns>
        public int GetOption(string prompt, bool reprompt, params string[] options)
        {
            Console.WriteLine(prompt);
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                int selectedOption = GetInt($"Please enter a choice between 1 and {options.Length}.");
                if (selectedOption < 1 || selectedOption > options.Length)
                {
                    if (!reprompt)
                    {
                        return selectedOption - 1;
                    }
                    DisplayError("Supplied value is out of range, please try again.");
                    continue;
                }
                return selectedOption - 1;
            }
        }

        /// <summary>
        /// Returns a DateTime from the console.
        /// </summary>
        public DateTime GetDatetime()
        {
            while (true)
            {
                Console.WriteLine($"Please enter a date and time (e.g. 14:30 31/01/2024).");
                if (DateTime.TryParseExact(Console.ReadLine(), FormatConsts.DATETIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
                DisplayError("Supplied value is not a valid DateTime.");
            }
        }
    }
}
