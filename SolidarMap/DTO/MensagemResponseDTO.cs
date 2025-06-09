namespace SolidarMap.DTO
{
    public class MensagemResponseDTO
    {
        public int Id { get; set; }
        public int AjudaId { get; set; }
        public int UsuarioId { get; set; }
        public string Conteudo { get; set; }
        public DateTime DataEnvio { get; set; }
        public string NomeUsuario { get; set; }
    }
}
