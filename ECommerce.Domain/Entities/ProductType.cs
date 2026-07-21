namespace ECommerce.Domain.Entities;

public class ProductType : BaseEntity
{
    public string Name { get; set; } = null!;

    // ProductType One Type include Many Products 1:M
    public ICollection<Product> Products { get; set; } = [];

    private ProductType() { }
    public static ProductType Create(Guid id, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        if (id == Guid.Empty)
            throw new ArgumentException("Type Id Is Required", nameof(id));
        return new() { Id = id, Name = name.Trim() };
    }
}