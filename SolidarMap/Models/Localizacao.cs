using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolidarMap.Models
{
    [Table("T_SMP_LOCALIZACOES_C")]
    public class Localizacao
    {
        [Key]
        [Column("ID_LOCALIZACAO")]
        public int Id { get; set; }

        [Required]
        [Column("ID_AJUDA")]
        public int AjudaId { get; set; }

        [Required]
        [Column("ID_ZONA")]
        public int ZonaId { get; set; }

        [Required]
        [Column("LATITUDE")]
        [Precision(12, 8)]
        public decimal Latitude { get; set; }

        [Required]
        [Column("LONGITUDE")]
        [Precision(12, 8)]
        public decimal Longitude { get; set; }

        [ForeignKey("AjudaId")]
        public Ajuda Ajuda { get; set; }

        [ForeignKey("ZonaId")]
        public TipoZona TipoZona { get; set; }
    }
}
