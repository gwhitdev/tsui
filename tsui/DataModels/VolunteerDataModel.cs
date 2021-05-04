namespace tsui.DataModels
{
    public class VolunteerDataModel : BaseDataModel
    {
        public VolunteerDetails Details { get; set; }
    }
    public class VolunteerDetails 
    {
        public string AssociatedUserId { get; set; }
        public string Name { get; set; }
    }
}
