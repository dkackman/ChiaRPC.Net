﻿using ChiaRPC.Models;
using ChiaRPC.Results;
using ChiaRPC.Routes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ChiaRPC.Clients
{
    public abstract class ChiaRPCClient
    {
        private readonly HttpClient Client;
        private readonly Uri ApiUrl;

        internal ChiaRPCClient(string certDir, string certName, string apiUrl)
        {
            ApiUrl = new Uri(apiUrl);
            var certificate = LoadCertificate(certDir, certName);
            Client = CreateClient(certificate);
        }

        private static X509Certificate2 LoadCertificate(string certDir, string certName)
        {
            if (!Directory.Exists(certDir))
            {
                throw new DirectoryNotFoundException($"Could not find certficiate directory \"{certDir}\"");
            }

            string certificatePath = Path.Combine(certDir, certName, $"private_{certName}.crt");
            string keyPath = Path.Combine(certDir, certName, $"private_{certName}.key");

            if (!File.Exists(certificatePath))
            {
                throw new FileNotFoundException($"Could not find certificate file \"{certificatePath}\"");
            }
            if (!File.Exists(keyPath))
            {
                throw new FileNotFoundException($"Could not find certificate key file \"{keyPath}\"");
            }

            var certificate = X509Certificate2.CreateFromPemFile(certificatePath, keyPath);
            return certificate;
        }

        private static HttpClient CreateClient(X509Certificate2 certificate)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };

            handler.ClientCertificates.Add(certificate);
            return new HttpClient(handler);
        }

        /// <summary>
        /// Returns a list of peers that we are currently connected to.
        /// </summary>
        /// <returns></returns>
        public async Task<ChiaConnection[]> GetConnections()
        {
            var result = await PostAsync<GetConnectionsResult>(SharedRoutes.GetConnections());
            return result.Connections;
        }

        internal async Task<T> PostAsync<T>(Uri requestUri, Dictionary<string, string> parameters = null) where T : ChiaResult
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(ApiUrl, requestUri))
            {
                Content = JsonContent.Create(parameters ?? new Dictionary<string, string>())
            };

            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<T>();

            return !result.Success
                ? throw new HttpRequestException("Chia responded with unsuccessful")
                : result;
        }

        protected Task PostAsync(Uri requestUri, Dictionary<string, string> parameters = null)
            => PostAsync<ChiaResult>(requestUri, parameters);
    }
}