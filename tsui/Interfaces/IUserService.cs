using System;
using tsui.DataModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace tsui.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDataModel>> GetAllUsersAsync();
        public Task<List<UserDataModel>> GetUserAsync(string userId);
        public Task<bool> CreateUser(string accessToken, string userId);
        //private abstract Task<bool> CheckIfUserExists(string userId);
        //public bool UpdateUser();
        //public bool DeleteUser();
    }
}
