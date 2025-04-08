using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SASS.Models;

namespace SASS.Models
{
    public class AppointmentLogs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointments Appointment { get; set; } // Navigation property

        [Required]
        public string Action { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public int ChangedByUserId { get; set; } // Renamed for clarity

        [ForeignKey("ChangedByUserId")]
        public Users ChangedByUser { get; set; } // Navigation property
    }
}
