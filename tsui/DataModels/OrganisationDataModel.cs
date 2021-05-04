using System.Collections.Generic;

namespace tsui.DataModels
{
    public class OrganisationDataModel : BaseDataModel
    {
        public OrganisationDetails Details { get; set; }
    }

    public class OrganisationDetails
    {
        public string Name { get; set; }
        public string Town { get; set; }
        public List<string> OpportunityIds { get; set; }
    }
}
