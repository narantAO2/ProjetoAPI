namespace OdontoApi.Models
{
    // Entidade que representa uma consulta (Appointment) no sistema
    public class Appointment
    {
        // Chave primária da consulta
        public int Id { get; set; }

        // Nome do paciente da consulta
        public string PatientName { get; set; } = "";

        // Telefone de contato do paciente
        public string PatientPhone { get; set; } = "";

        // Data e hora agendadas para a consulta
        public DateTime ScheduledAt { get; set; }

        // Situação atual da consulta (por exemplo: Scheduled, Rescheduled, Canceled)
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