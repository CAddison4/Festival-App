using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.ViewModel
{
    public class CreateTicketVM
    {
        public TicketType TicketType { get; set; }
        public FestivalTicketType FestivalTicketType { get; set; }
        public IEnumerable<Festival> Festivals { get; set; }
    }
}
