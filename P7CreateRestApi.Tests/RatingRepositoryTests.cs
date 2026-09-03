using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Tests;

public class RatingRepositoryTests
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
        RatingRepository repository = new RatingRepository(context);

        Rating created = await repository.Add(new Rating { MoodysRating = "Aaa" });

        Assert.True(created.Id > 0);
        Assert.Equal(1, context.Ratings.Count());
    }

    [Fact]
    public async Task FindById_Renvoie_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        RatingRepository repository = new RatingRepository(context);
        Rating r = await repository.Add(new Rating { MoodysRating = "Aaa" });

        Rating? found = await repository.FindById(r.Id);

        Assert.NotNull(found);
        Assert.Equal("Aaa", found!.MoodysRating);
    }

    [Fact]
    public async Task FindById_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        RatingRepository repository = new RatingRepository(context);

        Assert.Null(await repository.FindById(999));
    }

    [Fact]
    public async Task FindAll_RenvoieTout()
    {
        using LocalDbContext context = CreateContext();
        RatingRepository repository = new RatingRepository(context);
        await repository.Add(new Rating { MoodysRating = "Aaa" });
        await repository.Add(new Rating { MoodysRating = "Baa" });

        List<Rating> all = await repository.FindAll();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task Update_Modifie()
    {
        using LocalDbContext context = CreateContext();
        RatingRepository repository = new RatingRepository(context);
        Rating r = await repository.Add(new Rating { MoodysRating = "Aaa" });
        r.MoodysRating = "Caa";

        await repository.Update(r);

        Rating? updated = await repository.FindById(r.Id);
        Assert.Equal("Caa", updated!.MoodysRating);
    }

    [Fact]
    public async Task Delete_Supprime_EtRenvoieLObjet()
    {
        using LocalDbContext context = CreateContext();
        RatingRepository repository = new RatingRepository(context);
        Rating r = await repository.Add(new Rating { MoodysRating = "Aaa" });

        Rating? deleted = await repository.Delete(r.Id);

        Assert.NotNull(deleted);
        Assert.Equal(0, context.Ratings.Count());
    }

    [Fact]
    public async Task Delete_RenvoieNull_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        RatingRepository repository = new RatingRepository(context);

        Assert.Null(await repository.Delete(999));
    }

    [Fact]
    public async Task Exists_Vrai_SiPresent_Faux_Sinon()
    {
        using LocalDbContext context = CreateContext();
        RatingRepository repository = new RatingRepository(context);
        Rating r = await repository.Add(new Rating { MoodysRating = "Aaa" });

        Assert.True(await repository.Exists(r.Id));
        Assert.False(await repository.Exists(999));
    }
}