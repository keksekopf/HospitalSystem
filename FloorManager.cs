namespace HospitalSystem
{
    /// <summary>
    /// Represents a floor manager in the hospital.
    /// </summary>
    public class FloorManager : Staff
    {
        // Represents the floor managed by this floor manager
        public Floor FloorManaged { get; private set; }

        // Reference to the hospital interface for accessing the shared database
        private readonly IHospital _hospital;

        /// <summary>
        /// Creates a floor manager instance and connects it to a shared instance of the database using dependency injection.
        /// </summary>
        public FloorManager(string name, int age, string mobile, string email, string password, int staffId, Floor floorManaged, IHospital hospital)
            : base(name, age, mobile, email, password, staffId)
        {
            _hospital = hospital;
            FloorManaged = floorManaged;
            FloorManaged.AssignManager(this);
        }

        /// <summary>
        /// Assigns a patient to a specified room on the managed floor.
        /// </summary>
        /// <param name="selectedPatient">The patient to assign to a room.</param>
        /// <param name="selectedRoom">The room number to assign the patient to.</param>
        /// <returns>A message indicating the result of the method.</returns>
        public string AssignPatientToRoom(Patient selectedPatient, int selectedRoom)
        {
            // Attempt to assign the patient to the specific room
            Room roomToAssign = FloorManaged.GetRoom(selectedRoom);
            // Update the patient's assigned room and return a string indicating success
            selectedPatient.AssignedRoom = roomToAssign;
            return $"Patient {selectedPatient.Name} has been assigned to room number {roomToAssign.RoomNumber} on floor {FloorManaged.FloorNumber}.";
        }

        /// <summary>
        /// Unassigns a patient from their current room.
        /// </summary>
        /// <param name="selectedPatient">The patient to unassign from a room.</param>
        /// <returns>A message indicating the result of the method.</returns>
        public string UnassignPatientFromRoom(Patient selectedPatient)
        {
            // Check if the patient has a scheduled surgery
            if (selectedPatient.ScheduledSurgery != null && !selectedPatient.ScheduledSurgery.SurgeryCompleted)
            {
                return "Patient has a scheduled surgery.";
            }
            
            // Need to initialise a string here because the AssignedRoom values will be null after the last operation
            string message = $"Room number {selectedPatient.AssignedRoom.RoomNumber} on floor {selectedPatient.AssignedRoom.FloorNumber} has been unassigned.";

            // Unassign the patient from their room
            Room roomToUnassign = FloorManaged.GetRoom(selectedPatient.AssignedRoom.RoomNumber);
            roomToUnassign.RemovePatient(selectedPatient);
            return message;
        }

        /// <summary>
        /// Schedules a surgery for a patient with a specified surgeon and date.
        /// </summary>
        /// <param name="selectedPatient">the patient to schedule surgery for.</param>
        /// <param name="selectedSurgeon">The surgeon performing the surgery.</param>
        /// <param name="surgeryDate">The DateTime of the surgery.</param>
        /// <param name="dateTimeFormat">The format for displaying the surgery date.</param>
        /// <returns>A message indicating the result of the method.</returns>
        public string ScheduleSurgery(Patient selectedPatient, Surgeon selectedSurgeon, DateTime surgeryDate, string dateTimeFormat)
        {
            // Schedule the surgery and update the patient's surgery schedule
            SurgerySchedule surgerySchedule = new SurgerySchedule(selectedPatient, selectedSurgeon, surgeryDate);
            _hospital.AddScheduledSurgery(surgerySchedule);
            selectedPatient.ScheduledSurgery = surgerySchedule;

            return $"Surgeon {selectedSurgeon.Name} has been assigned to patient {selectedPatient.Name}." +
                    $"\nSurgery will take place on {surgeryDate.ToString(dateTimeFormat)}.";
        }

        /// <summary>
        /// Displays the details of the floor manager.
        /// </summary>
        /// <returns>A list of strings containing the details.</returns>
        public override List<string> GetDetails()
        {
            // Retrieve base details from the Staff class
            List<string> details = base.GetDetails();

            // Add additional details specific to floor managers
            details.Add($"Floor: {FloorManaged.FloorNumber}.");

            return details;
        }
    }
}
