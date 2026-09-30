using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Errors;

public static class ProductErrors
{

    public static readonly Error NotFound =
        Error.NotFound("Product.NotFound", "Product Not Found");

    public static readonly Error InvalidId =
        Error.Validation("Product.InvalidId", "Product Id is invalid");

    public static readonly Error NameRequired = 
        Error.Validation("Product.NameRequired", "Product name is required");
    
    public static readonly Error DescriptionRequired = 
        Error.Validation("Product.DescriptionRequired", "Product Description is required");
    
    public static readonly Error PictureUrlRequired = 
        Error.Validation("Product.PictureUrlRequired", "Product Picture Url is required");
    
    public static readonly Error ProductBrandRequired =
        Error.Validation("Product.ProductBrandRequired", "Product Brand is required");

    public static readonly Error ProductTypeRequired =
        Error.Validation("Product.ProductTypeRequired", "Product Type is required");

    public static readonly Error SkuRequired =
        Error.Validation("Product.SkuRequired", "Product SKU is required");

    public static readonly Error ProductBrandNotFound =
        Error.NotFound("Product.ProductBrandNotFound", "Product Brand not found");

    public static readonly Error ProductTypeNotFound =
        Error.NotFound("Product.ProductTypeNotFound", "Product Type not found");

    public static readonly Error NameLengthExceeded =
        Error.Validation("Product.NameLengthExceeded", $"Product Name Cannot Exceed {Product.MaxNameLength} characters.");

    public static readonly Error DescriptionLengthExceeded =
        Error.Validation("Product.DescriptionLengthExceeded", $"Product Description Cannot Exceed {Product.MaxDescriptionLength} characters.");

    public static readonly Error PictureUrlLengthExceeded =
        Error.Validation("Product.PictureUrlLengthExceeded", $"Product Picture Url Cannot Exceed {Product.MaxPictureUrlLength} characters.");

    public static readonly Error SkuLengthExceeded =
        Error.Validation("Product.SkuLengthExceeded", $"Product SKU Cannot Exceed {Product.MaxSkuLength} characters.");

    public static readonly Error SkuInvalidFormat =
        Error.Validation("Product.SkuInvalidFormat", $"Invalid Product SKU format, it can only contain (Letters, Numbers, Dashes, Underscores).");

    public static readonly Error NegativePrice =
         Error.Validation("Product.NegativePrice", $"Product Price cannot be negative.");


    public static readonly Error AlreadyExists = 
        Error.Conflict("Product.AlreadyExists", "Product already exists");

    public static readonly Error NameAlreadyExists =
        Error.Conflict("Product.NameAlreadyExists", "Product name already exists");

    public static readonly Error CreateFailed = 
        Error.Failure("Product.CreateFailed", "Product could not be created");

    public static readonly Error UpdateFailed = 
        Error.Failure("Product.UpdateFailed", "Product could not be updated");

    public static readonly Error DeleteFailed =
        Error.Failure("Product.DeleteFailed", "Product could not be deleted");

    public static readonly Error ConcurrencyConflict =
        Error.Conflict("Product.Stock.ConcurrencyConflict", "Stock was updated by another process. Please refresh and try again.");
}
