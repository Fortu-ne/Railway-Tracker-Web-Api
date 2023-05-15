namespace Railway.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string Depature { get; set; } = string.Empty;
        public string Arrival { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int TrainId { get; set; }
        public TrainDto Train { get; set; }
        public bool? IsDelayed { get; set; }
    }
}
