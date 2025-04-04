using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiOAuthEmpleados.Models
{
    public class EmpleadoModel
    {
        public int IdEmpleado { get; set; }
        public string Apellido { get; set; }
        public string Oficio { get; set; }
        public int IdDepartamento { get; set; }
    }
}
