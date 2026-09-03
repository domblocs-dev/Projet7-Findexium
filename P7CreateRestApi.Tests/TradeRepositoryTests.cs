using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Tests;

public class TradeRepositoryTests
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
        TradeRepository repository = new TradeRepository(context);

        Trade created = await repository.Add(new Trade { Account = "Compte A" });

        Assert.True(created.TradeId > 0);
        Assert.Equal(1, context.Trades.Count());
    }

    [Fact]
    public async Task FindById_Renvoie_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        TradeRepository repository = new TradeRepository(context);
        Trade t = await repository.Add(new Trade { Account = "Compte A" });

        Trade? found = await repository.FindById(t.TradeId);

        Assert.NotNull(found);
        Assert.Equal("Compte A", found!.Account);
    }

    [Fact]
    public async Task FindById_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        TradeRepository repository = new TradeRepository(context);

        Assert.Null(await repository.FindById(999));
    }

    [Fact]
    public async Task FindAll_RenvoieTout()
    {
        using LocalDbContext context = CreateContext();
        TradeRepository repository = new TradeRepository(context);
        await repository.Add(new Trade { Account = "A" });
        await repository.Add(new Trade { Account = "B" });

        List<Trade> all = await repository.FindAll();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task Update_Modifie()
    {
        using LocalDbContext context = CreateContext();
        TradeRepository repository = new TradeRepository(context);
        Trade t = await repository.Add(new Trade { Account = "Ancien" });
        t.Account = "Nouveau";

        await repository.Update(t);

        Trade? updated = await repository.FindById(t.TradeId);
        Assert.Equal("Nouveau", updated!.Account);
    }

    [Fact]
    public async Task Delete_Supprime_EtRenvoieLObjet()
    {
        using LocalDbContext context = CreateContext();
        TradeRepository repository = new TradeRepository(context);
        Trade t = await repository.Add(new Trade { Account = "A" });

        Trade? deleted = await repository.Delete(t.TradeId);

        Assert.NotNull(deleted);
        Assert.Equal(0, context.Trades.Count());
    }

    [Fact]
    public async Task Delete_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        TradeRepository repository = new TradeRepository(context);

        Assert.Null(await repository.Delete(999));
    }

    [Fact]
    public async Task Exists_Vrai_SiPresent_Faux_Sinon()
    {
        using LocalDbContext context = CreateContext();
        TradeRepository repository = new TradeRepository(context);
        Trade t = await repository.Add(new Trade { Account = "A" });

        Assert.True(await repository.Exists(t.TradeId));
        Assert.False(await repository.Exists(999));
    }
}