using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using ResourceMonitor.Models;

namespace ResourceMonitor.Services
{
    public class HttpSenderPlugin: IMonitorPlugin
    {
        public string Name => "API Server";

        private readonly HttpClient _client;
        private readonly string _endpointUrl;

        public HttpSenderPlugin(string endpointUrl)
        {
            _client = new HttpClient();
            _endpointUrl = endpointUrl;
        }

        public async Task ExecuteAsync(SystemStat stats)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(_endpointUrl, stats);


                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[API Success] Data Pushed to {_endpointUrl}");
                }
                else
                {
                    Console.WriteLine($"[API Failed] Endpoint returned status code {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API Error] Failed to send metrics {ex.Message}");
            }
        }
    }
}