using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameATron4000.Translator
{
    public class TranslatorClient
    {
        private const string HOST = "https://api.cognitive.microsofttranslator.com";

        private readonly string _subscriptionKey;

        public TranslatorClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<string> TranslateAsync(string text, string locale)
        {
            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            var jsonResponse = await SendRequestAsync(requestBody, "/translate?api-version=3.0&to=" + locale).ConfigureAwait(false);

            var responses = JsonConvert.DeserializeObject<TranslatorResponse[]>(jsonResponse);

            if (responses.Length > 0 && responses[0].Translations.Length > 0)
            {
                return responses[0].Translations[0].Text;
            }

            return text;
        }

        private async Task<string> SendRequestAsync(string requestBody, string route)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Set the method to POST
                request.Method = HttpMethod.Post;

                // Construct the full URI
                request.RequestUri = new Uri(HOST + route);

                // Add the serialized JSON object to your request
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                // Add the authorization header
                request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

                // Send request, get response
                var response = await client.SendAsync(request).ConfigureAwait(false);
                
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}