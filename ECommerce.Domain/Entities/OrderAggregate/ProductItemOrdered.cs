using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using System.Text.RegularExpressions;

namespace ECommerce.Domain.Entities.OrderAggregate;
public sealed class ProductItemOrdered
{
    public const int MaxProductNameLength = 200;
    public const int MaxPictureUrlLength = 500;
    public const int MaxSkuLength = 50;

    private ProductItemOrdered() { }

    public string Sku { get; private set; } = null!;
    public string ProductName { get; private set; } = null!;
    public string PictureUrl { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }

    public static Result<ProductItemOrdered> Create(
        string sku,
        string productName,
        string pictureUrl,
        decimal unitPrice)
    {
        var item = new ProductItemOrdered();

        var skuResult = item.SetSku(sku);
        if (skuResult.IsFailure)
            return Result<ProductItemOrdered>.Failure(skuResult.Error!);

        var productNameResult = item.SetProductName(productName);
        if (productNameResult.IsFailure)
            return Result<ProductItemOrdered>.Failure(productNameResult.Error!);

        var pictureUrlResult = item.SetPictureUrl(pictureUrl);
        if (pictureUrlResult.IsFailure)
            return Result<ProductItemOrdered>.Failure(pictureUrlResult.Error!);

        var unitPricelResult = item.SetUnitPrice(unitPrice);
        if (unitPricelResult.IsFailure)
            return Result<ProductItemOrdered>.Failure(unitPricelResult.Error!);

        return Result<ProductItemOrdered>.Success(item);
    }
    private Result SetProductName(string productName)
    {
        var trimmed = productName.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return Result.Failure(OrderErrors.ProductItemNameRequired);

        if (trimmed.Length > MaxProductNameLength)
            return Result.Failure(OrderErrors.ProductItemNameLengthExceeded);

        ProductName = trimmed;
        return Result.Success();
    }

    private Result SetPictureUrl(string pictureUrl)
    {
        var trimmed = pictureUrl.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return Result.Failure(OrderErrors.ProductItemPictureUrlRequired);

        if (trimmed.Length > MaxPictureUrlLength)
            return Result.Failure(OrderErrors.ProductItemPictureUrlLengthExceeded);

        PictureUrl = trimmed;
        return Result.Success();
    }

    private Result SetUnitPrice(decimal unitPrice)
    {
        if (unitPrice < 0)
            return Result.Failure(OrderErrors.ProductItemNegativePrice);

        UnitPrice = unitPrice;
        return Result.Success();
    }

    private Result SetSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            return Result.Failure(ProductErrors.SkuRequired);

        var normalizedSku = sku.Trim().ToUpperInvariant();

        if (normalizedSku.Length > MaxSkuLength)
            return Result.Failure(ProductErrors.SkuLengthExceeded);

        if (!Regex.IsMatch(normalizedSku, "^[A-Z0-9_-]+$"))
            return Result.Failure(ProductErrors.SkuInvalidFormat);

        Sku = normalizedSku;
        return Result.Success();
    }

}