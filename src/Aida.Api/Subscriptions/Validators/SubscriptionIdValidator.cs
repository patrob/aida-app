using FluentValidation;

namespace Aida.Api.Subscriptions.Validators;

public class SubscriptionIdValidator : AbstractValidator<string>
{
    public SubscriptionIdValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Subscription ID cannot be empty")
            .Must(id => id != null && id.StartsWith("sub_") && id.Length >= 10)
            .WithMessage("Subscription ID must start with 'sub_' and be at least 10 characters long");
    }
} 