using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models;

public class ResetPasswordModel
{
    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string NewPassword { get; set; } = string.Empty;
}