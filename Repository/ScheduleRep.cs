using Microsoft.EntityFrameworkCore;
using Railway.Data;
using Railway.DbDataContext;
using Railway.Interface;

namespace Railway.Repository
{
    public class ScheduleRep : ISchedule
    {

        private DataContext _context;
        public ScheduleRep(DataContext data)
        {
            _context = data;
        }

        public bool Create(int id, Schedule schedule)
        {
            var model = _context.Trains.Where(r => r.Id == id).FirstOrDefault();

            schedule.Train = model;
            _context.Schedules.Add(schedule);
            return Save();
        }

        public bool Delete(Schedule schedule)
        {
            var model = GetSchedule(schedule.Id);
            _context.Schedules.Remove(model);
            return Save();
        }

        public bool DoesItExist(int id)
        {
            return _context.Schedules.Any(r => r.Id == id);
        }

        public ICollection<Schedule> GetAll()
        {
            return _context.Schedules.Include(r => r.Train).ToList();
        }

        public Schedule GetSchedule(int id)
        {
            return _context.Schedules.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Schedule> GetSchedulesByTrain(int id)
        {
            return _context.Schedules.Where(r => r.Train.Id == id).Include(r => r.Train).ToList();
        }

        public ICollection<Schedule> GetSchedulesByTrain(string name)
        {
            return _context.Schedules.Where(r => r.Train.Name == name).Include(r=>r.Train).ToList();
        }

        public bool Save()
        {
            var model = _context.SaveChanges();
            return model > 0 ? true : false;
        }

        public bool Update(Schedule schedule)
        {
            
            _context.Schedules.Update(schedule);
            return Save();
        }
    }

   
}
