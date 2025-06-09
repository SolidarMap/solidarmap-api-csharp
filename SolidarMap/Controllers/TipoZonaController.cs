using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os tipos de zona.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TipoZonaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TipoZonaController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os tipos de zona cadastrados.
        /// </summary>
        /// <response code="200">Retorna todos os tipos de zona.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoZonaResponseDTO>>> GetTiposZona()
        {
            return await _context.TiposZona
                .Select(tz => new TipoZonaResponseDTO
                {
                    Id = tz.Id,
                    Zona = tz.Zona
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um tipo de zona pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de zona.</param>
        /// <response code="200">Tipo de zona encontrado.</response>
        /// <response code="404">Tipo de zona não encontrado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoZonaResponseDTO>> GetTipoZona(int id)
        {
            var tipoZona = await _context.TiposZona.FindAsync(id);

            if (tipoZona == null)
                return StatusCode(404, $"Não foi encontrada TipoZona com ID: {id}");

            return new TipoZonaResponseDTO
            {
                Id = tipoZona.Id,
                Zona = tipoZona.Zona
            };
        }

        /// <summary>
        /// Cadastra um novo tipo de zona.
        /// </summary>
        /// <param name="dto">Dados do tipo de zona a ser criado.</param>
        /// <response code="201">Tipo de zona criado com sucesso.</response>
        /// <response code="500">Erro ao salvar no banco de dados.</response>
        [HttpPost]
        public async Task<ActionResult<TipoZonaResponseDTO>> PostTipoZona(TipoZonaRequestDTO dto)
        {
            var tipoZona = new TipoZona
            {
                Zona = dto.Zona
            };

            try
            {
                _context.TiposZona.Add(tipoZona);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetTipoZona), new { id = tipoZona.Id }, new TipoZonaResponseDTO
            {
                Id = tipoZona.Id,
                Zona = tipoZona.Zona
            });
        }

        /// <summary>
        /// Atualiza um tipo de zona existente.
        /// </summary>
        /// <param name="id">ID do tipo de zona a ser atualizado.</param>
        /// <param name="dto">Novos dados do tipo de zona.</param>
        /// <response code="204">Atualização realizada com sucesso.</response>
        /// <response code="404">Tipo de zona não encontrado.</response>
        /// <response code="500">Erro ao atualizar no banco.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoZona(int id, TipoZonaRequestDTO dto)
        {
            var tipoZona = await _context.TiposZona.FindAsync(id);
            if (tipoZona == null)
                return StatusCode(404, $"Não foi encontrada TipoZona com ID: {id}");

            tipoZona.Zona = dto.Zona;

            try
            {
                _context.Entry(tipoZona).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao atualizar no banco de dados: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um tipo de zona pelo ID.
        /// </summary>
        /// <param name="id">ID do tipo de zona a ser removido.</param>
        /// <response code="204">Remoção realizada com sucesso.</response>
        /// <response code="404">Tipo de zona não encontrado.</response>
        /// <response code="500">Erro ao remover do banco.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoZona(int id)
        {
            var tipoZona = await _context.TiposZona.FindAsync(id);
            if (tipoZona == null)
                return StatusCode(404, $"Não foi encontrada TipoZona com ID: {id}");

            try
            {
                _context.TiposZona.Remove(tipoZona);
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
