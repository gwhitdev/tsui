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
    public class VolunteerService : IVolunteerService
    {
        private readonly IHttpClientFactory _timesharerapiClientFactory;
        private readonly ILogger _logger;
        public VolunteerService(IHttpClientFactory volunteerClientFactory, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<VolunteerService>();
            _timesharerapiClientFactory = volunteerClientFactory;
        }
        private static List<VolunteerDataModel> ConvertDataStringToVolunteersList(string toConvert)
        {
            JArray convertedResult = JArray.Parse(toConvert);
            IList<JToken> results = convertedResult[0]["data"].Children().ToList();

            List<VolunteerDataModel> volunteersList = new();

            foreach (JToken u in results)
            {
                VolunteerDataModel volunteer = u.ToObject<VolunteerDataModel>();
                volunteersList.Add(volunteer);
            }

            return volunteersList;
        }
        private static List<VolunteerDataModel> ReturnReceivedVolunteersList(string stringOfVolunteers)
        {
            List<VolunteerDataModel> volunteersList = ConvertDataStringToVolunteersList(stringOfVolunteers);
            return volunteersList;
        }
        private async Task<bool> CreateNewVolunteerInAppDb(string accessToken, string volunteerId)
        {
            bool volunteerCreated = true;
            bool volunteerNotCreated = false;

            try
            {
                var request = CreatePostRequestObject($"volunteers/{volunteerId}");
                var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode) return volunteerCreated;
                return volunteerNotCreated;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: ", ex.Message);
            }
            throw new Exception();
        }
        public async Task<bool> CheckIfVolunteerExists(string volunteerId)
        {
            try
            {
                List<VolunteerDataModel> exists = await GetVolunteerAsync(volunteerId);
                if (exists.Count > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: ", ex.Message);
            }
            throw new Exception();
        }

        public async Task<bool> CreateVolunteer(string accessToken, string volunteerId)
        {
            bool appDbVolunteerCreated = true;
            bool appDbVolunteerNotCreated = false;

            bool exists = await CheckIfVolunteerExists(volunteerId);
            if (!exists)
            {
                var succesfulAppVolunteerCreation = await CreateNewVolunteerInAppDb(accessToken, volunteerId);
                if (succesfulAppVolunteerCreation) return appDbVolunteerCreated;
            }
            return appDbVolunteerNotCreated;
        }

        

        public async Task<List<VolunteerDataModel>> GetAllVolunteersAsync()
        {
            try
            {
                var request = CreateGetRequestObject("volunteers");
                var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                var listOfVolunteers = ReturnReceivedVolunteersList(result);
                return listOfVolunteers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            throw new Exception();
        }

        public async Task<List<VolunteerDataModel>> GetVolunteerAsync(string volunteerId)
        {
            try
            {
                var request = CreateGetRequestObject($"volunteers/{volunteerId}");
                var client = _timesharerapiClientFactory.CreateClient("timesharerapiServiceClient");
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                var listForVolunter = ReturnReceivedVolunteersList(result);
                return listForVolunter;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            throw new Exception();
        }
    }
}
