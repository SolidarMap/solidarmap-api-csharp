using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolidarMap.Models;
using SolidarMap.Connection;
using SolidarMap.DTO;

namespace SolidarMap.Controllers
{
    /// <summary>
    /// Controlador responsável por gerenciar os usuários do sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os usuários cadastrados.
        /// </summary>
        /// <returns>Lista de usuários</returns>
        /// <response code="200">Retorna a lista de usuários</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDTO>), 200)]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDTO>>> GetUsuarios()
        {
            return await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .Select(u => new UsuarioResponseDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    DataCriacao = u.DataCriacao,
                    TipoUsuarioNome = u.TipoUsuario.NomeTipo
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Usuário correspondente</returns>
        /// <response code="200">Usuário encontrado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UsuarioResponseDTO>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return StatusCode(404, $"Não foi encontrado Usuário com ID: {id}");

            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao,
                TipoUsuarioNome = usuario.TipoUsuario.NomeTipo
            };
        }

        /// <summary>
        /// Retorna um usuário pelo e-mail.
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <returns>Usuário correspondente</returns>
        /// <response code="200">Usuário encontrado</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpGet("email")]
        [ProducesResponseType(typeof(UsuarioResponseDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UsuarioResponseDTO>> GetUsuarioPorEmail([FromQuery] string email)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
                return StatusCode(404, $"Não foi encontrado Usuário com o email: {email}");

            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao,
                TipoUsuarioNome = usuario.TipoUsuario.NomeTipo
            };
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="dto">Dados do novo usuário</param>
        /// <returns>Usuário criado</returns>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Erro de validação nos dados enviados</response>
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioResponseDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UsuarioResponseDTO>> PostUsuario(UsuarioRequestDTO dto)
        {
            var tipoValido = await _context.TiposUsuario.AnyAsync(t => t.Id == dto.TipoUsuarioId);
            if (!tipoValido)
                return StatusCode(404, $"Não foi encontrado Tipo de Usuário com ID: {dto.TipoUsuarioId}");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha,
                TipoUsuarioId = dto.TipoUsuarioId,
                DataCriacao = DateTime.Now
            };

            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao salvar no banco de dados: {ex.Message}");
            }

            var tipo = await _context.TiposUsuario.FindAsync(usuario.TipoUsuarioId);

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataCriacao = usuario.DataCriacao,
                TipoUsuarioNome = tipo?.NomeTipo
            });
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="dto">Novos dados</param>
        /// <response code="204">Usuário atualizado com sucesso</response>
        /// <response code="404">Usuário ou Tipo de Usuário não encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutUsuario(int id, UsuarioRequestDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return StatusCode(404, $"Não foi encontrado Usuário com ID: {id}");

            var tipoValido = await _context.TiposUsuario.AnyAsync(t => t.Id == dto.TipoUsuarioId);
            if (!tipoValido)
                return StatusCode(404, $"Não foi encontrado Tipo de Usuário com ID: {dto.TipoUsuarioId}");

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Senha = dto.Senha;
            usuario.TipoUsuarioId = dto.TipoUsuarioId;

            try
            {
                _context.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Erro ao atualizar no banco de dados: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <response code="204">Usuário removido com sucesso</response>
        /// <response code="404">Usuário não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return StatusCode(404, $"Não foi encontrado Usuário com ID: {id}");

            try
            {
                _context.Usuarios.Remove(usuario);
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
