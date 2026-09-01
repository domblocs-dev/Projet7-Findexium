using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly TradeRepository _tradeRepository;
    private readonly ILogger<TradeController> _logger;

    public TradeController(TradeRepository tradeRepository, ILogger<TradeController> logger)
    {
        _tradeRepository = tradeRepository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Trade>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        List<Trade> trades = await _tradeRepository.FindAll();
        return Ok(trades);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Trade), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Trade? trade = await _tradeRepository.FindById(id);
        if (trade is null)
        {
            return NotFound();
        }
        return Ok(trade);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Trade), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] Trade trade)
    {
        Trade created = await _tradeRepository.Add(trade);
        _logger.LogInformation("Trade {Id} cree par {User}", created.TradeId, User.Identity?.Name);
        return CreatedAtAction(nameof(GetById), new { id = created.TradeId }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Trade), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] Trade trade)
    {
        if (!await _tradeRepository.Exists(id))
        {
            return NotFound();
        }
        trade.TradeId = id;
        await _tradeRepository.Update(trade);
        _logger.LogInformation("Trade {Id} modifie par {User}", id, User.Identity?.Name);
        return Ok(trade);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Trade), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        Trade? deleted = await _tradeRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        _logger.LogInformation("Trade {Id} supprime par {User}", id, User.Identity?.Name);
        return Ok(deleted);
    }
}