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

public class TradeControllerTests
{
    private static LocalDbContext CreateContext()
    {
        DbContextOptions<LocalDbContext> options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LocalDbContext(options);
    }

    private static TradeController CreateController(LocalDbContext context)
    {
        TradeRepository repository = new TradeRepository(context);
        TradeController controller = new TradeController(
            repository, NullLogger<TradeController>.Instance);
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
        TradeController controller = CreateController(context);

        IActionResult result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        Trade created = await new TradeRepository(context).Add(new Trade { Account = "A" });
        TradeController controller = CreateController(context);

        IActionResult result = await controller.GetById(created.TradeId);

        OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<Trade>(ok.Value);
    }

    [Fact]
    public async Task Create_Renvoie201Created()
    {
        using LocalDbContext context = CreateContext();
        TradeController controller = CreateController(context);

        IActionResult result = await controller.Create(new Trade { Account = "A" });

        CreatedAtActionResult created = Assert.IsType<CreatedAtActionResult>(result);
        Trade returned = Assert.IsType<Trade>(created.Value);
        Assert.True(returned.TradeId > 0);
    }

    [Fact]
    public async Task Update_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        TradeController controller = CreateController(context);

        IActionResult result = await controller.Update(999, new Trade { Account = "A" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        Trade created = await new TradeRepository(context).Add(new Trade { Account = "A" });
        TradeController controller = CreateController(context);

        IActionResult result = await controller.Delete(created.TradeId);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        TradeController controller = CreateController(context);

        IActionResult result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}