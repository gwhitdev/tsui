using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tsui.DataModels;

namespace tsui.Interfaces
{
    public interface IVolunteerService
    {
        public Task<List<VolunteerDataModel>> GetAllVolunteersAsync();
        public Task<List<VolunteerDataModel>> GetVolunteerAsync(string volunteerId);
        public Task<bool> CreateVolunteer(string accessToken, string volunteerId);
        public Task<bool> CheckIfVolunteerExists(string volunteerId);
        //public bool UpdateVolunteer();
        //public bool DeleteVolunteer();
    }
}
