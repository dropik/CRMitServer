﻿namespace CRMitServer.Core
{
    public class PurchaseRequest
    {
        public Client SenderClient { get; set; }
        public PurchaseItem Item { get; set; }
    }
}
