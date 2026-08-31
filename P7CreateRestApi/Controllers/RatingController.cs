using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RatingController : ControllerBase
{
    private readonly RatingRepository _ratingRepository;
    private readonly ILogger<RatingController> _logger;

    public RatingController(RatingRepository ratingRepository, ILogger<RatingController> logger)
    {
        _ratingRepository = ratingRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Rating> ratings = await _ratingRepository.FindAll();
        return Ok(ratings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Rating? rating = await _ratingRepository.FindById(id);
        if (rating is null)
        {
            return NotFound();
        }
        return Ok(rating);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Rating rating)
    {
        Rating created = await _ratingRepository.Add(rating);
        _logger.LogInformation("Rating {Id} cree par {User}", created.Id, User.Identity?.Name);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Rating rating)
    {
        if (!await _ratingRepository.Exists(id))
        {
            return NotFound();
        }
        rating.Id = id;
        await _ratingRepository.Update(rating);
        _logger.LogInformation("Rating {Id} modifie par {User}", id, User.Identity?.Name);
        return Ok(rating);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Rating? deleted = await _ratingRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        _logger.LogInformation("Rating {Id} supprime par {User}", id, User.Identity?.Name);
        return Ok(deleted);
    }
}