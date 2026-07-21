namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 1000;
    public const int MaxPictureUrlLength = 500;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public string PictureUrl { get; private set; } = null!;
    public decimal Price { get; private set; }

    // ProductBrand One Brand include Many Products 1:M
    public Guid ProductBrandId { get; set; }
    public ProductBrand ProductBrand { get; private set; } = null!;

    // ProductType One Type include Many Products 1:M
    public Guid ProductTypeId { get; set; }
    public ProductType ProductType { get; private set; } = null!;

    private Product() { }
    private Product(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid productBrandId,
        Guid productTypeId)
    {
        Id = Guid.NewGuid();

        SetName(name);
        SetDescription(description);  
        SetPictureUrl(pictureUrl);
        SetPrice(price);
        SetBrand(productBrandId);
        SetType(productTypeId);
    }

    public static Product Create(
    string name,
    string description,
    string pictureUrl,
    decimal price,
    Guid productBrandId,
    Guid productTypeId)
    {
        return new(
            name,
            description,
            pictureUrl,
            price,
            productBrandId,
            productTypeId
        );
    }
    private void SetName( string name )
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Product Name Is Required.");
        }
        if (name.Length > MaxNameLength)
        {
            throw new InvalidOperationException($"Product Name Cannot Exceed {MaxNameLength} characters.");
        }
        Name = name.Trim();
    }
    private void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidOperationException("Product Description Is Required.");
        }
        if (description.Length > MaxDescriptionLength)
        {
            throw new InvalidOperationException($"Product Description Cannot Exceed {MaxDescriptionLength} characters.");
        }
        Description = description.Trim();
    }
    private void SetPictureUrl(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
        {
            throw new InvalidOperationException("Product Picture Url Is Required.");
        }
        if (pictureUrl.Length > MaxPictureUrlLength)
        {
            throw new InvalidOperationException($"Product Picture Url Cannot Exceed {MaxPictureUrlLength} characters.");
        }
        PictureUrl = pictureUrl.Trim();
    }
    private void SetPrice(decimal price)
    {
        if(price < 0)
        {
            throw new InvalidOperationException($"Product Price cannot be negative.");
        }
        Price = price;
    }
    private void SetBrand(Guid productBrandId)
    {
        if (productBrandId == Guid.Empty)
        {
            throw new InvalidOperationException($"Product Brand Is Required.");
        }
        ProductBrandId = productBrandId;
    }
    private void SetType(Guid productTypeId)
    {
        if (productTypeId == Guid.Empty)
        {
            throw new InvalidOperationException($"Product Type Is Required.");
        }
        ProductTypeId = productTypeId;
    }

}
