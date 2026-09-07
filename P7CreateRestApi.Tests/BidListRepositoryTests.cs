using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Tests;

public class BidListRepositoryTests
{
    private static LocalDbContext CreateContext()
    {
        DbContextOptions<LocalDbContext> options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Add_AjouteUnBid_EtGenereUnId()
    {
        // Arrange
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);
        BidList bid = new BidList { Account = "Compte A", BidType = "Type1" };

        // Act
        BidList created = await repository.Add(bid);

        // Assert
        Assert.True(created.BidListId > 0);
        Assert.Equal(1, context.BidLists.Count());
    }

    [Fact]
    public async Task FindById_RenvoieLeBid_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);
        BidList bid = await repository.Add(new BidList { Account = "Compte B", BidType = "Type1" });

        BidList? found = await repository.FindById(bid.BidListId);

        Assert.NotNull(found);
        Assert.Equal("Compte B", found!.Account);
    }

    [Fact]
    public async Task FindById_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);

        BidList? found = await repository.FindById(999);

        Assert.Null(found);
    }

    [Fact]
    public async Task FindAll_RenvoieTousLesBids()
    {
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);
        await repository.Add(new BidList { Account = "A", BidType = "Type1" });
        await repository.Add(new BidList { Account = "B", BidType = "Type1" });

        List<BidList> all = await repository.FindAll();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task Update_ModifieLeBid()
    {
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);
        BidList bid = await repository.Add(new BidList { Account = "Ancien", BidType = "Type1" });
        bid.Account = "Nouveau";

        await repository.Update(bid);

        BidList? updated = await repository.FindById(bid.BidListId);
        Assert.Equal("Nouveau", updated!.Account);
    }

    [Fact]
    public async Task Delete_SupprimeLeBid_EtRenvoieLObjet()
    {
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);
        BidList bid = await repository.Add(new BidList { Account = "A supprimer", BidType = "Type1" });

        BidList? deleted = await repository.Delete(bid.BidListId);

        Assert.NotNull(deleted);
        Assert.Equal(0, context.BidLists.Count());
    }

    [Fact]
    public async Task Delete_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);

        BidList? deleted = await repository.Delete(999);

        Assert.Null(deleted);
    }

    [Fact]
    public async Task Exists_Vrai_SiPresent_Faux_Sinon()
    {
        using LocalDbContext context = CreateContext();
        BidListRepository repository = new BidListRepository(context);
        BidList bid = await repository.Add(new BidList { Account = "X", BidType = "Type1" });

        Assert.True(await repository.Exists(bid.BidListId));
        Assert.False(await repository.Exists(999));
    }
}
