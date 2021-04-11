using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
//using System.Text.Json;
using Newtonsoft.Json.Serialization;
using tsui.DataModels;
using tsui.Interfaces;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace tsui.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _timesharerapiClientFactory;
        private readonly ILogger _logger;

        public UserService(IHttpClientFactory userClientFactory, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserService>();
            _timesharerapiClientFactory = userClientFactory;
        }
        public async Task<List<UserDataModel>> GetAllUsers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "users");

            var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");

            var response = await client.SendAsync(request);
            
            List<UserDataModel> usersList = new();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JArray convertedResult = JArray.Parse(result);
                    IList<JToken> results = convertedResult[0]["data"].Children().ToList();

                    foreach (JToken u in results)
                    {
                        UserDataModel user = u.ToObject<UserDataModel>();
                        usersList.Add(user);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }
                
            }
            return usersList ;
        }
    }
}
