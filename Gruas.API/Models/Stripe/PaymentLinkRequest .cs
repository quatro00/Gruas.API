﻿namespace Gruas.API.Models.Stripe
{
    public class PaymentLinkRequest
    {
        public long Amount { get; set; } // Monto en centavos
        public string Currency { get; set; }
    }
}
