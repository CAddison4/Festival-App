using System.ComponentModel.DataAnnotations;

namespace TeamRedInternalProject.ViewModel
{
    public class TicketVM
    {
        [Key]
        [Display(Name = "Ticket Number")]
        public int TicketId {  get; set; }
        [Display(Name = "Ticket Type")]
        public string TicketType { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string Location {  get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
