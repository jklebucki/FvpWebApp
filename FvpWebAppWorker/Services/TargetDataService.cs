using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Models;
using FvpWebAppWorker.Services.Interfaces;
using Newtonsoft.Json;

namespace FvpWebAppWorker.Services
{
    public class TargetDataService : ITargetDataService
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiUrl = "https://api.ajk-software.pl/";
        private static readonly ApiUserKey apiUserKey = new ApiUserKey { UserKey = "8317EA7B-524B-4981-BADC-EA8654851FB8" };
        public async Task<ApiResponseContractor> CheckContractorByGusApi(string vatId)
        {

            var apiToken = await ApiLogin();
            if (apiToken.Token == null)
                return new ApiResponseContractor
                {
                    ApiStatus = ApiStatus.Error,
                };
            var gusContractors = await GetGusDataAsync(vatId, apiToken);
            if (gusContractors.Count > 0)
                return new ApiResponseContractor
                {
                    ApiStatus = ApiStatus.Valid,
                    Contractors = GusContractorsToContractors(gusContractors),
                };
            return new ApiResponseContractor
            {
                ApiStatus = ApiStatus.NotValid,
            };
        }

        private async Task<ApiToken> ApiLogin()
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
                    //Message = JsonSerializer.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                    return new List<GusContractor>();
                }
            }
            //Message = response.ReasonPhrase;
            return new List<GusContractor>();
        }

        private List<Contractor> GusContractorsToContractors(List<GusContractor> gusContractors
        )
        {
            List<Contractor> contractors = new List<Contractor>();
            try
            {
                foreach (var item in gusContractors)
                {
                    contractors.Add(new Contractor
                    {
                        Name = item.Name,
                        VatCode = item.VatNumber,
                        City = item.City,
                        PostCode = item.PostalCode,
                        CountryCode = "PL",
                        StreetAndNumber = item.Street,
                        ContractorValid = true,
                        CheckDate = DateTime.Now,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return contractors;
        }

        public Task<ApiResponseContractor> CheckContractorByViesApi(string countryCode, string vatId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Contractor> GetContractorByVatId(string vatId)
        {
            throw new System.NotImplementedException();
        }

        public Task TransferContractors(List<Document> documents, Target target)
        {
            throw new System.NotImplementedException();
        }

        public Task TransferDocuments(List<Document> documents, Target target)
        {
            throw new System.NotImplementedException();
        }
    }
}