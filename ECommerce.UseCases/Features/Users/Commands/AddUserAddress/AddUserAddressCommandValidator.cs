using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using FluentValidation;

namespace ECommerce.UseCases.Features.Users.Commands.AddUserAddress;

public sealed class AddUserAddressCommandValidator : AbstractValidator<AddUserAddressCommand>
{
    public AddUserAddressCommandValidator()
    {
        RuleFor(x => x.RecipientFirstName)
            .NotEmpty()
            .WithMessage("Recipient first name is required.")
            .MaximumLength(UserAddress.MaxNameLength)
            .WithMessage($"Recipient first name must not exceed {UserAddress.MaxNameLength} characters.");
        
        RuleFor(x => x.RecipientLastName)
            .NotEmpty()
            .WithMessage("Recipient last name is required.")
            .MaximumLength(UserAddress.MaxNameLength)
            .WithMessage($"Recipient last name must not exceed {UserAddress.MaxNameLength} characters.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .MaximumLength(UserAddress.MaxPhoneLength)
            .WithMessage($"Phone number must not exceed {UserAddress.MaxPhoneLength} characters.")
            .Matches(@"^\+?[0-9]+$")
            .WithMessage("Phone number must contain only digits, with an optional '+' at the beginning.");
        
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .MaximumLength(UserAddress.MaxCountryLength)
            .WithMessage($"Country must not exceed {UserAddress.MaxCountryLength} characters.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(UserAddress.MaxCityLength)
            .WithMessage($"City must not exceed {UserAddress.MaxCityLength} characters.");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.")
            .MaximumLength(UserAddress.MaxStreetLength)
            .WithMessage($"Street must not exceed {UserAddress.MaxStreetLength} characters.");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("Postal Code is required.")
            .MaximumLength(UserAddress.MaxPostalCodeLength)
            .WithMessage($"Postal Code must not exceed {UserAddress.MaxPostalCodeLength} characters.");
    }
}
