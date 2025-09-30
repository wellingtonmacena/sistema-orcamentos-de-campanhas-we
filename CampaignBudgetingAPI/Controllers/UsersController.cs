using AutoMapper;
using CampaignBudgetingAPI.Data;
using CampaignBudgetingAPI.Models.DTOs;
using CampaignBudgetingAPI.Models.Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampaignBudgetingAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper; // <--- Injetar AutoMapper

        public UsersController(AppDbContext context, IMapper mapper) // <--- Construtor atualizado
        {
            _context = context;
            _mapper = mapper;
        }

        // ------------------------------------------------------------------
        // GET: /api/users/paged?pageNumber=1&pageSize=10
        // ------------------------------------------------------------------
        /// <summary>
        /// Obtém uma lista paginada de usuários.
        /// </summary>
        /// <param name="pageNumber">O número da página (começando em 1).</param>
        /// <param name="pageSize">O tamanho da página (número de itens).</param>
        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetPagedAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            // Garantir que os parâmetros são válidos
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Limite para evitar abuso

            // Calcular o número de registros a pular
            var skip = (pageNumber - 1) * pageSize;

            // 1. Obter o total de registros para o cabeçalho (opcional, mas recomendado)
            var totalCount = await _context.Users.CountAsync();

            // 2. Aplicar SKIP e TAKE (Paginação)
            var users = await _context.Users
                                    .Skip(skip)
                                    .Take(pageSize)
                                    .ToListAsync();


            // Mapeamento: Entidade para DTO de Resposta
            var response = _mapper.Map<IEnumerable<UserResponse>>(users);

            // ... (Headers de Paginação - permanecem iguais)
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            Response.Headers.Append("X-Page-Number", pageNumber.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());

            return Ok(response); // <--- Retorna a lista de DTOs
        }

        // ------------------------------------------------------------------
        // GET: /api/users/5
        // ------------------------------------------------------------------
        /// <summary>
        /// Obtém um usuário específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do usuário.</param>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            return Ok(user);
        }

        // ------------------------------------------------------------------
        // POST: /api/users
        // ------------------------------------------------------------------
        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="newUser">Dados do novo usuário.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserResponse>> CreateAsync([FromBody] UserCreateRequest request) // <--- Recebe Request DTO
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapeamento: DTO de Requisição para Entidade
            var newUser = _mapper.Map<User>(request);

            // TODO: Aqui você adicionaria a lógica de hash de senha antes de salvar!

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();


            // Mapeamento: Entidade para DTO de Resposta antes de retornar
            var response = _mapper.Map<UserResponse>(newUser);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = response.Id }, response);
        }

        // ------------------------------------------------------------------
        // POST: /api/users/login
        // ------------------------------------------------------------------
        /// <summary>
        /// Realiza o login de um usuário.
        /// </summary>
        /// <param name="loginRequest">Dados de login (e-mail e senha).</param>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserResponse>> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null || user.Password != loginRequest.Password)
            {
                // Em produção, nunca detalhe se o e-mail ou senha está incorreto
                return Unauthorized("E-mail ou senha inválidos.");
            }

            var response = _mapper.Map<UserResponse>(user);
            return Ok(response);
        }

        // ------------------------------------------------------------------
        // PUT: /api/users/5
        // ------------------------------------------------------------------
        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">O ID do usuário a ser atualizado.</param>
        /// <param name="updatedUser">Os novos dados do usuário.</param>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UserUpdateRequest request) // <--- Recebe Request DTO
        {
            if (id != request.Id)
            {
                return BadRequest("O ID na URL não corresponde ao ID do corpo da requisição.");
            }

            if (!await UserExists(id))
            {
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            // 1. Obter a entidade original do banco
            var existingUser = await _context.Users.FindAsync(id);

            // 2. Mapear os dados do DTO para a entidade existente (o AutoMapper só atualiza os campos mapeados)
            _mapper.Map(request, existingUser);

            // Se o EF Core estiver rastreando, basta salvar. Se não, use o modified.
            // _context.Entry(existingUser).State = EntityState.Modified; // Pode ser necessário dependendo do seu contexto

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // ... (tratamento de concorrência permanece igual)
                throw;
            }

            return NoContent();
        }

        // ------------------------------------------------------------------
        // DELETE: /api/users/5
        // ------------------------------------------------------------------
        /// <summary>
        /// Exclui um usuário.
        /// </summary>
        /// <param name="id">O ID do usuário a ser excluído.</param>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"Usuário com ID {id} não encontrado.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // ------------------------------------------------------------------
        // Helper Method
        // ------------------------------------------------------------------
        private async Task<bool> UserExists(Guid id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }
    }
}