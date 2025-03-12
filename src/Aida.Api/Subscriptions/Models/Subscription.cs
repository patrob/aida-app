using System;

namespace Aida.Api.Subscriptions.Models;

public class Subscription
{
    public required string Id { get; set; }
    public required string CustomerId { get; set; }
    public required string PlanId { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
}

public enum SubscriptionStatus
{
    Active,
    PastDue,
    Canceled,
    Unpaid
} 