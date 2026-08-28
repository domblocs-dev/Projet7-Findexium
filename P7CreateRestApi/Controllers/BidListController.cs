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

    public BidListController(BidListRepository bidListRepository)
    {
        _bidListRepository = bidListRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<BidList> bidLists = await _bidListRepository.FindAll();
        return Ok(bidLists);
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> Create([FromBody] BidList bidList)
    {
        BidList created = await _bidListRepository.Add(bidList);
        return CreatedAtAction(nameof(GetById), new { id = created.BidListId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BidList bidList)
    {
        if (!await _bidListRepository.Exists(id))
        {
            return NotFound();
        }
        bidList.BidListId = id;
        await _bidListRepository.Update(bidList);
        return Ok(bidList);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        BidList? deleted = await _bidListRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        return Ok(deleted);
    }
}