using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmCentralApp.OOP
{
    public class Products : IComparable
    {
        private string type;
        private int price;
        private DateTime dateRange;

        public Products()
        {

        }

        public Products(string type, int price, DateTime dateRange)
        {
            Type = type;
            Price = price;
            DateRange = dateRange;
        }

        public string Type { get => type; set => type = value; }
        public int Price { get => price; set => price = value; }
        public DateTime DateRange { get => dateRange; set => dateRange = value; }

        public int CompareTo(object obj)
        {
            return type.CompareTo(obj.ToString());


        }
        public override string ToString()
        {
            return type;
        }

    }
}
