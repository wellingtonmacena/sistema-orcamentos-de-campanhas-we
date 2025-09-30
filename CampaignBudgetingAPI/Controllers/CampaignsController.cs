using AutoMapper;
using CampaignBudgetingAPI.Api.DTOs.Requests;
using CampaignBudgetingAPI.Api.DTOs.Responses;
using CampaignBudgetingAPI.Data;
using CampaignBudgetingAPI.Models.DTOs;
using CampaignBudgetingAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampaignBudgetingAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CampaignsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: /api/campaigns/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<CampaignResponse>>> GetPagedAsync(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var skip = (pageNumber - 1) * pageSize;
            var totalCount = await _context.Campaigns.CountAsync();

            var campaigns = await _context.Campaigns
                                          .Include(c => c.Client)
                                          .Include(c => c.User)
                                          .Skip(skip)
                                          .Take(pageSize)
                                          .ToListAsync();

            var response = _mapper.Map<IEnumerable<CampaignResponse>>(campaigns);

            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            Response.Headers.Append("X-Page-Number", pageNumber.ToString());
            Response.Headers.Append("X-Page-Size", pageSize.ToString());

            return Ok(response);
        }


        // GET: /api/campaigns/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CampaignResponse>> GetByIdAsync(Guid id)
        {
            var campaign = await _context.Campaigns
                                         .Include(c => c.Client)
                                         .Include(c => c.User)
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
                return NotFound($"Campanha com ID {id} não encontrada.");

            var response = _mapper.Map<CampaignResponse>(campaign);
            return Ok(response);
        }

        // POST: /api/campaigns
        [HttpPost]
        public async Task<ActionResult<CampaignResponse>> CreateAsync([FromBody] CampaignCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCampaign = _mapper.Map<Campaign>(request);

            _context.Campaigns.Add(newCampaign);
            await _context.SaveChangesAsync();

            var response = _mapper.Map<CampaignResponse>(newCampaign);
            return Created($"/{response.Id}", response);

        }

        // PUT: /api/campaigns/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] CampaignUpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest("O ID na URL não corresponde ao ID do corpo da requisição.");

            var existingCampaign = await _context.Campaigns.FindAsync(id);
            if (existingCampaign == null)
                return NotFound($"Campanha com ID {id} não encontrada.");

            _mapper.Map(request, existingCampaign);
            existingCampaign.MarkAsUpdated();

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: /api/campaigns/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var campaign = await _context.Campaigns.FindAsync(id);
            if (campaign == null)
                return NotFound($"Campanha com ID {id} não encontrada.");

            _context.Campaigns.Remove(campaign);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
