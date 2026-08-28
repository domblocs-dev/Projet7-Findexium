using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dot.Net.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RuleNameController : ControllerBase
{
    private readonly RuleNameRepository _ruleNameRepository;

    public RuleNameController(RuleNameRepository ruleNameRepository)
    {
        _ruleNameRepository = ruleNameRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<RuleName> ruleNames = await _ruleNameRepository.FindAll();
        return Ok(ruleNames);
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> Create([FromBody] RuleName ruleName)
    {
        RuleName created = await _ruleNameRepository.Add(ruleName);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RuleName ruleName)
    {
        if (!await _ruleNameRepository.Exists(id))
        {
            return NotFound();
        }
        ruleName.Id = id;
        await _ruleNameRepository.Update(ruleName);
        return Ok(ruleName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        RuleName? deleted = await _ruleNameRepository.Delete(id);
        if (deleted is null)
        {
            return NotFound();
        }
        return Ok(deleted);
    }
}