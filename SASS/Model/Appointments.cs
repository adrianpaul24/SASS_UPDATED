using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SASS.Models
{
    public class Appointments
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AssignedTo { get; set; }

        [ForeignKey("AssignedTo")]
        public Users User { get; set; } // Navigation property

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending; // Enum for status

        [Required]
        public string? Remarks { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified { get; set; }
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}