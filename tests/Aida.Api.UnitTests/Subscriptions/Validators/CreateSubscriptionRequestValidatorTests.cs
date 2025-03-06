using Aida.Api.Subscriptions.Models;
using Aida.Api.Subscriptions.Validators;
using Aida.Api.Testing;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Aida.Api.UnitTests.Subscriptions.Validators;

public class CreateSubscriptionRequestValidatorTests : BaseTest
{
    private readonly CreateSubscriptionRequestValidator _validator;

    public CreateSubscriptionRequestValidatorTests()
    {
        _validator = new CreateSubscriptionRequestValidator();
    }

    [Fact]
    public void Validator_WhenAllPropertiesAreValid_ShouldNotHaveErrors()
    {
        // Arrange
        var request = new CreateSubscriptionRequest
        {
            CustomerId = "cus_123456abcdef",
            PlanId = "price_123456abcdef",
            PaymentMethodId = "pm_123456abcdef"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null, "Customer ID cannot be empty")]
    [InlineData("", "Customer ID cannot be empty")]
    [InlineData("   ", "Customer ID cannot be empty")]
    [InlineData("cus", "Customer ID must start with 'cus_' and be at least 10 characters long")]
    [InlineData("invalid", "Customer ID must start with 'cus_' and be at least 10 characters long")]
    public void Validator_WhenCustomerIdIsInvalid_ShouldHaveError(string customerId, string expectedErrorMessage)
    {
        // Arrange
        var request = new CreateSubscriptionRequest
        {
            CustomerId = customerId,
            PlanId = "price_123456abcdef",
            PaymentMethodId = "pm_123456abcdef"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CustomerId)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [InlineData(null, "Plan ID cannot be empty")]
    [InlineData("", "Plan ID cannot be empty")]
    [InlineData("   ", "Plan ID cannot be empty")]
    [InlineData("price", "Plan ID must start with 'price_' and be at least 10 characters long")]
    [InlineData("invalid", "Plan ID must start with 'price_' and be at least 10 characters long")]
    public void Validator_WhenPlanIdIsInvalid_ShouldHaveError(string planId, string expectedErrorMessage)
    {
        // Arrange
        var request = new CreateSubscriptionRequest
        {
            CustomerId = "cus_123456abcdef",
            PlanId = planId,
            PaymentMethodId = "pm_123456abcdef"
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PlanId)
            .WithErrorMessage(expectedErrorMessage);
    }

    [Theory]
    [InlineData(null, "Payment Method ID cannot be empty")]
    [InlineData("", "Payment Method ID cannot be empty")]
    [InlineData("   ", "Payment Method ID cannot be empty")]
    [InlineData("pm", "Payment Method ID must start with 'pm_' and be at least 10 characters long")]
    [InlineData("invalid", "Payment Method ID must start with 'pm_' and be at least 10 characters long")]
    public void Validator_WhenPaymentMethodIdIsInvalid_ShouldHaveError(string paymentMethodId, string expectedErrorMessage)
    {
        // Arrange
        var request = new CreateSubscriptionRequest
        {
            CustomerId = "cus_123456abcdef",
            PlanId = "price_123456abcdef",
            PaymentMethodId = paymentMethodId
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PaymentMethodId)
            .WithErrorMessage(expectedErrorMessage);
    }
} 