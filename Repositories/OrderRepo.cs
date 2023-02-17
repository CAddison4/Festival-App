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
        public List<Order> GetOrdersByEmail(string email)
        {
            List<Order>? orders = _db.Orders.Where(o => o.Email == email).ToList();

            if (orders == null)
            {
                throw (new Exception("Cannot find orders"));
            }

            return orders;
        }
    }
}
