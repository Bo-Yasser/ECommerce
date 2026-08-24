using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities;

public class ProductType : BaseEntity
{
    public string Name { get; private set; } = null!;

    // ProductType One Type include Many Products 1:M
    public ICollection<Product> Products { get; private set; } = [];

    public const int MaxNameLength = 100;
    private ProductType() { }
    public static Result<ProductType> Create(Guid id, string name)
    {
        if (id == Guid.Empty)
            return Result<ProductType>.Failure(TypeErrors.InvalidId);
        var type = new ProductType() { Id = id };
        var nameResult = type.SetName(name);
        if (nameResult.IsFailure)
            return Result<ProductType>.Failure(nameResult.Error!);

        return Result<ProductType>.Success(type);
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
        if (string.IsNullOrWhiteSpace(name)) return Result.Failure(TypeErrors.NameRequired);
        if (name.Length > MaxNameLength) return Result.Failure(TypeErrors.NameLengthExceeded);

        Name = name.Trim();
        return Result.Success();
    }
}