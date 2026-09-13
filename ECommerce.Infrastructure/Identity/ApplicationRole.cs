using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public ApplicationRole() => Id = Guid.NewGuid();
    public ApplicationRole(string name)
        : this() => Name = name;
}
