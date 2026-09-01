using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BidListController : ControllerBase
{
    private readonly BidListRepository _bidListRepository;
    private readonly ILogger<BidListController> _logger;

    public BidListController(BidListRepository bidListRepository, ILogger<BidListController> logger)
    {
        _bidListRepository = bidListRepository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<BidList>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        List<BidList> bidLists = await _bidListRepository.FindAll();
        return Ok(bidLists);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BidList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        BidList? bidList = await _bidListRepository.FindById(id);
        if (bidList is null)
        {
            return NotFound();
        }
        return Ok(bidList);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BidList), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] BidList bidList)
    {
        BidList created = await _bidListRepository.Add(bidList);
        _logger.LogInformation("Bid {Id} cree par {User}", created.BidListId, User.Identity?.Name);
        return CreatedAtAction(nameof(GetById), new { id = created.BidListId }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BidList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] BidList bidList)
    {
        if (!await _bidListRepository.Exists(id))
        {
            return NotFound();
        }
        bidList.BidListId = id;
        await _bidListRepository.Update(bidList);
        _logger.LogInformation("Bid {Id} modifie par {User}", id, User.Identity?.Name);
        return Ok(bidList);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(BidList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        BidList? deleted = await _bidListRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        _logger.LogInformation("Bid {Id} supprime par {User}", id, User.Identity?.Name);
        return Ok(deleted);
    }
}