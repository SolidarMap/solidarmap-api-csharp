using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_TIPO_USUARIOS_C")]
    public class TipoUsuario
    {
        [Key]
        [Column("ID_TIPO_USUARIO")]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        [Column("NOME_TIPO")]
        public string NomeTipo { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
