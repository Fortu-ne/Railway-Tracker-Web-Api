using Railway.Data;

namespace Railway.Interface
{
    public interface ITrain
    {   
            bool Create(Train train);
            ICollection<Train> GetTrains();
            bool Delete(Train train);
            bool Update(Train train);
            bool DoesItExist(int id);
            bool DoesITExist(string name);
            Train GetTrain(string name);
            Train GetTrain(int id);
            bool Save();
        
    }

    public interface ISchedule
    {
        bool Create(int id, Schedule schedule);
        ICollection<Schedule> GetAll();
        bool Delete(Schedule schedule);
        bool Update(Schedule schedule);
        bool DoesItExist(int id);
        Schedule GetSchedule(int id);
        ICollection<Schedule> GetSchedulesByTrain(int trainId);
        ICollection<Schedule> GetSchedulesByTrain(string name);
        bool Save();
    }

    public interface IStation
    {
        bool Create(int id, Station model);
        ICollection<Station> GetAll();
        bool Delete(Station model);
        bool Update(Station model);
        bool DoesItExist(int id);
        bool DoesItExist(string model);
        Station GetStation(int id);
        Station GetStation(string model);
        ICollection<Station> GetStationByTrain(int trainId);
        ICollection<Station> GetStationByTrain(string name);
        bool Save();
    }

    public interface IUser
    {
        bool Insert(Admin user);
        IEnumerable<Admin> GetAll();
        bool Delete(Admin user);
        bool Update(Admin user);
        bool DoesItExist(string email);
        Admin Find(string email);

        //bool TokenValid(string token);
        bool Save();

    }

    public interface ISupervisor
    {
        bool Insert(Supervisior user);
        IEnumerable<Supervisior> GetAll();
        bool Delete(Supervisior user);
        bool Update(Supervisior user);
        bool DoesItExist(string email);
        Supervisior Find(string email);

        //bool TokenValid(string token);
        bool Save();

    }
}
