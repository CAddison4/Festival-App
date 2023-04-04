using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Repositories
{
    public class TicketTypeRepo
    {
        private readonly ConcertContext _db;
        public TicketTypeRepo(ConcertContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets the ticket types and saves it to a list 
        /// </summary>
        /// <returns>List TicketType</returns>
        public List<TicketType> GetTicketTypes()
        {
            return _db.FestivalTicketTypes
                      .Join(_db.TicketTypes,
                             ftt => ftt.TicketTypeId,
                             tt => tt.TicketTypeId,
                             (ftt, tt) => tt)
                      .ToList();
        }

        /// <summary>
        /// Creates the TicketType and saves it to the database, First the ticket type, then the festival, and also links the FestivalTicketType table
        /// </summary>
        /// <param name="model"></param>
        /// <returns>String Success</returns>
        public string CreateTicketType(CreateTicketVM model)
        {
            string message;
            try
            {
                // Create the TicketType object
                var ticketType = new TicketType
                {
                    Type = model.TicketType.Type,
                    Price = model.TicketType.Price
                };
                _db.TicketTypes.Add(ticketType);
                _db.SaveChanges();

                // Create the FestivalTicketType object using the TicketType object and the selected Festival object
                var festivalTicketType = new FestivalTicketType
                {
                    TicketTypeId = ticketType.TicketTypeId,
                    FestivalId = model.FestivalTicketType.FestivalId,
                    Quantity = model.FestivalTicketType.Quantity
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
