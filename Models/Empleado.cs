namespace EmployerSection.Models
{
    public class Empleado
    {
        public int ? Id { get; set; }
        public string ? Nombres { get; set; }
        public string ? Apellidos { get; set; }
        public string ? Correo { get; set; }

        public string ? Contraseña { get; set; }

        public DateTime ? Ultima_Hora_Entrada { get; set; }

        public DateTime ? Ultima_Hora_Salida { get; set; }

    }
}