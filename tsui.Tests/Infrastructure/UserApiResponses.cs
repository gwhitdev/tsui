using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using tsui.Tests.Models;
using Newtonsoft.Json.Linq;

namespace tsui.Tests.Infrastructure
{
    public static class UserApiResponses
    {
        public static StringContent OkResponse => BuildOkResponse();
        public static StringContent UnauthorizedRepsonse => BuildUnauthorizedResponse();
        public static StringContent NotFoundResponse => BuildNotFoundResponse();
        public static StringContent InternalErrorResponse => BuildInternalErrorResponse();

        private static StringContent BuildOkResponse()
        {
            var user1 = new UserDataResponseModel
            {
                AuthId = "Test000001",
                //AuthId = "StringToCreateFailure",
                Details = new UserDetails
                {
                    AssignedOrganisations = new()
                    {
                        "TestOrg001",
                    },
                    AssociatedVolunteerId = "TestVol001"
                }
            };

            var user2 = new UserDataResponseModel
            {
                AuthId = "Test000002",
                Details = new UserDetails
                {
                    AssignedOrganisations = new()
                    {
                        "TestOrg002",
                    },
                    AssociatedVolunteerId = "TestVol002"
                }
            };

            var users = new List<UserDataResponseModel>()
            {
                user1,
                user2
            };

            var data = new Dictionary<string, List<UserDataResponseModel>>()
            {
                { "data", users }
            };

            var json = JsonSerializer.Serialize(new[] { data });

            return new StringContent(json);
        }

        private static StringContent BuildUnauthorizedResponse()
        {
            var json = JsonSerializer.Serialize(new
            {
                Code = 401,
                Message = "Invalid Access Token"
            });
            return new StringContent(json);
        }

        private static StringContent BuildNotFoundResponse()
        {
            var json = JsonSerializer.Serialize(new
            {
                Code = 404,
                Message = "User Not Found"
            });
            return new StringContent(json);
        }
        private static StringContent BuildInternalErrorResponse()
        {
            var json = JsonSerializer.Serialize(new
            {
                Code = 500,
                Message = "Internal Server Error"
            });
            return new StringContent(json);
        }
    }
}
