using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(typeof(List<Rating>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        List<Rating> ratings = await _ratingRepository.FindAll();
        return Ok(ratings);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Rating), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(Rating), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] Rating rating)
    {
        Rating created = await _ratingRepository.Add(rating);
        _logger.LogInformation("Rating {Id} cree par {User}", created.Id, User.Identity?.Name);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Rating), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    [ProducesResponseType(typeof(Rating), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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