using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_TIPO_RECURSOS_C")]
    public class TipoRecurso
    {
        [Key]
        [Column("ID_RECURSO")]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        [Column("RECURSO")]
        public string Recurso { get; set; }
    }
}
