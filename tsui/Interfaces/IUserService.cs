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
        public bool CreateUser();
        //public bool UpdateUser();
        //public bool DeleteUser();
    }
}
