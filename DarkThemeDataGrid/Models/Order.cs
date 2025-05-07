using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkThemeDataGrid.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ShipCountry { get; set; }
        public DateTime ShippingDate { get; set; }
        public bool DeliveryStatus { get; set; }
        public string ImagePath { get; set; }
    }

}
