using System;
using System.Collections.Generic;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class OrderDetailsBusiness
    {

        private readonly OrderDetailsData orderDetailsData;
        public OrderDetailsBusiness(string connectionString)
        {
            orderDetailsData = new OrderDetailsData(connectionString);
        }

        public List<OrderDetail> GetAll()
        {
            return orderDetailsData.GetAll();
        }

    }
}