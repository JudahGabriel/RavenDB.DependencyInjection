using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using RavenDB.DI.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RavenDB.DI.Sample.Services
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
