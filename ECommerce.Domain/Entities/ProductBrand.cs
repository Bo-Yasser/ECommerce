namespace ECommerce.Domain.Entities;

public class ProductBrand : BaseEntity
{
    public string Name { get; set; } = null!;

    // ProductType One Type include Many Products 1:M
    public ICollection<Product> Products { get; set; } = [];

    private ProductBrand() { }
    public static ProductBrand Create(Guid id, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        if (id == Guid.Empty)
            throw new ArgumentException("Brand Id Is Required", nameof(id));
        return new() { Id = id, Name = name.Trim()};
    }
}