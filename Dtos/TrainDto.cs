namespace Railway.Dtos
{
    public class TrainDto
    {
        public int Id { get; set; }
        public string TrainNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        
    }
}
