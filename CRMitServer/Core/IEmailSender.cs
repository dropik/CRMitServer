﻿namespace CRMitServer.Core
{
    public interface IEmailSender
    {
        string Mailto { get; set; }
        string EmailBody { get; set; }
        string EmailObject { get; set; }

        void Send();
    }
}
