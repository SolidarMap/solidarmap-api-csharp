using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_TIPO_ZONAS_C")]
    public class TipoZona
    {
        [Key]
        [Column("ID_ZONA")]
        public int Id { get; set; }

        [Required, MaxLength(30)]
        [Column("ZONA")]
        public string Zona { get; set; }

        public ICollection<Localizacao> Localizacoes { get; set; }
    }
}
