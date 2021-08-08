using ChiaRPC.Models;
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
        private readonly ChiaRPCOptions Options;
        private readonly HttpClient Client;
        private readonly Uri ApiUrl;

        internal ChiaRPCClient(ChiaRPCOptions options, string certName, string apiUrl)
        {
            Options = options;
            ApiUrl = new Uri(apiUrl);
            var certificate = LoadCertificate(Options.SslCertificateDirectory, certName);
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

            return OperatingSystem.IsWindows()
                ? new X509Certificate2(certificate.Export(X509ContentType.Pkcs12)) //This is required due to a bug. Might be fixed in dotnet 6.0
                : certificate;
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

        /// <summary>
        /// Opens a connection to another peer.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public async Task OpenConnection(string host, ushort port)
        {
            await PostAsync(SharedRoutes.OpenConnection(), new Dictionary<string, string>()
            {
                ["host"] = host,
                ["port"] = $"{port}",
            });
        }

        /// <summary>
        /// Opens a connection to another peer.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public Task OpenConnection(Uri address)
            => OpenConnection(address.Host, (ushort)address.Port);

        /// <summary>
        /// Closes a connection with a peer.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task CloseConnection(HexBytes nodeId)
        {
            await PostAsync(SharedRoutes.CloseConnection(), new Dictionary<string, string>()
            {
                ["node_id"] = $"{nodeId}"
            });
        }

        protected async Task<T> PostAsyncRaw<T>(Uri requestUri, object parameters = null, bool throwOnError = true) where T : ChiaResult
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(ApiUrl, requestUri))
                {
                    Content = JsonContent.Create(parameters ?? new Dictionary<string, string>())
                };

                var response = await Client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<T>();

                if (throwOnError && !result.Success)
                {
                    throw new HttpRequestException("Chia responded with unsuccessful");
                }
                //
                return !result.Success
                    ? default
                    : result;
            }

            protected Task<T> PostAsync<T>(Uri requestUri, Dictionary<string, string> parameters = null, bool throwOnError = true) where T : ChiaResult
                => PostAsyncRaw<T>(requestUri, parameters, throwOnError);

            protected Task PostAsync(Uri requestUri, Dictionary<string, string> parameters = null)
                => PostAsync<ChiaResult>(requestUri, parameters);
        }
    }
