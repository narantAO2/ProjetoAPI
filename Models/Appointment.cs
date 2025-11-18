namespace OdontoApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = "";

        public string PatientPhone { get; set; } = "";

        public DateTime ScheduledAt { get; set; }

        public string Status { get; set; } = "";

        // Chave estrangeira que aponta para o dentista responsável
        public int DentistId { get; set; }

        // Propriedade de navegação para acessar os dados completos do dentista
        public Dentist? Dentist { get; set; }

        // Chave estrangeira que aponta para o procedimento realizado na consulta
        public int ProcedureId { get; set; }

        // Propriedade de navegação para acessar os dados completos do procedimento
        public Procedure? Procedure { get; set; }
    }

}
