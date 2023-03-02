using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace inköpslista_grupp2
{
    abstract public class Item
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public UnitEnum Unit { get; set; }
        public double Price { get; set; }
        public Guid ItemID { get; set; }
        //public Item(string name, string category, UnitEnum unit, double price)
        //{
        //    Name = name;
        //    Category = category;
        //    Unit = unit;
        //    Price = price;
        //    ItemID = Guid.NewGuid();
        //}
        public string CSVify()
        {
            return $"{nameof(Name)}:{Name},{nameof(Category)}:{Category},{nameof(Unit)}:{Unit},{nameof(Price)}:{Price},{nameof(ItemID)}:{ItemID}";
        }
        public enum UnitEnum
        {
            Pcs = 1,
            Litres,
            Gram
        }
        public abstract string ToString();
    }
}