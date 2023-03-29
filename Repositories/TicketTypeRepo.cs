using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;

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
            return _db.FestivalTicketTypes
                      .Join(_db.TicketTypes,
                             ftt => ftt.TicketTypeId,
                             tt => tt.TicketTypeId,
                             (ftt, tt) => tt)
                      .ToList();
        }

        public string CreateTicketType(string type, decimal price, int quantity, int festivalId)
        {
            string message;
            try
            {
                TicketType ticketType = new TicketType()
                {
                    Type = type,
                    Price = price
                };

                FestivalTicketType festivalTicketType = new FestivalTicketType()
                {
                    FestivalId = festivalId,
                    TicketType = ticketType,
                    Quantity = quantity
                };

                _db.FestivalTicketTypes.Add(festivalTicketType);
                _db.SaveChanges();

                message = "Success";
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }

            return message;
        }
    }
}
