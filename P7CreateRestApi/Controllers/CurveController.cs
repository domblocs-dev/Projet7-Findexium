using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CurveController : ControllerBase
{
    private readonly CurvePointRepository _curvePointRepository;
    private readonly ILogger<CurveController> _logger;

    public CurveController(CurvePointRepository curvePointRepository, ILogger<CurveController> logger)
    {
        _curvePointRepository = curvePointRepository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CurvePoint>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        List<CurvePoint> curvePoints = await _curvePointRepository.FindAll();
        return Ok(curvePoints);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CurvePoint), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        CurvePoint? curvePoint = await _curvePointRepository.FindById(id);
        if (curvePoint is null)
        {
            return NotFound();
        }
        return Ok(curvePoint);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CurvePoint), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CurvePoint curvePoint)
    {
        CurvePoint created = await _curvePointRepository.Add(curvePoint);
        _logger.LogInformation("CurvePoint {Id} cree par {User}", created.Id, User.Identity?.Name);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CurvePoint), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] CurvePoint curvePoint)
    {
        if (!await _curvePointRepository.Exists(id))
        {
            return NotFound();
        }
        curvePoint.Id = id;
        await _curvePointRepository.Update(curvePoint);
        _logger.LogInformation("CurvePoint {Id} modifie par {User}", id, User.Identity?.Name);
        return Ok(curvePoint);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(CurvePoint), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        CurvePoint? deleted = await _curvePointRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        _logger.LogInformation("CurvePoint {Id} supprime par {User}", id, User.Identity?.Name);
        return Ok(deleted);
    }
}