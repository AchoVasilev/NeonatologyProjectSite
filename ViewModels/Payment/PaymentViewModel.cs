namespace ViewModels.Payment;

using System;

using Offer;

public class PaymentViewModel
{
    public DateTime Date { get; set; }

    public OfferViewModel Offer { get; set; }
}