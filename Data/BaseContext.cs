using Microsoft.EntityFrameworkCore;
using EmpleadoLogin.Models;

namespace EmpleadoLogin.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }
        public DbSet<Empleado> Empleados { get; set; }
    }
}