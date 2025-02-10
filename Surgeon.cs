namespace HospitalSystem
{
    /// <summary>
    /// Represents a surgeon in the hospital
    /// </summary>
    public class Surgeon : Staff
    {
        // Gets or sets the surgeon's speciality
        public string Speciality { get; private set; }

        // Reference to the hospital the surgeon is associated with
        private readonly IHospital _hospital;

        /// <summary>
        /// Initialises a new instace of the Surgeon class.
        /// </summary>
        public Surgeon(string name, int age, string mobile, string email, string password, int staffId, string speciality, IHospital hospital)
            :base(name, age, mobile, email, password, staffId)
        {
            Speciality = speciality;
            _hospital = hospital;
        }

        /// <summary>
        /// Displays a list of patients assigned to this surgeon.
        /// </summary>
        /// <param name="assignedPatients">List of patients assigned to the surgeon.</param>
        /// <returns>A list of strings representing the patients' names.</returns>
        public List<string> ViewPatients(List<Patient> patients)
        {
            // Create a list of patient names to display
            List<string> scheduledPatients = new List<string> { "Your Patients." };

            // Filter patients assigned to this surgeonn
            List<Patient> assignedPatients = patients
                .Where(patient => patient.ScheduledSurgery?.AssignedSurgeon == this)
                .ToList();
            
            // Check if the surgeon has any assigned patients, and output the appropriate message
            scheduledPatients.AddRange(assignedPatients.Any()
                ? assignedPatients.Select((patient, index) => $"{index + 1}. {patient.Name}")
                : ["You do not have any patients assigned."]);

            return scheduledPatients;
        }

        /// <summary>
        /// Displays the surgeon's scheduled surgeries.
        /// </summary>
        /// <param name="patients">List of patients to check for scheduled surgeries.</param>
        /// <param name="dateTimeFormat">The format for displaying the surgery date.</param>
        /// <returns>A list of strings representing the scheduled surgeries.</returns>
        public List<string> ViewSurgerySchedule(List<Patient> patients, string dateTimeFormat)
        {
            List<string> scheduledSurgeries = new List<string> { "Your schedule." };

            // Filter patients to find those with scheduled surgeries assigned to this surgeon
            var assignedSurgeries = patients
                .Where(patient => patient.ScheduledSurgery != null && patient.ScheduledSurgery.AssignedSurgeon == this)
                .Select(patient => new 
                // Create an anonymous type to store the patient's name and surgery date
                {
                    PatientName = patient.Name,
                    SurgeryDate = patient.ScheduledSurgery.SurgeryDateTime
                })
                .OrderBy(schedule => schedule.SurgeryDate)
                .ToList();

            // Check if the surgeon has any assigned surgeries, and output the appropriate message
            scheduledSurgeries.AddRange(assignedSurgeries.Any()
                ? assignedSurgeries.Select(surgery => $"Performing surgery on patient {surgery.PatientName} on {surgery.SurgeryDate.ToString(dateTimeFormat)}")
                : ["You do not have any patients assigned."]);

            return scheduledSurgeries;
        }

        /// <summary>
        /// Surgeon performs a virtual surgery on the selected patient.
        /// </summary>
        /// <param name="assignedPatients">List of patients assigned to the surgeon.</param>
        /// <param name="selectedPatientIndex">Index of the patient to perform surgery on.</param>
        /// <returns>A message indicating the result of the surgery.</returns>
        public string PerformSurgery(Patient patient)
        {
            patient.ScheduledSurgery.CompleteSurgery();
            return $"Surgery performed on {patient.Name} by {Name}.";
        }

        /// <summary>
        /// Displays the details of this surgeon.
        /// </summary>
        /// <returns>A list of strings representing the surgeon's details.</returns>
        public override List<string> GetDetails()
        {
            // Retrieve base details from the Staff class
            List<string> details = base.GetDetails();

            // Add additional details specific to surgeons
            details.Add($"Speciality: {Speciality}");

            return details;
        }
    }
}
