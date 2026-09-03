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

public class CurveControllerTests
{
    private static LocalDbContext CreateContext()
    {
        DbContextOptions<LocalDbContext> options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new LocalDbContext(options);
    }

    private static CurveController CreateController(LocalDbContext context)
    {
        CurvePointRepository repository = new CurvePointRepository(context);
        CurveController controller = new CurveController(
            repository, NullLogger<CurveController>.Instance);
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
        CurveController controller = CreateController(context);

        IActionResult result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        CurvePoint created = await new CurvePointRepository(context).Add(new CurvePoint { CurvePointValue = 10.0 });
        CurveController controller = CreateController(context);

        IActionResult result = await controller.GetById(created.Id);

        OkObjectResult ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<CurvePoint>(ok.Value);
    }

    [Fact]
    public async Task Create_Renvoie201Created()
    {
        using LocalDbContext context = CreateContext();
        CurveController controller = CreateController(context);

        IActionResult result = await controller.Create(new CurvePoint { CurvePointValue = 5.0 });

        CreatedAtActionResult created = Assert.IsType<CreatedAtActionResult>(result);
        CurvePoint returned = Assert.IsType<CurvePoint>(created.Value);
        Assert.True(returned.Id > 0);
    }

    [Fact]
    public async Task Update_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        CurveController controller = CreateController(context);

        IActionResult result = await controller.Update(999, new CurvePoint { CurvePointValue = 1.0 });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieOk_SiExiste()
    {
        using LocalDbContext context = CreateContext();
        CurvePoint created = await new CurvePointRepository(context).Add(new CurvePoint { CurvePointValue = 10.0 });
        CurveController controller = CreateController(context);

        IActionResult result = await controller.Delete(created.Id);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Delete_RenvoieNotFound_SiInexistant()
    {
        using LocalDbContext context = CreateContext();
        CurveController controller = CreateController(context);

        IActionResult result = await controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
