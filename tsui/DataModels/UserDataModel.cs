using System;
using System.Collections.Generic;

namespace tsui.DataModels
{
    public class UserDataModel : BaseDataModel
    {
        public string AuthId { get; set; }
        public UserDetails Details { get; set; }
    }

    public class UserDetails
    {
        public List<string> AssignedOrganisations { get; set; }
        public string AssociatedVolunteerId { get; set; }
    }
}
