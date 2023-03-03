namespace TeamRedInternalProject.ViewModel
{
    // this is to bind the incoming information from the PayPal post request data
    public class TicketRequestVM
    {
        public string ticketType { get; set; }
        public int quantity { get; set; }
    }
}
