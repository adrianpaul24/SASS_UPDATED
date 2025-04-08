using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SASS.Data;
using SASS.Models;

namespace SASS.Pages.AppointmentLogs
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SASS.Models.AppointmentLogs> AppointmentLogsList { get; set; }

        public async Task OnGetAsync()
        {
            // Get all appointment logs, including related appointments and users
            AppointmentLogsList = await _context.AppointmentLogs
                .Include(log => log.Appointment) // Include the related Appointment
                .Include(log => log.ChangedByUser) // Include the related User who changed the appointment
                .OrderByDescending(log => log.Timestamp) // Sort by most recent
                .ToListAsync();
        }
    }
}