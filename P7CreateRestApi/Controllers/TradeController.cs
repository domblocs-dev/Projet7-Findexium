using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly TradeRepository _tradeRepository;

    public TradeController(TradeRepository tradeRepository)
    {
        _tradeRepository = tradeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Trade> trades = await _tradeRepository.FindAll();
        return Ok(trades);
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> Create([FromBody] Trade trade)
    {
        Trade created = await _tradeRepository.Add(trade);
        return CreatedAtAction(nameof(GetById), new { id = created.TradeId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Trade trade)
    {
        if (!await _tradeRepository.Exists(id))
        {
            return NotFound();
        }
        trade.TradeId = id;
        await _tradeRepository.Update(trade);
        return Ok(trade);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Trade? deleted = await _tradeRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        return Ok(deleted);
    }
}