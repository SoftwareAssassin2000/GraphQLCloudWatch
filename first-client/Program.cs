using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FirstClient
{
    class Program
    {
        static void Main(string[] args)
        {
            getAccessToken();
            Console.ReadLine();
        }
        static async void getAccessToken()
        {
            var tokenUrl = "https://dev-583903.oktapreview.com/oauth2/default/v1/token";

            var client = new HttpClient();
            var clientId = "0oakh6fav16ku9KTg0h7";
            var clientSecret = "Db_kSwDOvc_wiBVmzETk5hCiif9bDihLveXQnusv";
            var clientCreds = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientCreds));

            var postMessage = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"scope", "access_token"}
            };

            var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var newToken = JsonConvert.DeserializeObject<OktaToken>(json);
                newToken.ExpiresAt = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn);

                Console.WriteLine(string.Format("Bearer {0}", newToken.AccessToken));
                return;
            }

            throw new ApplicationException("Unable to retrieve access token from Okta");
        }
    }
    public class OktaToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Scope { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        public bool IsValidAndNotExpiring
        {
            get
            {
                return !String.IsNullOrEmpty(this.AccessToken) &&
          this.ExpiresAt > DateTime.UtcNow.AddSeconds(30);
            }
        }
    }
}
