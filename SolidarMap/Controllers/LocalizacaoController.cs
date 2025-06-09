using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável pelo gerenciamento de localizações.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocalizacaoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todas as localizações cadastradas.
        /// </summary>
        /// <response code="200">Retorna todas as localizações.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalizacaoResponseDTO>>> GetLocalizacoes()
        {
            return await _context.Localizacoes
                .Include(l => l.Ajuda)
                .Include(l => l.TipoZona)
                .Select(l => new LocalizacaoResponseDTO
                {
                    Id = l.Id,
                    AjudaId = l.AjudaId,
                    ZonaId = l.ZonaId,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    Zona = l.TipoZona.Zona
                })
                .ToListAsync();
        }

        /// <summary>
        /// Busca uma localização específica por ID.
        /// </summary>
        /// <param name="id">ID da localização.</param>
        /// <response code="200">Localização encontrada.</response>
        /// <response code="404">Localização não encontrada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<LocalizacaoResponseDTO>> GetLocalizacao(int id)
        {
            var local = await _context.Localizacoes
                .Include(l => l.Ajuda)
                .Include(l => l.TipoZona)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (local == null) return StatusCode(404, $"Não foi encontrado Localização com ID: {id}");

            return new LocalizacaoResponseDTO
            {
                Id = local.Id,
                AjudaId = local.AjudaId,
                ZonaId = local.ZonaId,
                Latitude = local.Latitude,
                Longitude = local.Longitude,
                Zona = local.TipoZona.Zona
            };
        }

        /// <summary>
        /// Busca uma localização associada a uma ajuda.
        /// </summary>
        /// <param name="ajudaId">ID da ajuda.</param>
        /// <response code="200">Localização encontrada.</response>
        /// <response code="404">Localização não encontrada.</response>
        [HttpGet("ajudaId/{ajudaId}")]
        public async Task<ActionResult<LocalizacaoResponseDTO>> GetLocalizacaoPorAjuda(int ajudaId)
        {
            var localizacao = await _context.Localizacoes
                .Include(l => l.TipoZona)
                .FirstOrDefaultAsync(l => l.AjudaId == ajudaId);

            if (localizacao == null)
                return StatusCode(404, $"Não foi encontrada localização para a Ajuda com ID: {ajudaId}");

            return new LocalizacaoResponseDTO
            {
                Id = localizacao.Id,
                AjudaId = localizacao.AjudaId,
                ZonaId = localizacao.ZonaId,
                Latitude = localizacao.Latitude,
                Longitude = localizacao.Longitude,
                Zona = localizacao.TipoZona.Zona
            };
        }

        /// <summary>
        /// Cadastra uma nova localização.
        /// </summary>
        /// <param name="dto">Dados da localização.</param>
        /// <response code="201">Localização criada.</response>
        /// <response code="400">Dados inválidos (ajuda ou zona inexistente).</response>
        /// <response code="500">Erro interno ao salvar no banco de dados.</response>
        [HttpPost]
        public async Task<ActionResult<LocalizacaoResponseDTO>> PostLocalizacao(LocalizacaoRequestDTO dto)
        {
            var ajudaValida = await _context.Ajudas.AnyAsync(a => a.Id == dto.AjudaId);
            var zonaValida = await _context.TiposZona.AnyAsync(z => z.Id == dto.ZonaId);

            if (!ajudaValida || !zonaValida)
                return StatusCode(400, "AjudaId ou ZonaId inválido.");

            var local = new Localizacao
            {
                AjudaId = dto.AjudaId,
                ZonaId = dto.ZonaId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            };

            try
            {
                _context.Localizacoes.Add(local);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetLocalizacao), new { id = local.Id }, new LocalizacaoResponseDTO
            {
                Id = local.Id,
                AjudaId = local.AjudaId,
                ZonaId = local.ZonaId,
                Latitude = local.Latitude,
                Longitude = local.Longitude,
                Zona = (await _context.TiposZona.FindAsync(local.ZonaId))?.Zona
            });
        }

        /// <summary>
        /// Atualiza os dados de uma localização existente.
        /// </summary>
        /// <param name="id">ID da localização.</param>
        /// <param name="dto">Novos dados da localização.</param>
        /// <response code="204">Atualização bem-sucedida.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="404">Localização não encontrada.</response>
        /// <response code="500">Erro ao atualizar no banco.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalizacao(int id, LocalizacaoRequestDTO dto)
        {
            var local = await _context.Localizacoes.FindAsync(id);
            if (local == null) return StatusCode(404, $"Não foi encontradao Localização com ID: {id}");

            var ajudaValida = await _context.Ajudas.AnyAsync(a => a.Id == dto.AjudaId);
            var zonaValida = await _context.TiposZona.AnyAsync(z => z.Id == dto.ZonaId);
            if (!ajudaValida || !zonaValida)
                return StatusCode(400, "AjudaId ou ZonaId inválido.");

            local.AjudaId = dto.AjudaId;
            local.ZonaId = dto.ZonaId;
            local.Latitude = dto.Latitude;
            local.Longitude = dto.Longitude;

            try
            {
                _context.Entry(local).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao atualizar no banco de dados: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove uma localização pelo ID.
        /// </summary>
        /// <param name="id">ID da localização.</param>
        /// <response code="204">Localização excluída com sucesso.</response>
        /// <response code="404">Localização não encontrada.</response>
        /// <response code="500">Erro ao remover do banco.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalizacao(int id)
        {
            var local = await _context.Localizacoes.FindAsync(id);
            if (local == null) return StatusCode(404, $"Não foi encontrado Localização com ID: {id}");

            try
            {
                _context.Localizacoes.Remove(local);
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
