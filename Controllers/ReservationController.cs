using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Migrations;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers
{
    public class ReservationController : Controller
	{
		private readonly Context? _context;

		public ReservationController(Context? context)
		{
			_context = context;
		}

		public async Task<IActionResult> Display(int UserID, int DesignationID)
		{
			IQueryable<Reservation> query = _context!.Reservations
										.Include(r => r.Facility)
										.Include(r => r.User);

			var reservationList = new List<Reservation>();

			if (DesignationID != 1)
			{
                reservationList = await Utility.GetReservationList(_context, UserID);
			} else
            {
				reservationList  = await Utility.GetReservationList(_context);
			}

            return View(reservationList);
		}

        public async Task<IActionResult> Create(int UserID, ReservationViewModel reservationViewModel = null!)
        {
			var facilities = await Utility.GetFacilityList(_context);

            TempData["Facilities"] = facilities.Where(f => f.Status != "Unavailable").Select(f => new SelectListItem
            {
                Value = f.FacilityID.ToString(),
                Text = f.FacilityName
            }).ToList();

			reservationViewModel.User = await Utility.GetUser(UserID, _context);
            Console.WriteLine("here");
            return View(reservationViewModel);
        }

		// Refactored version
        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel reservationViewModel)
        {
            if ((!string.IsNullOrEmpty(Request.Form["ReserveTimeStart"])) && (!string.IsNullOrEmpty(Request.Form["ReserveTimeEnd"])))
            {
                reservationViewModel.ReserveTimeStart = TimeSpan.Parse(Request.Form["ReserveTimeStart"]!);
                reservationViewModel.ReserveTimeEnd = TimeSpan.Parse(Request.Form["ReserveTimeEnd"]!);
            } 
			else
			{
				ModelState.AddModelError(string.Empty, "Check for mistakes in the requested time slot.");
                return View("Display", new { reservationViewModel.User.UserID });
            }

			if (!ModelState.IsValid)
			{
                Console.WriteLine("Invalid Reservation");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ToString} : {error.ErrorMessage}");
                }
                return View("Display", new { reservationViewModel.User.UserID });
            }

            var reservationList = (await Utility.GetReservationList(_context, reservationViewModel.UserID))
                                                            .Where(f => f.FacilityID == reservationViewModel.FacilityID
                                                                    && (f.ReserveDate == reservationViewModel.ReserveDate))
                                                            .ToList();

            TimeBlock checkTimeBlock = new TimeBlock(reservationViewModel.ReserveTimeStart, reservationViewModel.ReserveTimeEnd);
			
            bool isValid = (!reservationList.Any()) || (Utility.CheckTimeBlocks(reservationList, checkTimeBlock) == true);

            if (isValid)
			{
				var reservation = new Reservation
				{
					RequestDateTime = reservationViewModel.RequestDateTime,
					ReserveDate = reservationViewModel.ReserveDate,
					ReserveTimeStart = reservationViewModel.ReserveTimeStart,
					ReserveTimeEnd = reservationViewModel.ReserveTimeEnd,
					Purpose = reservationViewModel.Purpose,
					FacilityID = reservationViewModel.FacilityID,
					UserID = reservationViewModel.UserID
				};

				_context.Reservations.Add(reservation);
				await _context.SaveChangesAsync();

				Console.WriteLine("Valid Reservation");
				Console.WriteLine("User ID: " + reservation.User?.UserID);

				return RedirectToAction("Display", new { UserID = reservation.User?.UserID });
			}
			else
			{
				Console.WriteLine("Invalid Reservation");
				Console.WriteLine("User ID: " + reservationViewModel.UserID);

                ViewData["Facilities"] = TempData["Facilities"];

				ModelState.AddModelError(string.Empty, "Choose another schedule");
				return View("Create", reservationViewModel);
			} 
        }

        public async Task<IActionResult> Edit(int RequestID) 
		{
			Console.WriteLine("RequestID: " + RequestID);

            var facilities = await Utility.GetFacilityList(_context);

			TempData["Facilities"] = facilities.Where(f => f.Status != "Unavailable").Select(f => new SelectListItem
			{
				Value = f.FacilityID.ToString(),
				Text = f.FacilityName
			}).ToList();

			var reservation = await Utility.GetReservation(_context, RequestID);				
			return View(reservation);
		}

		// Refactored Version
        [HttpPost]
        public async Task<IActionResult> Edit(Reservation reservation, int UserID)
        {
			if(reservation == null)
			{
                return View(reservation);
            }

            List<Reservation> reservationList = (await Utility.GetReservationList(_context)).
                                                                Where(f => f.FacilityID == reservation.FacilityID 
                                                                && f.ReserveDate == reservation.ReserveDate)
                                                                .ToList();

            TimeBlock currentTimeBlock = new TimeBlock(reservation.ReserveTimeStart, reservation.ReserveTimeEnd);

            reservationList.Remove(reservationList.FirstOrDefault(r => r.RequestID == reservation.RequestID)!);
			
            bool isValid = ModelState.IsValid && ((!reservationList.Any()) || (Utility.CheckTimeBlocks(reservationList, currentTimeBlock) == true));

            if (isValid)
            {
                Console.WriteLine("Valid edit");
                Console.WriteLine("Request ID: " + reservation.RequestID);

                await EditReservation(reservation);
                return RedirectToAction("Display", new { UserID = reservation.UserID });
            }
            else
            {
                Console.WriteLine("Invalid Reservation");
                Console.WriteLine("Request ID: " + reservation.RequestID);

                var facilities = await _context!.Facilities.Where(f => f.Status != "Unavailable").ToListAsync();

                ViewData["Facilities"] = facilities.Select(f => new SelectListItem
                {
                    Value = f.FacilityID.ToString(),
                    Text = f.FacilityName
                }).ToList();

                ModelState.AddModelError(string.Empty, "Choose another schedule.");

                return View("Edit", reservation);
            }
        }

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> Delete(int RequestID, int UserID)
		{
			var item = await _context!.Reservations.FindAsync(RequestID);

			if (item != null)
			{
				try
				{
					_context.Reservations.Remove(item);
				}
				catch (Exception e)
				{
					Console.WriteLine("Error: " + e.Message);
				}
			}
			await _context.SaveChangesAsync();
			return RedirectToAction("Display", new { UserID });
		}

        public async Task EditReservation(Reservation reservation)
        {
            var ToEdit = await _context!.Reservations.FindAsync(reservation.RequestID);

            ToEdit!.ReserveDate = reservation.ReserveDate;
            ToEdit.ReserveTimeStart = reservation.ReserveTimeStart;
            ToEdit.ReserveTimeEnd = reservation.ReserveTimeEnd;
            ToEdit.Purpose = reservation.Purpose;
			ToEdit.Status = reservation.Status;
            ToEdit.FacilityID = reservation.FacilityID;

            await _context.SaveChangesAsync();
        }
    }
}
