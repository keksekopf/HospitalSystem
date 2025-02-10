namespace HospitalSystem
{
    /// <summary>
    /// Represents a room in the hospital.
    /// </summary>
    public class Room
    {
        // Room details
        public int RoomNumber { get; private set; }
        public int FloorNumber { get; private set; }
        public Patient AssignedPatient { get; private set; }
        public bool IsOccupied => AssignedPatient != null;

        public Room(int roomNumber, int floorNumber)
        {
            RoomNumber = roomNumber;
            FloorNumber = floorNumber;
        }

        /// <summary>
        /// Assign a patient to a room.
        /// </summary>
        /// <param name="patient">The patient to assign.</param>
        /// <returns>True of the room is unoccupied, false otherwise.</returns>
        public bool AssignPatient(Patient patient)
        {
            if (AssignedPatient == null)
            {
                AssignedPatient = patient;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove a patient from a room.
        /// </summary>
        /// <param name="patient">The patient to remove.</param>
        public void RemovePatient(Patient patient)
        {
            AssignedPatient = null;
            patient.AssignedRoom = null;
        }
    }
}
