namespace ECommerce.Domain.Common.Errors;

public static class ProductErrors
{
    public const int MaxNameLength = 100;
    public const int MaxDescriptionLength = 1000;
    public const int MaxPictureUrlLength = 500;

    public static readonly Error NotFound =
        Error.NotFound("Product.NotFound", "Product Not Found");

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

    public static readonly Error NameLengthExceeded =
        Error.Validation("Product.NameLengthExceeded", $"Product Name Cannot Exceed {MaxNameLength} characters.");

    public static readonly Error DescriptionLengthExceeded =
        Error.Validation("Product.DescriptionLengthExceeded", $"Product Description Cannot Exceed {MaxDescriptionLength} characters.");

    public static readonly Error PictureUrlLengthExceeded =
        Error.Validation("Product.PictureUrlLengthExceeded", $"Product Picture Url Cannot Exceed {MaxPictureUrlLength} characters.");

    public static readonly Error NegativePrice =
         Error.Validation("Product.NegativePrice", $"Product Price cannot be negative.");


    public static readonly Error AlreadyExists = 
        Error.Conflict("Product.AlreadyExists", "Product already exists");


    public static readonly Error CreateFailed = 
        Error.Failure("Product.CreateFailed", "Product could not be created");

    public static readonly Error UpdateFailed = 
        Error.Failure("Product.UpdateFailed", "Product could not be updated");

    public static readonly Error DeleteFailed =
        Error.Failure("Product.DeleteFailed", "Product could not be deleted");
}
