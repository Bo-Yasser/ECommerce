using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public string PictureUrl { get; private set; } = null!;
    public decimal Price { get; private set; }

    // ProductBrand One Brand include Many Products 1:M
    public Guid ProductBrandId { get; private set; }
    public ProductBrand ProductBrand { get; private set; } = null!;

    // ProductType One Type include Many Products 1:M
    public Guid ProductTypeId { get; private set; }
    public ProductType ProductType { get; private set; } = null!;


    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 1000;
    public const int MaxPictureUrlLength = 500;
    private Product() { } // important for EF Core

    public static Result<Product> Create(
        Guid id,
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid productBrandId,
        Guid productTypeId)
    {
        if (id == Guid.Empty)
            return Result<Product>.Failure(ProductErrors.InvalidId);
        var product = new Product() { Id = id };

        var nameResult = product.SetName(name);
        if (nameResult.IsFailure)
            return Result<Product>.Failure(nameResult.Error!);

        var descriptionResult = product.SetDescription(description);
        if (descriptionResult.IsFailure)
            return Result<Product>.Failure(descriptionResult.Error!);

        var pictureUrlResult = product.SetPictureUrl(pictureUrl);
        if (pictureUrlResult.IsFailure)
            return Result<Product>.Failure(pictureUrlResult.Error!);

        var priceResult = product.SetPrice(price);
        if (priceResult.IsFailure)
            return Result<Product>.Failure(priceResult.Error!);

        var brandResult = product.SetBrand(productBrandId);
        if (brandResult.IsFailure)
            return Result<Product>.Failure(brandResult.Error!);

        var typeResult = product.SetType(productTypeId);
        if (typeResult.IsFailure)
            return Result<Product>.Failure(typeResult.Error!);

        return Result<Product>.Success(product);

    }
    public Result Update(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid productBrandId,
        Guid productTypeId)
    {

        var nameResult = SetName(name);
        if (nameResult.IsFailure)
            return nameResult;

        var descriptionResult = SetDescription(description);
        if (descriptionResult.IsFailure)
            return descriptionResult;

        var pictureUrlResult = SetPictureUrl(pictureUrl);
        if (pictureUrlResult.IsFailure)
            return pictureUrlResult;

        var priceResult = SetPrice(price);
        if (priceResult.IsFailure)
            return priceResult;

        var brandResult = SetBrand(productBrandId);
        if (brandResult.IsFailure)
            return brandResult;

        var typeResult = SetType(productTypeId);
        if (typeResult.IsFailure)
            return typeResult;

        return Result.Success();

    } 

    private Result SetName( string name )
    {
        if (string.IsNullOrWhiteSpace(name)) return Result.Failure(ProductErrors.NameRequired);
        if (name.Length > MaxNameLength) return Result.Failure(ProductErrors.NameLengthExceeded);

        Name = name.Trim();
        return Result.Success();
    }
    private Result SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description)) return Result.Failure(ProductErrors.DescriptionRequired);
        if (description.Length > MaxDescriptionLength) return Result.Failure(ProductErrors.DescriptionLengthExceeded);
        
        Description = description.Trim();
        return Result.Success();
    }
    private Result SetPictureUrl(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl)) return Result.Failure(ProductErrors.PictureUrlRequired);
        if (pictureUrl.Length > MaxPictureUrlLength) return Result.Failure(ProductErrors.PictureUrlLengthExceeded);

        PictureUrl = pictureUrl.Trim();
        return Result.Success();
    }
    private Result SetPrice(decimal price)
    {
        if (price < 0) return Result.Failure(ProductErrors.NegativePrice);
        Price = price;
        return Result.Success();
    }
    private Result SetBrand(Guid productBrandId)
    {
        if (productBrandId == Guid.Empty) return Result.Failure(ProductErrors.ProductBrandRequired);
        ProductBrandId = productBrandId;
        return Result.Success();
    }
    private Result SetType(Guid productTypeId)
    {
        if (productTypeId == Guid.Empty) return Result.Failure(ProductErrors.ProductTypeRequired);

        ProductTypeId = productTypeId;
        return Result.Success();
    }

}
