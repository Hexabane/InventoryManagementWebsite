using System;
using System.Collections.Generic;

#nullable disable

namespace FarmCentralApp.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductType { get; set; }
        public int ProductPrice { get; set; }
        public DateTime ProductDaterange { get; set; }
        public int UsersId { get; set; }

        public virtual User Users { get; set; }
    }
}
