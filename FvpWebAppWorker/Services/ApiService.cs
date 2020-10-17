using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FvpWebAppWorker.Models;
using Newtonsoft.Json;

namespace FvpWebAppWorker.Services
{
    public class ApiService
    {
        private static readonly string apiUrl = "https://api.ajk-software.pl/";
        private static readonly ApiUserKey apiUserKey = new ApiUserKey { UserKey = "8317EA7B-524B-4981-BADC-EA8654851FB8" };

        public async Task<ApiToken> ApiLogin()
        {
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                var userKeyJson = JsonConvert.SerializeObject(apiUserKey);
                var httpContent = new StringContent(userKeyJson, Encoding.UTF8, "application/json");
                var stringTask = client.PostAsync($"{apiUrl}login", httpContent);
                var response = await stringTask;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeObject<ApiToken>(content);
                    return token;
                }
                return null;
            }
        }

        public async Task<List<GusContractor>> GetGusDataAsync(string vatId, ApiToken apiToken)
        {
            var client = new HttpClient() { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", apiToken.Token) } };
            var vatNumberJson = JsonConvert.SerializeObject(new NipRequest { Nip = vatId });
            var httpContent = new StringContent(vatNumberJson, Encoding.UTF8, "application/json");
            var stringTask = client.PostAsync($"{apiUrl}gusapi/data", httpContent);
            var response = await stringTask;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<GusContractor>>(await response.Content.ReadAsStringAsync());
                }
                catch
                {
                    return new List<GusContractor>();
                }
            }
            return new List<GusContractor>();
        }

        public async Task<ViesContractorResponse> GetViesDataAsync(ViesSimpleRequest viesRequest, ApiToken apiToken)
        {
            var client = new HttpClient() { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", apiToken.Token) } };
            var vatNumberJson = JsonConvert.SerializeObject(viesRequest);
            var httpContent = new StringContent(vatNumberJson, Encoding.UTF8, "application/json");
            var stringTask = client.PostAsync($"{apiUrl}ViesApi/simpleCheck", httpContent);
            var response = await stringTask;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resp = await response.Content.ReadAsByteArrayAsync();
                var encodedText = Encoding.UTF8.GetString(resp, 0, resp.Length);
                return JsonConvert.DeserializeObject<ViesContractorResponse>(encodedText);
            }
            client.Dispose();
            return null;
        }

    }
}