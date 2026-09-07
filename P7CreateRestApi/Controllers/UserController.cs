using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        User user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            Fullname = model.Fullname
        };

        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        const string defaultRole = "User";
        if (!await _roleManager.RoleExistsAsync(defaultRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(defaultRole));
        }
        await _userManager.AddToRoleAsync(user, defaultRole);

        return Ok(new { user.Id, user.UserName, user.Email, user.Fullname, Role = defaultRole });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<User> users = await _userManager.Users.ToListAsync();
        List<UserDto> dtos = new List<UserDto>();
        foreach (User u in users)
        {
            IList<string> roles = await _userManager.GetRolesAsync(u);
            dtos.Add(new UserDto
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                Fullname = u.Fullname,
                Roles = roles
            });
        }
        return Ok(dtos);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        IList<string> roles = await _userManager.GetRolesAsync(user);
        UserDto dto = new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Fullname = user.Fullname,
            Roles = roles
        };
        return Ok(dto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserModel model)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        user.Email = model.Email;
        user.Fullname = model.Fullname;

        IdentityResult result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { user.Id, user.UserName, user.Email, user.Fullname });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        await _userManager.DeleteAsync(user);
        return NoContent();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
    {
        User? user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return Unauthorized();
        }
        IdentityResult result = await _userManager.ChangePasswordAsync(
            user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok(new { message = "Mot de passe modifie." });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/role/{role}")]
    public async Task<IActionResult> AssignRole(string id, string role)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }
        if (!await _roleManager.RoleExistsAsync(role))
        {
            return BadRequest($"Le role '{role}' n'existe pas.");
        }
        await _userManager.AddToRoleAsync(user, role);
        return Ok(new { user.Id, user.UserName, Role = role });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}/role/{role}")]
    public async Task<IActionResult> RemoveRole(string id, string role)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        if (role == "Admin" && await _userManager.IsInRoleAsync(user, "Admin"))
        {
            IList<User> admins = await _userManager.GetUsersInRoleAsync("Admin");
            if (admins.Count <= 1)
            {
                return BadRequest(
                    "Impossible de retirer le role Admin : il doit rester au moins un administrateur.");
            }
        }

        await _userManager.RemoveFromRoleAsync(user, role);
        return Ok(new { user.Id, user.UserName, RemovedRole = role });
    }

}