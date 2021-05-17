using Xunit;
using tsui.Interfaces;
using tsui.Services;
using tsui.Tests.Infrastructure;
using tsui.DataModels;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Text.Json;
using tsui.Tests.Models;
using UserDetails = tsui.DataModels.UserDetails;
using System.Collections;

namespace tsui.Tests
{
    public class UnitTest1
    {
        
        [Fact]
        public async Task<List<UserDataModel>> Returns_All_Users()
        {
            var clientFactory = ClientBuilder.UserClientFactory(UserApiResponses.OkResponse);
            IUserService userService = new UserService(clientFactory, new NullLoggerFactory());
            var result = await userService.GetAllUsersAsync();
            var testResult = Assert.IsType<List<UserDataModel>>(result);
            return testResult;
        }
        [Fact]
        public async void Returns_Expected_Values_From_Api()
        {
            var clientFactory = ClientBuilder.UserClientFactory(UserApiResponses.OkResponse);
            IUserService userService = new UserService(clientFactory, new NullLoggerFactory());
            var result = await userService.GetAllUsersAsync();
            
            Assert.Equal("Test000001", result[0].AuthId);
            Assert.Equal("Test000002", result[1].AuthId);            
        }
    }
}
