using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineFoodAPI.Models;

namespace OnlineFoodAPI.Controllers
{
    public class OrderController : ApiController
    {
        [HttpPost]
        [Route("Order")]

        // Accept dishes list and userId
        public ReceiptModel Order(OrderModel orderDetails)
        {
            int sum = 0;
            ReceiptModel receipt; 

            try
            {
                using (DatabaseFoodOnlineEntityModel db = new DatabaseFoodOnlineEntityModel())
                {

                    if (orderDetails.dishes != null)
                    {
                        foreach (var item in orderDetails.dishes)
                        {
                            sum += item.price;
                        }
                        
                        //TODO: lägg till koppling till order tabellen
                    }

                    return null;
                }
            }
            catch
            {
                // If error, return null
                return null;
            }

        }
    }
}
