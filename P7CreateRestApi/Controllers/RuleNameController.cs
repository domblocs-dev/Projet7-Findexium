using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RuleNameController : ControllerBase
{
    private readonly RuleNameRepository _ruleNameRepository;
    private readonly ILogger<RuleNameController> _logger;

    public RuleNameController(RuleNameRepository ruleNameRepository, ILogger<RuleNameController> logger)
    {
        _ruleNameRepository = ruleNameRepository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RuleName>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        List<RuleName> ruleNames = await _ruleNameRepository.FindAll();
        return Ok(ruleNames);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RuleName), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        RuleName? ruleName = await _ruleNameRepository.FindById(id);
        if (ruleName is null)
        {
            return NotFound();
        }
        return Ok(ruleName);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RuleName), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] RuleName ruleName)
    {
        RuleName created = await _ruleNameRepository.Add(ruleName);
        _logger.LogInformation("RuleName {Id} cree par {User}", created.Id, User.Identity?.Name);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RuleName), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] RuleName ruleName)
    {
        if (!await _ruleNameRepository.Exists(id))
        {
            return NotFound();
        }
        ruleName.Id = id;
        await _ruleNameRepository.Update(ruleName);
        _logger.LogInformation("RuleName {Id} modifie par {User}", id, User.Identity?.Name);
        return Ok(ruleName);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(RuleName), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        RuleName? deleted = await _ruleNameRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        _logger.LogInformation("RuleName {Id} supprime par {User}", id, User.Identity?.Name);
        return Ok(deleted);
    }
}