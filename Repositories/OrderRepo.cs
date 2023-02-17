using TeamRedInternalProject.Models;

namespace TeamRedInternalProject.Repositories
{
    public class OrderRepo
    {
        private readonly ConcertContext _db;

        public OrderRepo()
        {
            _db = new();
        }
    }
}
