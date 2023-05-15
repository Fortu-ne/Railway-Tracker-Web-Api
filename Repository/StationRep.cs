using Microsoft.EntityFrameworkCore;
using Railway.Data;
using Railway.DbDataContext;
using Railway.Interface;

namespace Railway.Repository
{
    public class StationRep : IStation
    {
        private readonly DataContext _context;

        public StationRep(DataContext context)
        {
            _context = context;
        }

        public bool Create(int id, Station model)
        {
            var train = _context.Trains.Where(r => r.Id == id).FirstOrDefault();

            model.Train = train;
            _context.Stations.Add(model);

            return Save();
        }

        public bool Delete(Station model)
        {
            _context.Stations.Remove(model);

            return Save();
        }

        public bool DoesItExist(int id)
        {
            return _context.Stations.Any(r => r.Id == id);
        }

        public bool DoesItExist(string model)
        {
            return _context.Stations.Any(r => r.Name == model);
        }

        public ICollection<Station> GetAll()
        {
            return _context.Stations.Include(r => r.Train).ToList();
        }

        public Station GetStation(int id)
        {
            return _context.Stations.Where(r => r.Id == id).FirstOrDefault();
        }

        public Station GetStation(string model)
        {
            return _context.Stations.Where(r => r.Name == model).FirstOrDefault();
        }

        public ICollection<Station> GetStationByTrain(int trainId)
        {
            return _context.Stations.Where(r => r.Train.Id == trainId).ToList();
        }

        public ICollection<Station> GetStationByTrain(string name)
        {
            return _context.Stations.Where(r => r.Train.Name == name).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges()>0 ? true : false;
        }

        public bool Update(Station model)
        {
            _context.Stations.Update(model);
            return Save();
        }
    }
}
