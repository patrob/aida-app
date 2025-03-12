using System.Threading.Tasks;
using Stripe;

namespace Aida.Api.Subscriptions;

public interface IStripeCustomerService
{
    Task<Customer> UpdateAsync(string customerId, CustomerUpdateOptions options);
} 