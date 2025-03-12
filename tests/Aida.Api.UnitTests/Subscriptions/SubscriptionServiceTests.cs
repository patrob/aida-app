using Aida.Api.Subscriptions;
using Aida.Api.Subscriptions.Models;
using Aida.Api.Testing;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Aida.Api.UnitTests.Subscriptions;

public class SubscriptionServiceTests : BaseTest
{
    private readonly Mock<IStripeAdapter> _mockStripeAdapter;
    private readonly Mock<ILogger<SubscriptionService>> _mockLogger;
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionServiceTests()
    {
        _mockStripeAdapter = new Mock<IStripeAdapter>();
        _mockLogger = new Mock<ILogger<SubscriptionService>>();
        _subscriptionService = new SubscriptionService(_mockStripeAdapter.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateSubscription_WithValidRequest_ShouldReturnSubscription()
    {
        // Arrange
        var request = new CreateSubscriptionRequest
        {
            CustomerId = "cus_123456",
            PlanId = "plan_123456",
            PaymentMethodId = "pm_123456"
        };

        var expectedSubscription = new Subscription
        {
            Id = "sub_123456",
            CustomerId = request.CustomerId,
            PlanId = request.PlanId,
            Status = SubscriptionStatus.Active,
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1)
        };

        _mockStripeAdapter
            .Setup(x => x.CreateSubscriptionAsync(request.CustomerId, request.PlanId, request.PaymentMethodId))
            .ReturnsAsync(expectedSubscription);

        // Act
        var result = await _subscriptionService.CreateSubscriptionAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedSubscription.Id);
        result.CustomerId.Should().Be(expectedSubscription.CustomerId);
        result.PlanId.Should().Be(expectedSubscription.PlanId);
        result.Status.Should().Be(expectedSubscription.Status);
        result.CurrentPeriodEnd.Should().Be(expectedSubscription.CurrentPeriodEnd);
    }

    [Fact]
    public async Task GetSubscription_WithValidId_ShouldReturnSubscription()
    {
        // Arrange
        var subscriptionId = "sub_123456";
        var expectedSubscription = new Subscription
        {
            Id = subscriptionId,
            CustomerId = "cus_123456",
            PlanId = "plan_123456",
            Status = SubscriptionStatus.Active,
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1)
        };

        _mockStripeAdapter
            .Setup(x => x.GetSubscriptionAsync(subscriptionId))
            .ReturnsAsync(expectedSubscription);

        // Act
        var result = await _subscriptionService.GetSubscriptionAsync(subscriptionId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedSubscription.Id);
        result.CustomerId.Should().Be(expectedSubscription.CustomerId);
        result.PlanId.Should().Be(expectedSubscription.PlanId);
        result.Status.Should().Be(expectedSubscription.Status);
        result.CurrentPeriodEnd.Should().Be(expectedSubscription.CurrentPeriodEnd);
    }

    [Fact]
    public async Task CancelSubscription_WithValidId_ShouldReturnCanceledSubscription()
    {
        // Arrange
        var subscriptionId = "sub_123456";
        var expectedSubscription = new Subscription
        {
            Id = subscriptionId,
            CustomerId = "cus_123456",
            PlanId = "plan_123456",
            Status = SubscriptionStatus.Canceled,
            CurrentPeriodEnd = DateTime.UtcNow.AddMonths(1)
        };

        _mockStripeAdapter
            .Setup(x => x.CancelSubscriptionAsync(subscriptionId))
            .ReturnsAsync(expectedSubscription);

        // Act
        var result = await _subscriptionService.CancelSubscriptionAsync(subscriptionId);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(SubscriptionStatus.Canceled);
    }
} 