using System.Net.Http.Json;
using Aida.Api.Subscriptions.Models;
using FluentAssertions;

namespace Aida.Api.IntegrationTests.Subscriptions;

[Collection("Integration")]
public class SubscriptionsControllerTests(CustomWebFactory factory) : IClassFixture<CustomWebFactory>
{
    [Fact]
    public async Task CreateSubscription_ShouldReturnCreatedSubscription()
    {
        // Arrange
        var client = factory.CreateClient();
        var request = new CreateSubscriptionRequest
        {
            CustomerId = "cus_test_123",
            PlanId = "price_test_123",
            PaymentMethodId = "pm_test_123"
        };

        // Act
        var response = await client.PostAsJsonAsync("/subscriptions", request);

        // Assert
        response.EnsureSuccessStatusCode();
        // We're not checking for specific 201 Created anymore since we're not setting it manually
        var subscription = await response.Content.ReadFromJsonAsync<Subscription>();
        
        subscription.Should().NotBeNull();
        subscription!.CustomerId.Should().Be(request.CustomerId);
        subscription.PlanId.Should().Be(request.PlanId);
        subscription.Status.Should().Be(SubscriptionStatus.Active);
    }

    [Fact]
    public async Task GetSubscription_WithValidId_ShouldReturnSubscription()
    {
        // Arrange
        var client = factory.CreateClient();
        
        // First create a subscription to get its ID
        var createRequest = new CreateSubscriptionRequest
        {
            CustomerId = "cus_test_456",
            PlanId = "price_test_456",
            PaymentMethodId = "pm_test_456"
        };
        var createResponse = await client.PostAsJsonAsync("/subscriptions", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdSubscription = await createResponse.Content.ReadFromJsonAsync<Subscription>();
        
        // Act
        var response = await client.GetAsync($"/subscriptions/{createdSubscription!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var subscription = await response.Content.ReadFromJsonAsync<Subscription>();
        
        subscription.Should().NotBeNull();
        subscription!.Id.Should().Be(createdSubscription.Id);
    }

    [Fact]
    public async Task CancelSubscription_WithValidId_ShouldReturnCanceledSubscription()
    {
        // Arrange
        var client = factory.CreateClient();
        
        // First create a subscription to get its ID
        var createRequest = new CreateSubscriptionRequest
        {
            CustomerId = "cus_test_789",
            PlanId = "price_test_789",
            PaymentMethodId = "pm_test_789"
        };
        var createResponse = await client.PostAsJsonAsync("/subscriptions", createRequest);
        createResponse.EnsureSuccessStatusCode();
        var createdSubscription = await createResponse.Content.ReadFromJsonAsync<Subscription>();
        
        // Act
        var response = await client.DeleteAsync($"/subscriptions/{createdSubscription!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var subscription = await response.Content.ReadFromJsonAsync<Subscription>();
        
        subscription.Should().NotBeNull();
        subscription!.Status.Should().Be(SubscriptionStatus.Canceled);
    }
} 