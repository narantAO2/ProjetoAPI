namespace OdontoApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = "";

        public string PatientPhone { get; set; } = "";

        public DateTime ScheduledAt { get; set; }

        public string Status { get; set; } = "";

        public int DentistId { get; set; }

        public Dentist? Dentist { get; set; }

        public int ProcedureId { get; set; }

        public Procedure? Procedure { get; set; }

        public int MaterialId { get; set; }
        
        public Material? Material { get; set; }
    }
}


