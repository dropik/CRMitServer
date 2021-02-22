﻿namespace CRMitServer.Models
{
    public class EmailClientSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool UseSSL { get; set; }
    }
}
