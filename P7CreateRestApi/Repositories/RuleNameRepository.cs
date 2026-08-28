using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories;

public class RuleNameRepository
{
    private readonly LocalDbContext _context;

    public RuleNameRepository(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<List<RuleName>> FindAll()
    {
        return await _context.RuleNames.ToListAsync();
    }

    public async Task<RuleName?> FindById(int id)
    {
        return await _context.RuleNames.FindAsync(id);
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.RuleNames.AnyAsync(r => r.Id == id);
    }

    public async Task<RuleName> Add(RuleName ruleName)
    {
        _context.RuleNames.Add(ruleName);
        await _context.SaveChangesAsync();
        return ruleName;
    }

    public async Task<RuleName> Update(RuleName ruleName)
    {
        _context.RuleNames.Update(ruleName);
        await _context.SaveChangesAsync();
        return ruleName;
    }

    public async Task<RuleName?> Delete(int id)
    {
        RuleName? ruleName = await _context.RuleNames.FindAsync(id);
        if (ruleName is null)
        {
            return null;
        }
        _context.RuleNames.Remove(ruleName);
        await _context.SaveChangesAsync();
        return ruleName;
    }
}