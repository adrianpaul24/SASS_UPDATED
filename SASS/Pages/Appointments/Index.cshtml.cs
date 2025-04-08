using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SASS.Data;

namespace SASS.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SASS.Models.Appointments> AppointmentsList { get; set; }

        public async Task OnGetAsync()
        {
            AppointmentsList = await _context.Appointments
                .Include(a => a.User)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}