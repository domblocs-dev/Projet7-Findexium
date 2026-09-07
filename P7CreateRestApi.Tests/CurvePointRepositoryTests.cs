using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Tests;

public class CurvePointRepositoryTests
{
    private static LocalDbContext CreateContext()
    {
        DbContextOptions<LocalDbContext> options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Add_Ajoute_EtGenereUnId()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);

        CurvePoint created = await repository.Add(new CurvePoint { CurveId = 1, CurvePointValue = 10.0 });

        Assert.True(created.Id > 0);
        Assert.Equal(1, context.CurvePoints.Count());
    }

    [Fact]
    public async Task FindById_Renvoie_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);
        CurvePoint c = await repository.Add(new CurvePoint { CurveId = 1, CurvePointValue = 10.0 });

        CurvePoint? found = await repository.FindById(c.Id);

        Assert.NotNull(found);
        Assert.Equal(10.0, found!.CurvePointValue);
    }

    [Fact]
    public async Task FindById_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);

        Assert.Null(await repository.FindById(999));
    }

    [Fact]
    public async Task FindAll_RenvoieTout()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);
        await repository.Add(new CurvePoint { CurveId = 1, CurvePointValue = 1.0 });
        await repository.Add(new CurvePoint { CurveId = 2, CurvePointValue = 2.0 });

        List<CurvePoint> all = await repository.FindAll();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task Update_Modifie()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);
        CurvePoint c = await repository.Add(new CurvePoint { CurveId = 1, CurvePointValue = 10.0 });
        c.CurvePointValue = 20.0;

        await repository.Update(c);

        CurvePoint? updated = await repository.FindById(c.Id);
        Assert.Equal(20.0, updated!.CurvePointValue);
    }

    [Fact]
    public async Task Delete_Supprime_EtRenvoieLObjet()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);
        CurvePoint c = await repository.Add(new CurvePoint { CurveId = 1, CurvePointValue = 10.0 });

        CurvePoint? deleted = await repository.Delete(c.Id);

        Assert.NotNull(deleted);
        Assert.Equal(0, context.CurvePoints.Count());
    }

    [Fact]
    public async Task Delete_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);

        Assert.Null(await repository.Delete(999));
    }

    [Fact]
    public async Task Exists_Vrai_SiPresent_Faux_Sinon()
    {
        using LocalDbContext context = CreateContext();
        CurvePointRepository repository = new CurvePointRepository(context);
        CurvePoint c = await repository.Add(new CurvePoint { CurveId = 1, CurvePointValue = 10.0 });

        Assert.True(await repository.Exists(c.Id));
        Assert.False(await repository.Exists(999));
    }
}
