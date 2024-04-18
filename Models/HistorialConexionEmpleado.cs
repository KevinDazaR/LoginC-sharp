using System.ComponentModel.DataAnnotations.Schema;

namespace EmployerSection.Models
{
    public class HistorialConexionEmpleadoModel
    {
        public int ? Id { get; set; }
        public int ? Id_Empleado { get; set; }
        [ForeignKey("Id_Empleado")]
        public DateTime ? Hora_Entrada { get; set; }

        public DateTime ? Hora_Salida { get; set; }

    }
}