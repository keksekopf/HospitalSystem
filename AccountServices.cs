namespace HospitalSystem
{
    /// <summary>
    /// Account related services for the hospital system.
    /// </summary>
    public class AccountServices : IAccountServices
    {
        // Private field for the hospital database dependency
        private readonly IHospital _hospital;

        // The currently logged in user
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Constructor to initialise the account services with the hospital database.
        /// </summary>
        /// <param name="hospital">The hospital for dependecy injection.</param>
        public AccountServices(IHospital hospital)
        {
            _hospital = hospital;
        }

        /// <summary>
        /// Authenticates a user by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AuthenticateUser(string email, string password, out string message)
        {
            // Find the user with the specified email
            var user = _hospital.GetAllUsers().FirstOrDefault(user => user.Email == email);
            // If the user's password does not match the inner password, return false
            if (user.Password != password)
            {
                message = "Wrong Password.";
                return false;
            }

            // Set the current user and return true
            CurrentUser = user;
            message = $"Hello {user.Name} welcome back.";
            return true;
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public string Logout()
        {
            string user = userType();
            // Need to store the user's name before setting the CurrentUser to null
            string message = $"{user} {CurrentUser.Name} has logged out.";
            CurrentUser = null;
            return message;
        }

        /// <summary>
        /// Registers a new patient.
        /// </summary>
        public string RegisterPatient(string name, int age, string mobile, string email, string password)
        {
            Patient patient = new Patient(name, age, mobile, email, password);
            _hospital.AddUser(patient);
            return $"{patient.Name} is registered as a patient.";
        }

        /// <summary>
        /// Registers a new floor manager.
        /// </summary>
        public string RegisterFloorManager(string name, int age, string mobile, string email, string password, int staffId, Floor selectedFloor)
        {
            // Assume all validation is done before calling this method
            FloorManager floorManager = new FloorManager(name, age, mobile, email, password, staffId, selectedFloor, _hospital);
            selectedFloor.AssignManager(floorManager);
            _hospital.AddUser(floorManager);
            return $"{floorManager.Name} is registered as a floor manager.";
        }

        /// <summary>
        /// Registers a new surgeon.
        /// </summary>
        public string RegisterSurgeon(string name, int age, string mobile, string email, string password, int staffId, string speciality)
        {
            // Assume all validation is done before calling this method
            Surgeon surgeon = new Surgeon(name, age, mobile, email, password, staffId, speciality, _hospital);
            _hospital.AddUser(surgeon);
            return $"{surgeon.Name} is registered as a surgeon.";
        }

        /// <summary>
        /// Gets all users in the hospital.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public List<User> GetAllUsers()
        {
            return _hospital.GetAllUsers();
        }

        /// <summary>
        /// Gets all patients in the hospital.
        /// </summary>
        /// <returns>A list of all patients.</returns>
        public List<Patient> GetPatients()
        {
            return _hospital.GetAllUsers().OfType<Patient>().ToList();
        }

        /// <summary>
        /// Gets all surgeons in the hospital.
        /// </summary>
        /// <returns>A list of all surgeons.</returns>
        public List<Surgeon> GetSurgeons()
        {
            return _hospital.GetAllUsers().OfType<Surgeon>().ToList();
        }

        /// <summary>
        /// Gets a floor by its number.
        /// </summary>
        /// <param name="floorNumber">The floor to retrieve.</param>
        /// <returns>The specified floor.</returns>
        public Floor GetFloor(int floorNumber)
        {
            return _hospital.Floors.FirstOrDefault(f => f.FloorNumber == floorNumber);
        }

        /// <summary>
        /// Checks if there are any users registered in the hospital.
        /// </summary>
        /// <returns>True if there are registered users, false otherwise.</returns>
        public bool AreUsersRegistered() => _hospital.GetAllUsers().Any();

        /// <summary>
        /// Checks if the user's email is already registered.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is registered, false otherwise</returns>
        public bool IsEmailRegistered(string email) => _hospital.GetAllUsers().Any(user => user.Email == email);

        /// <summary>
        /// Checks if the user's staff ID is already registered.
        /// </summary>
        /// <param name="staffId">The staff ID to check</param>
        /// <returns>True if the staff ID is registered, false otherwise</returns>
        public bool IsStaffIdUnique(int staffId) => !_hospital.GetAllUsers().OfType<Staff>().Any(staff => staff.StaffId == staffId);

        /// <summary>
        /// Checks if there are any available floors for assignment.
        /// </summary>
        /// <returns>True if any floors do not have an assigned manager, false otherwise</returns>
        public bool AreAnyFloorsAvailable() => _hospital.Floors.Any(f => f.AssignedManager == null);

        /// <summary>
        /// Checks if the specified floor is available for assignment.
        /// </summary>
        /// <param name="floorNumber">The floor number to check.</param>
        /// <returns>True if floor is available for assignment, false otherwise.</returns>
        public bool IsSpecifiedFloorAvailable(int floorNumber) => _hospital.Floors.FirstOrDefault(f => f.FloorNumber == floorNumber)?.AssignedManager == null;

        /// <summary>
        /// Returns the type of the current user.
        /// </summary>
        private string userType()
        {
            string userType = CurrentUser.GetType().Name switch
            {
                "Patient" => "Patient",
                "FloorManager" => "Floor manager",
                "Surgeon" => "Surgeon",
                _ => "???", // Shouldn't happen
            };
            return userType;
        }
    }
}
