
namespace HospitalSystem
{
    /// <summary>
    /// Abstraction of the user interface.
    /// </summary>
    public interface IUserInterface
    {
        void DisplayError(string errorMessage);
        void DisplayErrorAgain(string msg);
        void DisplayMessage(string message = "");
        DateTime GetDatetime();
        int GetInt(string msg);
        int GetOption(string prompt, bool reprompt, params string[] options);
        string GetString(string msg = "");
    }
}