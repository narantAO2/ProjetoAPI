namespace OdontoApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = "";

        public string PatientPhone { get; set; } = "";

        public DateTime ScheduledAt { get; set; }

        // Situação atual da consulta (por exemplo: Scheduled, Rescheduled, Canceled)
        public string Status { get; set; } = "";

        public int DentistId { get; set; }

        // Propriedade de navegação para acessar os dados completos do dentista
        public Dentist? Dentist { get; set; }

        public int ProcedureId { get; set; }

        // Propriedade de navegação para acessar os dados completos do procedimento
        public Procedure? Procedure { get; set; }

        public int MaterialId { get; set; }
        
        public Material? Material { get; set; }
    }
}

