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

public class BidListControllerTests
{
    private static LocalDbContext CreateContext()
    {
        DbContextOptions<LocalDbContext> options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LocalDbContext(options);
    }

    private static BidListController CreateController(LocalDbContext context)
    {
        BidListRepository repository = new BidListRepository(context);
        BidListController controller = new BidListController(
            repository, NullLogger<BidListController>.Instance);

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
        BidListController controller = CreateController(context);

        IActionResult result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        BidList created = await new BidListRepository(context).Add(new BidList { Account = "A" });
        BidListController controller = CreateController(context);

        IActionResult result = await controller.GetById(created.BidListId);

        OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
        BidList returned = Assert.IsType<BidList>(ok.Value);
        Assert.Equal("A", returned.Account);
    }

    [Fact]
    public async Task Create_Renvoie201Created()
    {
        using LocalDbContext context = CreateContext();
        BidListController controller = CreateController(context);

        IActionResult result = await controller.Create(new BidList { Account = "Nouveau" });

        CreatedAtActionResult created = Assert.IsType<CreatedAtActionResult>(result);
        BidList returned = Assert.IsType<BidList>(created.Value);
        Assert.True(returned.BidListId > 0);
    }

    [Fact]
    public async Task Update_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        BidListController controller = CreateController(context);

        IActionResult result = await controller.Update(999, new BidList { Account = "X" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        BidList created = await new BidListRepository(context).Add(new BidList { Account = "A" });
        BidListController controller = CreateController(context);

        IActionResult result = await controller.Delete(created.BidListId);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        BidListController controller = CreateController(context);

        IActionResult result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}