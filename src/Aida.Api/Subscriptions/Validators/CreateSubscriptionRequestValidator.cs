using Aida.Api.Subscriptions.Models;
using FluentValidation;

namespace Aida.Api.Subscriptions.Validators;

public class CreateSubscriptionRequestValidator : AbstractValidator<CreateSubscriptionRequest>
{
    public CreateSubscriptionRequestValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID cannot be empty")
            .Must(id => id != null && id.StartsWith("cus_") && id.Length >= 10)
            .WithMessage("Customer ID must start with 'cus_' and be at least 10 characters long");
            
        RuleFor(x => x.PlanId)
            .NotEmpty().WithMessage("Plan ID cannot be empty")
            .Must(id => id != null && id.StartsWith("price_") && id.Length >= 10)
            .WithMessage("Plan ID must start with 'price_' and be at least 10 characters long");
            
        RuleFor(x => x.PaymentMethodId)
            .NotEmpty().WithMessage("Payment Method ID cannot be empty")
            .Must(id => id != null && id.StartsWith("pm_") && id.Length >= 10)
            .WithMessage("Payment Method ID must start with 'pm_' and be at least 10 characters long");
    }
} 