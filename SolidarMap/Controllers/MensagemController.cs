using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de mensagens.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MensagemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MensagemController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todas as mensagens cadastradas.
        /// </summary>
        /// <response code="200">Retorna todas as mensagens.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MensagemResponseDTO>>> GetMensagens()
        {
            return await _context.Mensagens
                .Include(m => m.Usuario)
                .Include(m => m.Ajuda)
                .Select(m => new MensagemResponseDTO
                {
                    Id = m.Id,
                    AjudaId = m.AjudaId,
                    UsuarioId = m.UsuarioId,
                    Conteudo = m.Conteudo,
                    DataEnvio = m.DataEnvio,
                    NomeUsuario = m.Usuario.Nome
                })
                .ToListAsync();
        }

        /// <summary>
        /// Lista as mensagens associadas a uma ajuda específica.
        /// </summary>
        /// <param name="ajudaId">ID da ajuda.</param>
        /// <response code="200">Mensagens encontradas.</response>
        /// <response code="404">Ajuda não encontrada.</response>
        [HttpGet("ajuda/{ajudaId}")]
        public async Task<ActionResult<IEnumerable<MensagemResponseDTO>>> GetMensagensPorAjuda(int ajudaId)
        {
            var existeAjuda = await _context.Ajudas.AnyAsync(a => a.Id == ajudaId);
            if (!existeAjuda)
                return StatusCode(404, $"Ajuda com ID {ajudaId} não encontrada.");

            var mensagens = await _context.Mensagens
                .Where(m => m.AjudaId == ajudaId)
                .Include(m => m.Usuario)
                .Select(m => new MensagemResponseDTO
                {
                    Id = m.Id,
                    AjudaId = m.AjudaId,
                    UsuarioId = m.UsuarioId,
                    Conteudo = m.Conteudo,
                    DataEnvio = m.DataEnvio,
                    NomeUsuario = m.Usuario.Nome
                })
                .ToListAsync();

            return mensagens;
        }

        /// <summary>
        /// Retorna uma mensagem específica pelo ID.
        /// </summary>
        /// <param name="id">ID da mensagem.</param>
        /// <response code="200">Mensagem encontrada.</response>
        /// <response code="404">Mensagem não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<MensagemResponseDTO>> GetMensagem(int id)
        {
            var msg = await _context.Mensagens
                .Include(m => m.Usuario)
                .Include(m => m.Ajuda)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (msg == null)
                return StatusCode(404, $"Não foi encontrada mensagem com ID: {id}");

            return new MensagemResponseDTO
            {
                Id = msg.Id,
                AjudaId = msg.AjudaId,
                UsuarioId = msg.UsuarioId,
                Conteudo = msg.Conteudo,
                DataEnvio = msg.DataEnvio,
                NomeUsuario = msg.Usuario.Nome
            };
        }

        /// <summary>
        /// Cadastra uma nova mensagem.
        /// </summary>
        /// <param name="dto">Dados da mensagem.</param>
        /// <response code="201">Mensagem criada.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="500">Erro ao salvar no banco.</response>
        [HttpPost]
        public async Task<ActionResult<MensagemResponseDTO>> PostMensagem(MensagemRequestDTO dto)
        {
            var ajudaValida = await _context.Ajudas.AnyAsync(a => a.Id == dto.AjudaId);
            var usuarioValido = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);

            if (!ajudaValida || !usuarioValido)
                return StatusCode(400, "AjudaId ou UsuarioId inválido.");

            var mensagem = new Mensagem
            {
                AjudaId = dto.AjudaId,
                UsuarioId = dto.UsuarioId,
                Conteudo = dto.Conteudo,
                DataEnvio = DateTime.Now
            };

            try
            {
                _context.Mensagens.Add(mensagem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetMensagem), new { id = mensagem.Id }, new MensagemResponseDTO
            {
                Id = mensagem.Id,
                AjudaId = mensagem.AjudaId,
                UsuarioId = mensagem.UsuarioId,
                Conteudo = mensagem.Conteudo,
                DataEnvio = mensagem.DataEnvio,
                NomeUsuario = (await _context.Usuarios.FindAsync(mensagem.UsuarioId))?.Nome
            });
        }

        /// <summary>
        /// Atualiza uma mensagem existente.
        /// </summary>
        /// <param name="id">ID da mensagem.</param>
        /// <param name="dto">Novos dados da mensagem.</param>
        /// <response code="204">Mensagem atualizada.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Mensagem não encontrada.</response>
        /// <response code="500">Erro ao atualizar no banco.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensagem(int id, MensagemRequestDTO dto)
        {
            var mensagem = await _context.Mensagens.FindAsync(id);
            if (mensagem == null)
                return StatusCode(404, $"Não foi encontrada mensagem com ID: {id}");

            var ajudaValida = await _context.Ajudas.AnyAsync(a => a.Id == dto.AjudaId);
            var usuarioValido = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);

            if (!ajudaValida || !usuarioValido)
                return StatusCode(400, "AjudaId ou UsuarioId inválido.");

            mensagem.AjudaId = dto.AjudaId;
            mensagem.UsuarioId = dto.UsuarioId;
            mensagem.Conteudo = dto.Conteudo;

            try
            {
                _context.Entry(mensagem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao atualizar no banco de dados: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Exclui uma mensagem pelo ID.
        /// </summary>
        /// <param name="id">ID da mensagem.</param>
        /// <response code="204">Mensagem excluída com sucesso.</response>
        /// <response code="404">Mensagem não encontrada.</response>
        /// <response code="500">Erro ao excluir do banco.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensagem(int id)
        {
            var mensagem = await _context.Mensagens.FindAsync(id);
            if (mensagem == null)
                return StatusCode(404, $"Não foi encontrada mensagem com ID: {id}");

            try
            {
                _context.Mensagens.Remove(mensagem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao remover no banco de dados: {ex.Message}");
            }

            return NoContent();
        }
    }
}
