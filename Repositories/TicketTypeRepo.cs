using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{
    public class TicketTypeRepo
    {
        private readonly ConcertContext _db;
        public TicketTypeRepo()
        {
            _db = new();
        }

        public List<TicketType> GetTicketTypes()
        {
            return _db.TicketTypes.ToList();
        }

        public string CreateTicketType(string type, int price)
        {
            string message;
            try
            {
                TicketType ticketType = new TicketType()
                {
                    Type = type,
                    Price = price
                };

                _db.TicketTypes.Add(ticketType);
                _db.SaveChanges();
                return message = "Success";
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return message;
            }
        }
    }
}
