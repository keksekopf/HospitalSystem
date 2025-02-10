
namespace HospitalSystem
{
    /// <summary>
    /// Abstraction of Hospital.
    /// </summary>
    public interface IHospital
    {
        List<Floor> Floors { get; }
        void AddScheduledSurgery(SurgerySchedule surgerySchedule);
        void AddUser(User user);
        List<User> GetAllUsers();
        List<Patient> GetPatients();
        List<Surgeon> GetSurgeons();
        List<SurgerySchedule> RetrieveScheduledSurgeries();
    }
}