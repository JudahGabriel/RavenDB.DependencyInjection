using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Sample3.Models;

namespace Sample3.Services
{
    public class OrderService
    {
        private readonly IAsyncDocumentSession dbSession;

        public OrderService(IAsyncDocumentSession dbSession)
        {
            this.dbSession = dbSession;
        }

        public Task<int> GetOrderCount()
        {
            return this.dbSession
                .Query<Order>()
                .CountAsync();
        }
    }
}
