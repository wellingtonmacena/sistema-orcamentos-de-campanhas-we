using AutoMapper;
using CampaignBudgetingAPI.Data;
using CampaignBudgetingAPI.Models.DTOs;
using CampaignBudgetingAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampaignBudgetingAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClientsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /api/clients/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<ClientResponse>>> GetPagedAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var skip = (pageNumber - 1) * pageSize;
            var totalCount = await _context.Clients.CountAsync();

            var clients = await _context.Clients
                                        .Skip(skip)
                                        .Take(pageSize)
                                        .ToListAsync();

            var response = _mapper.Map<IEnumerable<ClientResponse>>(clients);

            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            Response.Headers.Append("X-Page-Number", pageNumber.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());

            return Ok(response);
        }

        // GET: /api/clients/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ClientResponse>> GetByIdAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound($"Cliente com ID {id} não encontrado.");

            var response = _mapper.Map<ClientResponse>(client);
            return Ok(response);
        }

        // POST: /api/clients
        [HttpPost]
        public async Task<ActionResult<ClientResponse>> CreateAsync([FromBody] ClientCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newClient = _mapper.Map<Client>(request);

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<ClientResponse>(newClient);
            return Created($"/{response.Id}", response);
        }

        // PUT: /api/clients/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ClientUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest("O ID na URL não corresponde ao ID do corpo da requisição.");

            var existingClient = await _context.Clients.FindAsync(id);
            if (existingClient == null)
                return NotFound($"Cliente com ID {id} não encontrado.");

            _mapper.Map(request, existingClient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/clients/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return NotFound($"Cliente com ID {id} não encontrado.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
