using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace P7CreateRestApi.Tests;

public class UserControllerTests
{
    // Fabrique un UserManager<User> SIMULE (Moq). On lui passe un IUserStore factice
    // et huit null : c'est la signature du constructeur de UserManager<T>.
    // Ses methodes étant virtuelles, on pourra définir leur comportement par test.
    private static Mock<UserManager<User>> CreateUserManagerMock()
    {
        Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    private static UserController CreateController(UserManager<User> userManager)
    {
        // RoleManager n'est pas utilise par Delete : on passe null! (le constructeur
        // ne fait que l'assigner). Logger silencieux comme dans les autres tests.
        return new UserController(userManager, null!, NullLogger<UserController>.Instance);
    }

    [Fact]
    public async Task Delete_RenvoieBadRequest_SiDernierAdmin()
    {
        // Arrange : l'utilisateur cible est Admin et c'est le SEUL admin
        User admin = new User { Id = "1", UserName = "admin" };
        Mock<UserManager<User>> userManager = CreateUserManagerMock();
        userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(admin);
        userManager.Setup(m => m.IsInRoleAsync(admin, "Admin")).ReturnsAsync(true);
        userManager.Setup(m => m.GetUsersInRoleAsync("Admin"))
            .ReturnsAsync(new List<User> { admin });          // un seul admin
        UserController controller = CreateController(userManager.Object);

        // Act
        IActionResult result = await controller.Delete("1");

        // Assert : refus (400) ET la suppression ne doit PAS avoir eu lieu
        Assert.IsType<BadRequestObjectResult>(result);      // 400
        userManager.Verify(m => m.DeleteAsync(It.IsAny<User>()), Times.Never);
    }
}