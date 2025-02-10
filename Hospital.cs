namespace HospitalSystem
{
    /// <summary>
    /// Represents the hospital.
    /// </summary>
    public class Hospital : IHospital
    {
        // Initilaises a list of floors (and rooms)
        public List<Floor> Floors { get; private set; }
        // List for storing registered users
        private readonly List<User> _users;
        // List for storing surgery schedules
        private readonly List<SurgerySchedule> _surgerySchedules;

        public Hospital(int numOfFloors, int numOfRooms)
        {
            _surgerySchedules = new List<SurgerySchedule>();
            _users = new List<User>();
            Floors = new List<Floor>();
            for (int i = 1; i <= numOfFloors; i++)
            {
                Floors.Add(new Floor(i, numOfRooms));
            }
        }

        /// <summary>
        /// Adds a user to the hospital system.
        /// </summary>
        public void AddUser(User user)
        {
            _users.Add(user);
        }

        /// <summary>
        /// Returns a list of all users in the system.
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return _users;
        }

        /// <summary>
        /// Returns a list of all patients in the system.
        /// </summary>
        public List<Patient> GetPatients()
        {
            return _users.OfType<Patient>().ToList();
        }

        /// <summary>
        /// Returns a list of all surgeons in the system.
        /// </summary>
        public List<Surgeon> GetSurgeons()
        {
            return _users.OfType<Surgeon>().ToList();
        }

        /// <summary>
        /// Adds a scheduled surgery to the hospital system.
        /// </summary>
        /// <param name="surgerySchedule">The surgery to add.</param>
        public void AddScheduledSurgery(SurgerySchedule surgerySchedule)
        {
            _surgerySchedules.Add(surgerySchedule);
        }

        /// <summary>
        /// Returns a list of all scheduled surgeries.
        /// </summary>
        public List<SurgerySchedule> RetrieveScheduledSurgeries()
        {
            return _surgerySchedules;
        }
    }
}
