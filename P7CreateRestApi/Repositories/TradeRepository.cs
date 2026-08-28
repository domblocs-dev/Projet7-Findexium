using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories;

public class TradeRepository
{
    private readonly LocalDbContext _context;

    public TradeRepository(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Trade>> FindAll()
    {
        return await _context.Trades.ToListAsync();
    }

    public async Task<Trade?> FindById(int id)
    {
        return await _context.Trades.FindAsync(id);
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.Trades.AnyAsync(t => t.TradeId == id);
    }

    public async Task<Trade> Add(Trade trade)
    {
        _context.Trades.Add(trade);
        await _context.SaveChangesAsync();
        return trade;
    }

    public async Task<Trade> Update(Trade trade)
    {
        _context.Trades.Update(trade);
        await _context.SaveChangesAsync();
        return trade;
    }

    public async Task<Trade?> Delete(int id)
    {
        Trade? trade = await _context.Trades.FindAsync(id);
        if (trade is null)
        {
            return null;
        }
        _context.Trades.Remove(trade);
        await _context.SaveChangesAsync();
        return trade;
    }
}