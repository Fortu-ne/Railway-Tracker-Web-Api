using Microsoft.EntityFrameworkCore;
using Railway.Data;
using Railway.DbDataContext;
using Railway.Interface;


namespace Railway.Repository
{
    public class TrainRep : ITrain
    {
        private readonly DataContext _context;

        public TrainRep(DataContext context)
        {
           _context = context;
        }

        public bool Create(Train train)
        {
            _context.Trains.Add(train);
            return Save();
        }

        public bool Delete(Train train)
        {
            _context.Trains.Remove(train);
            return Save();
        }

        public bool DoesItExist(int id)
        {
            return _context.Trains.Any(r => r.Id == id);
        }

        public bool DoesITExist(string name)
        {
            return _context.Trains.Any(r => r.Name == name);
        }

        public Train GetTrain(string name)
        {
            return _context.Trains.Where(r => r.Name == name).FirstOrDefault();
        }



        public Train GetTrain(int id)
        {
            return _context.Trains.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Train> GetTrains()
        {
            return _context.Trains.Include(r=>r.Stations).Include(r=>r.Schedules).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        public bool Update(Train train)
        {
            _context.Trains.Update(train);
            return Save();
        }
    }
}
