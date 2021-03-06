﻿using System;
using System.Net.Http;

namespace TfsMigrator.Data
{
    public class RequestData : IDisposable
    {
        public string BaseAddress { get; set; }
        public HttpClient HttpClient { get; set; }

        public void Dispose()
        {
            if (HttpClient != null)
                HttpClient.Dispose();
        }
    }
}