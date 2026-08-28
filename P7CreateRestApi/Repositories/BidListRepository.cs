using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories;

public class BidListRepository
{
    private readonly LocalDbContext _context;

    public BidListRepository(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<List<BidList>> FindAll()
    {
        return await _context.BidLists.ToListAsync();
    }

    public async Task<BidList?> FindById(int id)
    {
        return await _context.BidLists.FindAsync(id);
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.BidLists.AnyAsync(b => b.BidListId == id);
    }

    public async Task<BidList> Add(BidList bidList)
    {
        _context.BidLists.Add(bidList);
        await _context.SaveChangesAsync();
        return bidList;
    }

    public async Task<BidList> Update(BidList bidList)
    {
        _context.BidLists.Update(bidList);
        await _context.SaveChangesAsync();
        return bidList;
    }

    public async Task<BidList?> Delete(int id)
    {
        BidList? bidList = await _context.BidLists.FindAsync(id);
        if (bidList is null)
        {
            return null;
        }
        _context.BidLists.Remove(bidList);
        await _context.SaveChangesAsync();
        return bidList;
    }
}