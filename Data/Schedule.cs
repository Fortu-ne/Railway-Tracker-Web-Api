using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Railway.Data
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TimeOnly Depature { get; set; }
        public TimeOnly Arrival { get; set; }
        public TimeOnly Duration { get; set; }
        public bool IsDelayed { get; set; } = false;
        public int TrainId { get; set; }
        public virtual Train Train { get; set; }
    }
}
