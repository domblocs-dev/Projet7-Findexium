namespace P7CreateRestApi.Models;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Fullname { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
}