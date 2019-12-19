
namespace smartFunds.Service.Models
{
    public class EventHostModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int HouseholderId { get; set; }
        public int CP { get; set; }
        public int SCP { get; set; }
        public int DefaultCP { get; set; }
        public int DefaultSCP { get; set; }
    }
}
