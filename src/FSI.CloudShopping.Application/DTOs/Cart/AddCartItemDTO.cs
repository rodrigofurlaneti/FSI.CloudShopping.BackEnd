namespace FSI.CloudShopping.Application.DTOs.Cart
{
    public class AddCartItemDTO
    {
        public Guid SessionToken { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
