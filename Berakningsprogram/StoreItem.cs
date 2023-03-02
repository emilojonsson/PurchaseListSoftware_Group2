using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace purchase_list_group2
{
    [DataContract]
    public class StoreItem : Item
    {
        [DataMember]
        public Guid ItemID { get; set; }
        [DataMember]
        public ShoppingListItem.UnitEnum Unit { get; set; }
        public StoreItem(string name, string category, double price, ShoppingListItem.UnitEnum unit)
        {
            this.Name = name;
            this.Category = category;
            this.Price = price;
            this.ItemID = Guid.NewGuid();
            this.Unit = unit;
        }

        public override string ToString()
        {
            return $"Name: {Name}, ItemID: {ItemID}, Category: {Category}, Price: {Price}, Unit: {Unit}";
        }
    }
}