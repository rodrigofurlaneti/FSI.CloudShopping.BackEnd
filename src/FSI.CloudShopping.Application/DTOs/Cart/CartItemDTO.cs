namespace FSI.CloudShopping.Application.DTOs.Cart
{
    public class CartItemDTO
    {
        public CartItemDTO() { }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalPrice { get; set; }
    }
}