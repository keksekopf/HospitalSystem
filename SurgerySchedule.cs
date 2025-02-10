namespace HospitalSystem
{
    /// <summary>
    /// Keeps track of the patient's scheduled surgeries
    /// </summary>
    public class SurgerySchedule
    {
        // Details of the scheduled surgery
        public Patient ScheduledPatient { get; private set; }
        public Surgeon AssignedSurgeon { get; private set; }
        public DateTime SurgeryDateTime { get; private set; }
        public bool SurgeryCompleted { get; private set; } = false;

        public SurgerySchedule (Patient scheduledPatient, Surgeon assignedSurgeon, DateTime surgeryDateTime)
        {
            ScheduledPatient = scheduledPatient;
            AssignedSurgeon = assignedSurgeon;
            SurgeryDateTime = surgeryDateTime;
        }

        /// <summary>
        /// Marks a surgery as completed.
        /// </summary>
        public void CompleteSurgery()
        {
            SurgeryCompleted = true;
        }
    }
}
