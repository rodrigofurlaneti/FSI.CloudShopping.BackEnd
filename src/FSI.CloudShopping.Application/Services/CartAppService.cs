using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Application.Services
{
    public class CartAppService : ICartAppService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartService _domainCartService; // O Domain Service do Merge

        public CartAppService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            ICartService domainCartService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _domainCartService = domainCartService;
        }

        public async Task AddItemAsync(string token, AddItemDTO dto)
        {
            // 1. Validar Produto e Estoque via Repositório/Domínio
            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null) throw new ApplicationException("Produto não encontrado.");

            // 2. Localizar ou Criar Carrinho (usando o VO VisitorToken)
            var visitorToken = new VisitorToken(Guid.Parse(token));
            var cart = await _cartRepository.GetByVisitorTokenAsync(visitorToken)
                       ?? new Domain.Entities.Cart(visitorToken);

            // 3. Executar comportamento do Domínio
            cart.AddOrUpdateItem(product.Id, new Quantity(dto.Quantity), product.Price);

            // 4. Persistir
            if (cart.Id == 0)
                await _cartRepository.AddAsync(cart);
            else
                await _cartRepository.UpdateAsync(cart);
        }

        public async Task MergeAfterLoginAsync(Guid visitorToken, int customerId)
        {
            // Orquestra a fusão de carrinhos que definimos no Domínio
            await _domainCartService.MergeCartAsync(new VisitorToken(visitorToken), customerId);
        }

        public async Task<CartDTO> GetCartAsync(string token)
        {
            var visitorToken = new VisitorToken(Guid.Parse(token));
            var cart = await _cartRepository.GetByVisitorTokenAsync(visitorToken);

            if (cart == null) return new CartDTO();

            // Mapeamento Manual (ou use AutoMapper) para o DTO
            return new CartDTO
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                Items = cart.Items.Select(i => new CartItemDTO(
                    i.ProductId,
                    "Product Name", // Idealmente buscar do cache ou join
                    i.Quantity.Value,
                    i.UnitPrice.Value,
                    i.TotalPrice.Value)).ToList(),
                TotalAmount = cart.Items.Sum(x => x.TotalPrice.Value)
            };
        }

        public async Task ClearCartAsync(string token)
        {
            var visitorToken = new VisitorToken(Guid.Parse(token));
            var cart = await _cartRepository.GetByVisitorTokenAsync(visitorToken);

            if (cart == null) return;

            // Remove o carrinho ou limpa os itens dependendo da sua regra
            await _cartRepository.RemoveAsync(cart.Id);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(string token, int productId)
        {
            var cart = await _cartRepository.GetByVisitorTokenAsync(new VisitorToken(Guid.Parse(token)));
            if (cart == null) return;

            // Implementar lógica de remoção na entidade Cart e persistir
            // cart.RemoveItem(productId);
            await _cartRepository.UpdateAsync(cart);
        }
    }
}
