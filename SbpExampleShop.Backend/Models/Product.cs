namespace SbpExampleShop.Backend.Models
{
    public class Product
    {
        public long id;
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
    }
}