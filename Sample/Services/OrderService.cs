using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Services
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
