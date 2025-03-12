using System.Threading.Tasks;
using Stripe;

namespace Aida.Api.Subscriptions;

public interface IStripePaymentMethodService
{
    Task<PaymentMethod> AttachAsync(string paymentMethodId, PaymentMethodAttachOptions options);
} 