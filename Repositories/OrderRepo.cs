using System.Net.Sockets;
using TeamRedInternalProject.Models;
using TeamRedInternalProject.ViewModel;

namespace TeamRedInternalProject.Repositories
{
    public class OrderRepo
    {
        private readonly ConcertContext _db;
        private readonly UserRepo _userRepo;

        public OrderRepo(ConcertContext db)
        {
            _db = db;
            _userRepo = new UserRepo();
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

        /// <summary>
        /// Create an order object using the order id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Order</returns>
        public Order GetOrderById(int id)
        {
            Order order = _db.Orders.Where(o => o.OrderId == id).FirstOrDefault();

            return order;
        }
        /// <summary>
        /// 1. Create order date
        /// 2. Create order object
        /// 3. Save order to database
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="payerEmail"></param>
        public Order CreateNewOrder(string userEmail, string payerEmail)
        {
            // get current date
            DateTime orderDate = DateTime.Now;

            //Create Order Object
            Order order = new Order()
            {
                OrderDate = orderDate,
                Email = userEmail,
                PayerEmail = payerEmail
            };

            //Add and Save to database
            try
            {
                _db.Orders.Add(order);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }

            return order;
        }

        /// <summary>
        /// Create an Order Confirmation object to pass back to the view upon purchase.
        /// 1. Iterate through the Tickets list item to identify the name of the ticket type. Add types to list
        /// 2. Iterate through list to add ticket type names to dictionary to get quantities purchased.
        /// 3. Create Order confirmation object.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user"></param>
        /// <param name="tickets"></param>
        /// <returns>Order Confirmation View Model Object</returns>
        public PurchaseConfirmationVM CreateOrderConfirmation(Order order, User user, List<Ticket> tickets)
        {
            Dictionary<string, int> ticketTypeCounts = new Dictionary<string, int>();
            List<string> ticketTypes = new List<string>();

            string ticketTypeName = "";
            DateTime orderDate = order.OrderDate;

            foreach (Ticket ticket in tickets)
            {
                TicketType ticketType = _db.TicketTypes.Where(tt => tt.TicketTypeId == ticket.TicketTypeId).FirstOrDefault();
                ticketTypeName = ticketType.Type;
                ticketTypes.Add(ticketTypeName);
            }

            foreach (string type in ticketTypes)
            {
                if (ticketTypeCounts.ContainsKey(type))
                {
                    ticketTypeCounts[type]++;
                }
                else
                {
                    ticketTypeCounts.Add(type, 1);
                }
            }

            PurchaseConfirmationVM purchaseConfirmationVM = new PurchaseConfirmationVM()
            {
                OrderId = order.OrderId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrderDate = orderDate,
                OrderEmail = user.Email,
                PayerEmail = order.PayerEmail,
                TicketTypes = ticketTypeCounts
            };

            return purchaseConfirmationVM;
        }
    }
}
