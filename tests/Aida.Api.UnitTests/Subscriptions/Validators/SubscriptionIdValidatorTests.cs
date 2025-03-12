using Aida.Api.Subscriptions.Validators;
using Aida.Api.Testing;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Aida.Api.UnitTests.Subscriptions.Validators;

public class SubscriptionIdValidatorTests : BaseTest
{
    private readonly SubscriptionIdValidator _validator;

    public SubscriptionIdValidatorTests()
    {
        _validator = new SubscriptionIdValidator();
    }

    [Fact]
    public void Validator_WhenSubscriptionIdIsValid_ShouldNotHaveErrors()
    {
        // Arrange
        var subscriptionId = "sub_123456abcdef";

        // Act
        var result = _validator.TestValidate(subscriptionId);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "Subscription ID cannot be empty")]
    [InlineData("   ", "Subscription ID cannot be empty")]
    [InlineData("sub", "Subscription ID must start with 'sub_' and be at least 10 characters long")]
    [InlineData("invalid", "Subscription ID must start with 'sub_' and be at least 10 characters long")]
    public void Validator_WhenSubscriptionIdIsInvalid_ShouldHaveError(string subscriptionId, string expectedErrorMessage)
    {
        // Arrange & Act
        var result = _validator.TestValidate(subscriptionId);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedErrorMessage);
    }
} 