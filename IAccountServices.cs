namespace HospitalSystem
{
    /// <summary>
    /// Abstraction of AccountServices.
    /// </summary>
    public interface IAccountServices
    {
        User CurrentUser { get; }
        bool AreAnyFloorsAvailable();
        bool AreUsersRegistered();
        bool AuthenticateUser(string email, string password, out string message);
        bool IsEmailRegistered(string email);
        bool IsSpecifiedFloorAvailable(int floorNumber);
        bool IsStaffIdUnique(int staffId);
        List<Patient> GetPatients();
        List<Surgeon> GetSurgeons();
        Floor GetFloor(int floorNumber);
        string Logout();
        string RegisterFloorManager(string name, int age, string mobile, string email, string password, int staffId, Floor selectedFloor);
        string RegisterPatient(string name, int age, string mobile, string email, string password);
        string RegisterSurgeon(string name, int age, string mobile, string email, string password, int staffId, string speciality);
    }
}