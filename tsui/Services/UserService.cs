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
using tsui.Library;
using static tsui.Library.ServiceRequestHelpers;
using System.Net.Http.Headers;

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

        
        public async Task<List<UserDataModel>> GetAllUsersAsync()
        {
            var request = CreateGetRequestObject("users");
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

        public async Task<List<UserDataModel>> GetUserAsync(string userId)
        {
            var request = CreateGetRequestObject($"users/{userId}");
            var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
            var response = await client.SendAsync(request);

            List<UserDataModel> userList = new();

            if(response.IsSuccessStatusCode)
            {
                try
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JArray convertedResult = JArray.Parse(result);
                    IList<JToken> results = convertedResult[0]["data"].Children().ToList();
                    foreach(JToken u in results)
                    {
                        UserDataModel user = u.ToObject<UserDataModel>();
                        userList.Add(user);
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return userList;
        }
        private async Task<bool> CheckIfUserExists(string userId)
        {
            List<UserDataModel> exists = new();
            try
            {
                exists = await GetUserAsync(userId);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error: ", ex.Message);
            }
            if (exists.Count == 1) return true;
            return false;
        }

        public async Task<bool> CreateUser(string accessToken, string userId)
        {
            bool exists = await CheckIfUserExists(userId);
            
            if (!exists)
            {
                try
                {
                    var request = CreatePostRequestObject($"users/{userId}");
                    var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode) return true;
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error: ", ex.Message);
                }
            }
            return false;
        }
    }
}
