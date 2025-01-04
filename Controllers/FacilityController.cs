using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Controllers
{
	public class FacilityController : Controller
	{
		private readonly Context? _context;

		public FacilityController(Context? context)
		{
			_context = context;
		}

		public async Task<IActionResult> Display(int UserID)
		{
            Console.WriteLine(UserID);
            var user = await _context.Users
                                        .Include(d => d.Designation)
                                        .FirstOrDefaultAsync(u => u.UserID == UserID);

            var userViewModel = new UserViewModel()
            {
                User = user
            };

            ViewData["Facilities"] = (await Utility.GetFacilityList(_context)).ToList();

			return View(userViewModel);
		}

		public IActionResult Create(int UserID)
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("FacilityName, Floor, Number, Status")] Facility facility, int UserID)
		{
			facility.FacilityName = facility.FacilityName.ToUpper();
            Console.WriteLine("Valid Facility? " + facility.FacilityName);
            if (ModelState.IsValid)
			{
				_context.Facilities.Add(facility);
				await _context.SaveChangesAsync();
				Console.WriteLine("Added Facilty: " + facility.FacilityName);
				return RedirectToAction("Display");
			} else
			{
                Console.WriteLine("Failed to Add Facilty: " + facility.FacilityName);
            }
			return View();
		}

        public async Task<IActionResult> Edit(int FacilityID, int UserID)
        {
            var facility = new Models.Facility();
            try
            {
                facility = await _context.Facilities.FirstOrDefaultAsync(x => x.FacilityID == FacilityID);
                Console.WriteLine("Editing ID: " + facility!.FacilityID);
            }
            catch (Exception)
            {
                Console.WriteLine("Error editing: " + FacilityID);
                return NotFound();
            }

            return View(facility);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("FacilityID, FacilityName, Floor, Number, Status")] Facility facility, int UserID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("Updating ID: " + facility.FacilityID + "\n\n");
                    _context.Facilities.Update(facility);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Successful updating: " + facility.FacilityID);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    Console.WriteLine("Error editing: " + facility.FacilityID);
                    return NotFound();
                }
                return RedirectToAction("Display", new {UserID});
            }
            return View(facility);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int FacilityID)
        {
            var facility = await _context!.Facilities.FindAsync(FacilityID);

            if (facility != null)
            {
                Console.WriteLine("not null");
                try
                {
                    _context.Facilities.Remove(facility);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Display");
        }

		public async Task<IActionResult> DisplaySchedule(DateTime Date, int FacilityID = 0)
		{
            ScheduleViewModel schedule = new ScheduleViewModel();

            var facilities = (await Utility.GetFacilityList(_context)).ToList();

            ViewData["Facilities"] = facilities.Select(f => new SelectListItem
            {
                Value = f.FacilityID.ToString(),
                Text = f.FacilityName
            }).ToList();

            if (FacilityID != 0)
            {
                List<TimeBlock>? facilitySchedule = await Utility.GetFacilitySchedule(_context, FacilityID, Date);

                if(facilitySchedule != null)
                {
                    schedule = new ScheduleViewModel
                    {
                        Schedule = facilitySchedule,
                        Date = Date,
                        FacilityID = FacilityID
                    };
                    return View("Schedule", schedule);
                }
                ModelState.AddModelError(string.Empty, "No reservations!");
            }
            return View("Schedule", schedule);
		}
	}
}
