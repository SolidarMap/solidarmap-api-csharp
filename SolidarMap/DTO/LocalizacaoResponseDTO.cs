namespace SolidarMap.DTO
{
    public class LocalizacaoResponseDTO
    {
        public int Id { get; set; }
        public int AjudaId { get; set; }
        public int ZonaId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Zona { get; set; }
    }
}
