using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Claims;

namespace P7CreateRestApi.Tests;

public class RatingControllerTests
{
    private static LocalDbContext CreateContext()
    {
        DbContextOptions<LocalDbContext> options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LocalDbContext(options);
    }

    private static RatingController CreateController(LocalDbContext context)
    {
        RatingRepository repository = new RatingRepository(context);
        RatingController controller = new RatingController(
            repository, NullLogger<RatingController>.Instance);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                    new[] { new Claim("sub", "testeur") }, "test"))
            }
        };
        return controller;
    }

    [Fact]
    public async Task GetById_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        RatingController controller = CreateController(context);

        IActionResult result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        Rating created = await new RatingRepository(context).Add(new Rating { MoodysRating = "Aaa" });
        RatingController controller = CreateController(context);

        IActionResult result = await controller.GetById(created.Id);

        OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<Rating>(ok.Value);
    }

    [Fact]
    public async Task Create_Renvoie201Created()
    {
        using LocalDbContext context = CreateContext();
        RatingController controller = CreateController(context);

        IActionResult result = await controller.Create(new Rating { MoodysRating = "Aaa" });

        CreatedAtActionResult created = Assert.IsType<CreatedAtActionResult>(result);
        Rating returned = Assert.IsType<Rating>(created.Value);
        Assert.True(returned.Id > 0);
    }

    [Fact]
    public async Task Update_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        RatingController controller = CreateController(context);

        IActionResult result = await controller.Update(999, new Rating { MoodysRating = "Aaa" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        Rating created = await new RatingRepository(context).Add(new Rating { MoodysRating = "Aaa" });
        RatingController controller = CreateController(context);

        IActionResult result = await controller.Delete(created.Id);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        RatingController controller = CreateController(context);

        IActionResult result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}