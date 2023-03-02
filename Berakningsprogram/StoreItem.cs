using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace inköpslista_grupp2
{
    public class StoreItem : Item
    {
        public Guid ItemID { get; set; }
        public ShoppingListItem.UnitEnum Unit { get; set; }
        public StoreItem(string name, string category, double price, ShoppingListItem.UnitEnum unit)
        {
            this.Name = name;
            this.Category = category;
            this.Price = price;
            this.ItemID = new Guid();
            this.Unit = unit;
        }

        public override string ToString()
        {
            return $"Name: {Name}, ItemID: {ItemID}, Category: {Category}, Price: {Price}, Unit: {Unit}";
        }
    }
}