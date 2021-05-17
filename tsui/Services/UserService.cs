using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using tsui.DataModels;
using tsui.Interfaces;
using System.Linq;
using Microsoft.Extensions.Logging;
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

        private static List<UserDataModel> ConvertDataStringToUsersList(string toConvert)
        {
            JArray convertedResult = JArray.Parse(toConvert);
            IList<JToken> results = convertedResult[0]["data"].Children().ToList();

            List<UserDataModel> usersList = new();

            foreach (JToken u in results)
            {
                UserDataModel user = u.ToObject<UserDataModel>();
                usersList.Add(user);
            }

            return usersList;
        }
        private static List<UserDataModel> ReturnReceivedUsersList(string stringOfUsers)
        {
            List<UserDataModel> usersList = ConvertDataStringToUsersList(stringOfUsers);
            return usersList;
        }
        private async Task<bool> CreateNewUserInAppDb(string accessToken, string userId)
        {
            bool userCreated = true;
            bool userNotCreated = false;

            try
            {
                var request = CreatePostRequestObject($"users/{userId}");
                var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode) return userCreated;
                return userNotCreated;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: ", ex.Message);
            }
            return userNotCreated;
        }

        public async Task<List<UserDataModel>> GetAllUsersAsync()
        {
            try
            {
                var request = CreateGetRequestObject("users");
                var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                var listOfUsers = ReturnReceivedUsersList(result);
                return listOfUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            throw new Exception();
        }

       

        public async Task<List<UserDataModel>> GetUserAsync(string userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"users/{userId}");
            var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) throw new Exception();
            List<UserDataModel> userList = new();
            try
            {
                var result = await response.Content.ReadAsStringAsync();
                JArray convertedResult = JArray.Parse(result);
                IList<JToken> results = convertedResult[0]["data"].Children().ToList();
                

                foreach (JToken u in results)
                {
                    UserDataModel user = u.ToObject<UserDataModel>();
                    userList.Add(user);
                }
                return userList;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return userList;
        }

        public async Task<bool> CheckIfUserExists(string userId)
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
            bool appDbUserCreated = true;
            bool appDbUserNotCreated = false;

            bool exists = await CheckIfUserExists(userId);
            if (!exists)
            {
                var succesfulAppUserCreation = await CreateNewUserInAppDb(accessToken, userId);
                if (succesfulAppUserCreation) return appDbUserCreated;
            }
            return appDbUserNotCreated;
        }
    }
}
