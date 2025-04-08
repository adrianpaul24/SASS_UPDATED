using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SASS.Data;
using SASS.Models;

namespace SASS.Pages.Reminders
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SASS.Models.Reminders> RemindersList { get; set; }

        public async Task OnGetAsync()
        {
            // Include the related Appointment data when loading Reminders
            RemindersList = await _context.Reminders
                .Include(r => r.Appointment)  // Ensure Appointment is loaded
                .OrderByDescending(r => r.Date)  // Optional: Sort by Date
                .ToListAsync();
        }
    }
}