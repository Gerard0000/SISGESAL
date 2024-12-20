using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}