using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities;

public class ProductBrand : BaseEntity
{
    public string Name { get; private set; } = null!;
    // ProductType One Type include Many Products 1:M
    public ICollection<Product> Products { get; private set; } = [];


    public const int MaxNameLength = 100;
    private ProductBrand() { }
    public static Result<ProductBrand> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            return Result<ProductBrand>.Failure(BrandErrors.InvalidId);

        var brand = new ProductBrand() { Id = id};
        var nameResult = brand.SetName(name);
        if (nameResult.IsFailure)
            return Result<ProductBrand>.Failure(nameResult.Error!);

        return Result<ProductBrand>.Success(brand);
    }

    public Result Update(string name)
    {
        var nameResult = SetName(name);
        if (nameResult.IsFailure)
            return Result.Failure(nameResult.Error!);
        return Result.Success();
    }

    private Result SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result.Failure(BrandErrors.NameRequired);
        if (name.Length > MaxNameLength) return Result.Failure(BrandErrors.NameLengthExceeded);

        Name = name.Trim();
        return Result.Success();
    }

}