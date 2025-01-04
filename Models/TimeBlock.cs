namespace WebApplication1.Models
{
	public struct TimeBlock
	{
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public Reservation? Reservation { get; set; }
		public TimeBlock(TimeSpan startTime, TimeSpan endTime) 
		{
			StartTime = startTime;
			EndTime = endTime;
		}
	}
}
