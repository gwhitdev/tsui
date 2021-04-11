using System;
using tsui.DataModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace tsui.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDataModel>> GetAllUsers();
        //public UserDataModel GetUser();
        //public bool CreateUser();
        //public bool UpdateUser();
        //public bool DeleteUser();
    }
}
