namespace Railway.Dtos
{
    public class StationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ArrivalTime { get; set; }
        public int TrainId { get; set; }
        public TrainDto Train { get; set; }


    }
}
