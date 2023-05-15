using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Railway.Data;

namespace Railway.DbDataContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        public DbSet<Train> Trains => Set<Train>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Station> Stations => Set<Station>();
        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Supervisior> Supervisiors => Set<Supervisior>();


        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {

            builder.Properties<TimeOnly>().HaveConversion<TimeOnlyConverter>().HaveColumnType("time");
            base.ConfigureConventions(builder);
        }


        public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
        {
            public TimeOnlyConverter() : base(
                d => d.ToTimeSpan(),
                d => TimeOnly.FromTimeSpan(d))
            { }
        }
    }
}
