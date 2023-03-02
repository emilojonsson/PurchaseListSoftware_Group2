
namespace purchase_list_group2
{
    abstract public class Item
    {
        public string? Name { get; set; }
        public string? Category { get; set; }
        public UnitEnum Unit { get; set; }
        public double Price { get; set; }
        public Guid ItemID { get; set; }
        public enum UnitEnum
        {
            Pcs = 1,
            Litres,
            Gram
        }
        public abstract string ToString();
    }
}