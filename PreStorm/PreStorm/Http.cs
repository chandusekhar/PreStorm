﻿using System;
using System.Net;
using System.Text;

namespace PreStorm
{
    internal static class Http
    {
        public static string Get(string url, ICredentials credentials)
        {
            using (var c = new GZipWebClient(credentials))
                return c.DownloadString(url);
        }

        public static string Post(string url, string data, ICredentials credentials)
        {
            using (var c = new GZipWebClient(credentials))
                return c.UploadString(url, data);
        }

        private class GZipWebClient : WebClient
        {
            private readonly ICredentials _credentials;

            public GZipWebClient(ICredentials credentials)
            {
                _credentials = credentials;

                Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                Encoding = Encoding.UTF8;
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address) as HttpWebRequest;

                if (request == null)
                    return null;

                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.Credentials = _credentials;
                return request;
            }
        }
    }
}