using Microsoft.EntityFrameworkCore;
using EmployerSection.Models;

namespace EmployerSection.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<HistorialConexionEmpleadoModel> HistorialConexionEmpleado { get; set; }
    }
}