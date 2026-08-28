using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories;

public class CurvePointRepository
{
    private readonly LocalDbContext _context;

    public CurvePointRepository(LocalDbContext context)
    {
        _context = context;
    }

    public async Task<List<CurvePoint>> FindAll()
    {
        return await _context.CurvePoints.ToListAsync();
    }

    public async Task<CurvePoint?> FindById(int id)
    {
        return await _context.CurvePoints.FindAsync(id);
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.CurvePoints.AnyAsync(c => c.Id == id);
    }

    public async Task<CurvePoint> Add(CurvePoint curvePoint)
    {
        _context.CurvePoints.Add(curvePoint);
        await _context.SaveChangesAsync();
        return curvePoint;
    }

    public async Task<CurvePoint> Update(CurvePoint curvePoint)
    {
        _context.CurvePoints.Update(curvePoint);
        await _context.SaveChangesAsync();
        return curvePoint;
    }

    public async Task<CurvePoint?> Delete(int id)
    {
        CurvePoint? curvePoint = await _context.CurvePoints.FindAsync(id);
        if (curvePoint is null)
        {
            return null;
        }
        _context.CurvePoints.Remove(curvePoint);
        await _context.SaveChangesAsync();
        return curvePoint;
    }
}