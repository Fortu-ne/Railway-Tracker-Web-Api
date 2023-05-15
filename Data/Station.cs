using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Railway.Data
{
    public class Station
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
         public TimeOnly ArrivalTime { get; set; }
        public int  TrainId { get; set; }
        public virtual Train Train { get; set; }


    }
}
