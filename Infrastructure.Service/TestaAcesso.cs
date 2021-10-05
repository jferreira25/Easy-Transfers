using Easy.Transfers.CrossCutting.Configuration;
using Easy.Transfers.CrossCutting.Configuration.Exceptions;
using Easy.Transfers.Domain.Interfaces.Services;
using Easy.Transfers.Domain.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Transfers.Infrastructure.Service
{
    public class TestaAcesso : ITestaAcesso
    {
        private readonly HttpClient _httpClient;
        public TestaAcesso(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Account> GetAccountByAccountNumber(string accountNumber)
        {
            var uri = $"{ AppSettings.Settings.UrlAccount}/Api/Account/{accountNumber}";

            var httpResponse = await _httpClient.GetAsync(uri);

            if (!httpResponse.IsSuccessStatusCode)
                MakeResponseErrorByAccountNumber(httpResponse.StatusCode, "GET",await httpResponse.Content.ReadAsStringAsync());

            var response = JsonConvert.DeserializeObject<Account>(await httpResponse.Content.ReadAsStringAsync());

            return response;
        }
        public async Task TransferAsync(TransferAccount transferAccount)
        {

            var uri = $"{ AppSettings.Settings.UrlAccount}/Api/Account";

            using HttpContent stringContent = new StringContent(JsonConvert.SerializeObject(transferAccount), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync(uri, stringContent);

            if (!httpResponse.IsSuccessStatusCode)
                MakeResponseErrorByAccountNumber(httpResponse.StatusCode, "POST", await httpResponse.Content.ReadAsStringAsync());
        }

        public async Task<List<Account>> GetAccounts()
        {
            var uri = $"{ AppSettings.Settings.UrlAccount}/Api/Account";

            var httpResponse = await _httpClient.GetAsync(uri);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return default;
            }

            var response = JsonConvert.DeserializeObject<List<Account>>(await httpResponse.Content.ReadAsStringAsync());

            return response;
        }

        private void MakeResponseErrorByAccountNumber(HttpStatusCode httpStatus, string httpType,string errorMessage)
        {
            switch (httpStatus)
            {
                case HttpStatusCode.NotFound:
                    throw new ApiHttpCustomException("Invalid account number", httpStatus, null);
                default:
                    if (httpType == "GET")
                        throw new ApiHttpCustomException("Ocurred one error with get account by accountNumber", httpStatus, null);
                    else
                        throw new ApiHttpCustomException($"Ocurred one error with Transfer Accounts- {errorMessage}", httpStatus, null);

            }
        }
    }
}
