using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Tests;

public class RuleNameRepositoryTests
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
        RuleNameRepository repository = new RuleNameRepository(context);

        RuleName created = await repository.Add(new RuleName { Name = "Regle1" });

        Assert.True(created.Id > 0);
        Assert.Equal(1, context.RuleNames.Count());
    }

    [Fact]
    public async Task FindById_Renvoie_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        RuleNameRepository repository = new RuleNameRepository(context);
        RuleName r = await repository.Add(new RuleName { Name = "Regle1" });

        RuleName? found = await repository.FindById(r.Id);

        Assert.NotNull(found);
        Assert.Equal("Regle1", found!.Name);
    }

    [Fact]
    public async Task FindById_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        RuleNameRepository repository = new RuleNameRepository(context);

        Assert.Null(await repository.FindById(999));
    }

    [Fact]
    public async Task FindAll_RenvoieTout()
    {
        using LocalDbContext context = CreateContext();
        RuleNameRepository repository = new RuleNameRepository(context);
        await repository.Add(new RuleName { Name = "R1" });
        await repository.Add(new RuleName { Name = "R2" });

        List<RuleName> all = await repository.FindAll();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task Update_Modifie()
    {
        using LocalDbContext context = CreateContext();
        RuleNameRepository repository = new RuleNameRepository(context);
        RuleName r = await repository.Add(new RuleName { Name = "Ancien" });
        r.Name = "Nouveau";

        await repository.Update(r);

        RuleName? updated = await repository.FindById(r.Id);
        Assert.Equal("Nouveau", updated!.Name);
    }

    [Fact]
    public async Task Delete_Supprime_EtRenvoieLObjet()
    {
        using LocalDbContext context = CreateContext();
        RuleNameRepository repository = new RuleNameRepository(context);
        RuleName r = await repository.Add(new RuleName { Name = "R" });

        RuleName? deleted = await repository.Delete(r.Id);

        Assert.NotNull(deleted);
        Assert.Equal(0, context.RuleNames.Count());
    }

    [Fact]
    public async Task Delete_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        RuleNameRepository repository = new RuleNameRepository(context);

        Assert.Null(await repository.Delete(999));
    }

    [Fact]
    public async Task Exists_Vrai_SiPresent_Faux_Sinon()
    {
        using LocalDbContext context = CreateContext();
        RuleNameRepository repository = new RuleNameRepository(context);
        RuleName r = await repository.Add(new RuleName { Name = "R" });

        Assert.True(await repository.Exists(r.Id));
        Assert.False(await repository.Exists(999));
    }
}