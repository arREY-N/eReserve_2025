using Azure.Core;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Migrations;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Models
{
    public class Utility
    {
		public static string SetLayout(int designationID)
		{
			return designationID == 1 ? "_AdminLayout" : "_UserLayout";
		}

		public static async Task<User?> GetUser(int userID, Context? _context)
		{
			return await _context!.Users
                                .Include(d => d.Designation)
                                .FirstOrDefaultAsync(u => u.UserID == userID);
		}

		public static async Task<Reservation?> GetReservation(Context? _context, int requestID)
		{
			return await _context!.Reservations
										.Include(f => f.Facility)
										.Include(u => u.User)
										.FirstOrDefaultAsync(r => r.RequestID == requestID)!;
		}

		public static Facility GetFacility(Context? _context, int facilityID)
		{
			return _context!.Facilities
										.Include(r => r.Reservations)
										.FirstOrDefault(f => f.FacilityID == facilityID)!;
		}

		public static async Task<List<User>> GetUserList(Context? _context)
		{
			return await _context!.Users.Include(d => d.Designation).ToListAsync();
		}

		public static async Task<List<Reservation>> GetReservationList(Context? _context, int userID = 0)
        {
            IQueryable<Reservation> query = _context!.Reservations
                                        .Include(f => f.Facility)
                                        .Include(u => u.User);

            if (userID != 0)
            {
                query = query.Where(u => u.UserID == userID);
            }
            
            return await query
                            .OrderBy(d => d.ReserveDate)    
                            .ThenBy(d => d.ReserveTimeStart)
                            .ToListAsync();
        }

		public static async Task<List<Facility>> GetFacilityList(Context? _context)
		{
			return await _context!.Facilities
										.Include(r => r.Reservations)
										.ToListAsync();
		}

		public static bool CheckTimeBlocks(List<Reservation> reservations, TimeBlock currentTimeBlock)
        {
            List<TimeBlock> schedule = CreateFacilitySchedule(reservations)!;
            TimeSpan schoolOpen = new TimeSpan(7, 0, 0);
            TimeSpan schoolClose = new TimeSpan(20, 30, 0);
            int start = 0;
            int end = schedule.Count;
            int pointer = (start + end) / 2;

            TimeBlock pointerTimeBlock = schedule[pointer];

            bool available = false;

            if ((currentTimeBlock.StartTime >= schoolOpen) && (currentTimeBlock.EndTime <= schoolClose))
            {
                while (start < end)
                {
                    pointerTimeBlock = schedule[pointer];

                    if (currentTimeBlock.StartTime < pointerTimeBlock.StartTime)
                    {
                        end = pointer;
                    }
                    else
                    {
                        start = pointer + 1;
                    }
                    pointer = (start + end) / 2;
                }
            }

            if (pointer >= schedule.Count)
            {
                if (currentTimeBlock.StartTime >= schedule[schedule.Count - 1].EndTime)
                {
                    available = true;
                }
            }
            else if (pointer == 0)
            {
                if (currentTimeBlock.EndTime <= schedule[0].StartTime)
                {
                    available = true;
                }
            }
            else
            {
                if ((currentTimeBlock.StartTime >= schedule[pointer - 1].EndTime) && (currentTimeBlock.EndTime <= schedule[pointer].StartTime))
                {
                    available = true;
                }
            }
            return available;
        }

        public static List<TimeBlock> CreateFacilitySchedule(List<Reservation> reservationList)
        {
            List<TimeBlock> timeBlocks = new List<TimeBlock>();
            foreach (var reservation in reservationList)
            {
                TimeBlock timeBlock = new TimeBlock(reservation.ReserveTimeStart, reservation.ReserveTimeEnd);
                timeBlock.Reservation = reservation;
                timeBlocks.Add(timeBlock);

                Console.WriteLine($"Reserved Date: {reservation.ReserveDate}");
                Console.WriteLine($"Reserved Time: {reservation.ReserveTimeStart}-{reservation.ReserveTimeEnd}\n\n");
            }
            return timeBlocks;
        }

		public static async Task<List<TimeBlock>> GetFacilitySchedule(Context? _context, int facilityID, DateTime date)
		{
            List<Reservation> facilityReservation = await _context!.Reservations
                                        .Include(f => f.Facility)
                                        .Include(u => u.User)
                                        .Where(f => f.FacilityID == facilityID && f.ReserveDate == date)
                                        .OrderBy(t => t.ReserveTimeStart)
										.ToListAsync();

            if (facilityReservation.Any())
            {
                List<TimeBlock> reservedSchedule = CreateFacilitySchedule(facilityReservation);
                List<TimeBlock> vacantSchedule = CreateVacantBlocks(reservedSchedule);

                return vacantSchedule;
            }

            return null;
        }

        public static List<TimeBlock> CreateVacantBlocks(List<TimeBlock> reserved)
        {
            List<TimeBlock> ScheduleBlock = new List<TimeBlock>();
            TimeSpan open = new TimeSpan(7, 0, 0);
            TimeSpan close = new TimeSpan(20, 30, 0);
            TimeBlock timeBlock;
            TimeBlock currentTimeBlock = new TimeBlock();

            if (reserved[0].StartTime != open)
            {
                timeBlock = new TimeBlock(open, reserved[0].StartTime);
                Console.WriteLine($"A: {timeBlock.StartTime}-{timeBlock.EndTime}");
                ScheduleBlock.Add(timeBlock);
            }

            Console.WriteLine($"{reserved[0].StartTime}-{reserved[0].EndTime}");
            ScheduleBlock.Add(reserved[0]);

            for (int i = 1; i < reserved.Count; i++)
            {
                currentTimeBlock = reserved[i];

                if (currentTimeBlock.StartTime != ScheduleBlock[ScheduleBlock.Count - 1].EndTime)
                {
                    timeBlock = new TimeBlock(ScheduleBlock[ScheduleBlock.Count - 1].EndTime, currentTimeBlock.StartTime);
                    Console.WriteLine($"{timeBlock.StartTime}-{timeBlock.EndTime}");
                    ScheduleBlock.Add(timeBlock);
                }
                ScheduleBlock.Add(currentTimeBlock);
                Console.WriteLine($"{currentTimeBlock.StartTime}-{currentTimeBlock.EndTime}");
            }

            if (ScheduleBlock[ScheduleBlock.Count - 1].EndTime != close)
            {
                timeBlock = new TimeBlock(reserved[reserved.Count - 1].EndTime, close);
                Console.WriteLine($"{timeBlock.StartTime}-{timeBlock.EndTime}");
                ScheduleBlock.Add(timeBlock);
            }

            return ScheduleBlock;
        }
    }
}
