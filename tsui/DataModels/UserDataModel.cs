using System;
using System.Collections.Generic;

namespace tsui.DataModels
{
    public class UserDataModel
    {
        public string Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AuthId { get; set; }
        public Details Details { get; set; }
    }

    public class Details
    {
        public List<string> AssignedOrganisations { get; set; }
        public string AssociatedVolunteerId { get; set; }
    }
}
