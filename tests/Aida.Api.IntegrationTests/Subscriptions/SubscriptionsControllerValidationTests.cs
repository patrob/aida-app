using System.Net;
using System.Net.Http.Json;
using Aida.Api.Subscriptions.Models;
using FluentAssertions;

namespace Aida.Api.IntegrationTests.Subscriptions;

[Collection("Integration")]
public class SubscriptionsControllerValidationTests(CustomWebFactory factory) : IClassFixture<CustomWebFactory>
{
    [Theory]
    [InlineData(null, "price_test123456", "pm_test123456", "Customer ID cannot be empty")]
    [InlineData("invalid", "price_test123456", "pm_test123456", "Customer ID must start with 'cus_' and be at least 10 characters long")]
    [InlineData("cus_test123456", null, "pm_test123456", "Plan ID cannot be empty")]
    [InlineData("cus_test123456", "invalid", "pm_test123456", "Plan ID must start with 'price_' and be at least 10 characters long")]
    [InlineData("cus_test123456", "price_test123456", null, "Payment Method ID cannot be empty")]
    [InlineData("cus_test123456", "price_test123456", "invalid", "Payment Method ID must start with 'pm_' and be at least 10 characters long")]
    public async Task CreateSubscription_WithInvalidRequest_ShouldReturnBadRequest(
        string customerId, string planId, string paymentMethodId, string expectedError)
    {
        // Arrange
        var client = factory.CreateClient();
        var request = new CreateSubscriptionRequest
        {
            CustomerId = customerId,
            PlanId = planId,
            PaymentMethodId = paymentMethodId
        };

        // Act
        var response = await client.PostAsJsonAsync("/subscriptions", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var errorContent = await response.Content.ReadAsStringAsync();
        errorContent.Should().Contain(expectedError);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("sub")]
    public async Task GetSubscription_WithInvalidId_ShouldReturnBadRequest(string invalidId)
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/subscriptions/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var errorContent = await response.Content.ReadAsStringAsync();
        errorContent.Should().Contain("Subscription ID must start with 'sub_' and be at least 10 characters long");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("sub")]
    public async Task CancelSubscription_WithInvalidId_ShouldReturnBadRequest(string invalidId)
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.DeleteAsync($"/subscriptions/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var errorContent = await response.Content.ReadAsStringAsync();
        errorContent.Should().Contain("Subscription ID must start with 'sub_' and be at least 10 characters long");
    }
} 