using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models;

public class RegisterModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? Fullname { get; set; }
}