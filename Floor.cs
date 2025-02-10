namespace HospitalSystem
{
    /// <summary>
    /// A floor in the hospital, containing a list of rooms.
    /// </summary>
    public class Floor
    {
        public int FloorNumber { get; private set; }
        public List<Room> Rooms { get; private set; }
        public FloorManager AssignedManager { get; private set; }

        public Floor(int floorNumber, int roomCount)
        {
            FloorNumber = floorNumber;
            Rooms = new List<Room>();

            // Initialise rooms for each floor
            for (int i = 1; i <= roomCount; i++)
            {
                Rooms.Add(new Room(i, floorNumber));
            }
        }

        /// <summary>
        /// Assigns a floor manager to this floor.
        /// </summary>
        /// <param name="manager">The FloorManager to assign.</param>
        public bool AssignManager(FloorManager manager)
        {
            if (AssignedManager != null)
            {
                return false;
            }
            else
            {
                AssignedManager = manager;
                return true;
            }
        }

        /// <summary>
        /// Checks if all rooms on this floor are occupied.
        /// </summary>
        /// <returns>True if all rooms are occupied, false otherwise.</returns>
        public bool IsFloorFull()
        {
            if (Rooms.All(r => r.IsOccupied))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the room instance matching the room number on this floor.
        /// </summary>
        /// <param name="roomNumber">The number of the room to be retrieved.</param>
        /// <returns>The specified room object.</returns>
        public Room GetRoom(int roomNumber)
        {
            return Rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
        }
    }
}
