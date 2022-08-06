namespace ViewModels.Stripe;

public class StripeSettings
{
    public string PublishableKey { get; set; }

    public string SecretKey { get; set; }

    public string AccountSecret { get; set; }

    public string SigningSecret { get; set; }

    public string Price { get; set; }
}