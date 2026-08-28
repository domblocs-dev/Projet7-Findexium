using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories;

public class RatingRepository
{
    private readonly LocalDbContext _context;

    public RatingRepository(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rating>> FindAll()
    {
        return await _context.Ratings.ToListAsync();
    }

    public async Task<Rating?> FindById(int id)
    {
        return await _context.Ratings.FindAsync(id);
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.Ratings.AnyAsync(r => r.Id == id);
    }

    public async Task<Rating> Add(Rating rating)
    {
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();
        return rating;
    }

    public async Task<Rating> Update(Rating rating)
    {
        _context.Ratings.Update(rating);
        await _context.SaveChangesAsync();
        return rating;
    }

    public async Task<Rating?> Delete(int id)
    {
        Rating? rating = await _context.Ratings.FindAsync(id);
        if (rating is null)
        {
            return null;
        }
        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync();
        return rating;
    }
}