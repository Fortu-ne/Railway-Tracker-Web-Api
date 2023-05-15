using Railway.Data;
using Railway.DbDataContext;
using Railway.Interface;

namespace Railway.Repository
{
    public class UserRep : IUser
    {
        private DataContext _context;
        public UserRep(DataContext context)
        {
            _context = context;
        }

        public bool Delete(Admin user)
        {
            var currentUser = Find(user.Email);
            _context.Admins.Remove(currentUser);
            return Save();
        }

        public bool DoesItExist(string email)
        {
            return _context.Admins.Any(r => r.Email == email);
        }


        public Admin Find(string email)
        {
            return _context.Admins.Where(r => r.Email.Trim().ToUpper() == email.ToUpper().TrimEnd()).FirstOrDefault();
        }

        public IEnumerable<Admin> GetAll()
        {
            return _context.Admins.ToList();
        }

        public bool Insert(Admin user)
        {
            _context.Admins.Add(user);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }

        //public bool TokenValid(string token)
        //{
        //    var user = _context.Admins.FirstOrDefault(r => r.Verfication == token);
        //    if (user != null)
        //    {
        //        user.VerifiedTime = DateTime.Now;
        //    }

        //    return Save();
        //}

        public bool Update(Admin user)
        {
            _context.Admins.Update(user);
            return Save();
        }
    }

    public class  SupervisorRep :ISupervisor
    {
       
            private DataContext _context;
            public SupervisorRep(DataContext context)
            {
                _context = context;
            }

            public bool Delete(Supervisior user)
            {
                var currentUser = Find(user.Email);
                _context.Supervisiors.Remove(currentUser);
                return Save();
            }

            public bool DoesItExist(string email)
            {
                return _context.Supervisiors.Any(r => r.Email == email);
            }


            public Supervisior Find(string email)
            {
                return _context.Supervisiors.Where(r => r.Email.Trim().ToUpper() == email.ToUpper().TrimEnd()).FirstOrDefault();
            }

            public IEnumerable<Supervisior> GetAll()
            {
                return _context.Supervisiors.ToList();
            }

            public bool Insert(Supervisior user)
            {
                _context.Supervisiors.Add(user);
                return Save();
            }

            public bool Save()
            {
                return _context.SaveChanges() > 0 ? true : false;
            }

            //public bool TokenValid(string token)
            //{
            //    var user = _context.Supervisiors.FirstOrDefault(r => r.Verfication == token);
            //    if (user != null)
            //    {
            //        user.VerifiedTime = DateTime.Now;
            //    }

            //    return Save();
            //}

            public bool Update(Supervisior user)
            {
                _context.Supervisiors.Update(user);
                return Save();
            }

      
    }
}
