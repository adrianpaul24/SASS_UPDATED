using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SASS.Models
{
    public class Reminders
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointments Appointment { get; set; } // Navigation property

        [Required]
        public string Type { get; set; } // Could be "email", "sms", etc.

        [Required]
        public DateTime Date { get; set; } // Reminder date & time

        [Required]
        public string Status { get; set; } // Pending, sent, failed, etc.
    }
}