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
    }
}
