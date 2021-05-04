using System.Collections.Generic;

namespace tsui.DataModels
{
    public class OpportunityDataModel : BaseDataModel
    {
        public OpportunityDetails Details { get; set; }
    }
    public class OpportunityDetails 
    {
        public string Title { get; set; }
        public bool Live { get; set; }
        public string Town { get; set; }
        public List<string> AssignedVolunteers { get; set; }
    }
}
