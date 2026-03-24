namespace FSI.CloudShopping.Infrastructure.Data.Seed;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Popula o banco com dados iniciais necessários para o funcionamento do sistema.
/// Execute via: dotnet run --seed (ou via WebApplication.Services.SeedAsync() no Program.cs).
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        await SeedCategoriesAsync(context);
        await SeedProductsAsync(context);
        await SeedCouponsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedCategoriesAsync(AppDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            Category.Create("Eletrônicos", "eletronicos", "Dispositivos eletrônicos em geral", null, 1),
            Category.Create("Computadores", "computadores", "Notebooks, desktops e acessórios", null, 2),
            Category.Create("Periféricos", "perifericos", "Mouse, teclado, headset e mais", null, 3),
            Category.Create("Games", "games", "Consoles, jogos e acessórios gamer", null, 4),
            Category.Create("Acessórios", "acessorios", "Cabos, suportes e outros acessórios", null, 5),
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(AppDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        var computersCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Slug == "computadores");
        var peripheralsCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Slug == "perifericos");
        var gamesCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Slug == "games");

        var products = new List<Product>
        {
            Product.Create(
                sku: "NOTE-DELL-001",
                name: "Notebook Dell Inspiron 15",
                slug: "notebook-dell-inspiron-15",
                description: "Notebook Dell Inspiron 15 3000, Intel Core i5, 8GB RAM, 256GB SSD, Windows 11",
                shortDescription: "Notebook Dell i5 8GB 256GB SSD",
                price: new Money(3_299.99m),
                compareAtPrice: new Money(3_999.99m),
                costPrice: new Money(2_500.00m),
                stockQuantity: 15,
                minStockAlert: 3,
                categoryId: computersCategory?.Id ?? 0,
                imageUrl: "/products/NOTE-DELL-001/NotebookDellInspiron15.jpg",
                weight: 1.8m,
                isFeatured: true),

            Product.Create(
                sku: "MOUSE-LOGI-G502",
                name: "Mouse Gamer Logitech G502 Hero",
                slug: "mouse-gamer-logitech-g502-hero",
                description: "Mouse Gamer Logitech G502 Hero, 25600 DPI, RGB, 11 botões programáveis",
                shortDescription: "Mouse Gamer G502 25600 DPI RGB",
                price: new Money(349.99m),
                compareAtPrice: new Money(449.99m),
                costPrice: new Money(220.00m),
                stockQuantity: 42,
                minStockAlert: 10,
                categoryId: peripheralsCategory?.Id ?? 0,
                imageUrl: "/products/MOUSE-LOGI-G502/MouseGamerLogitechG502.jpg",
                weight: 0.12m,
                isFeatured: true),

            Product.Create(
                sku: "TECL-MECA-RGB",
                name: "Teclado Mecânico RGB Gamer",
                slug: "teclado-mecanico-rgb-gamer",
                description: "Teclado Mecânico RGB Switch Blue, ABNT2, Anti-ghosting",
                shortDescription: "Teclado Mecânico RGB Switch Blue ABNT2",
                price: new Money(289.99m),
                compareAtPrice: null,
                costPrice: new Money(180.00m),
                stockQuantity: 28,
                minStockAlert: 5,
                categoryId: peripheralsCategory?.Id ?? 0,
                imageUrl: "/products/TECL-MECA-RGB/TecladoMecanicoRGB.jpg",
                weight: 0.95m,
                isFeatured: false),

            Product.Create(
                sku: "MONI-LG-29W",
                name: "Monitor UltraWide LG 29\"",
                slug: "monitor-ultrawide-lg-29",
                description: "Monitor LG 29WP500-B UltraWide 29\" IPS 75Hz Full HD",
                shortDescription: "Monitor LG 29\" UltraWide IPS 75Hz",
                price: new Money(1_499.99m),
                compareAtPrice: new Money(1_799.99m),
                costPrice: new Money(1_100.00m),
                stockQuantity: 10,
                minStockAlert: 2,
                categoryId: computersCategory?.Id ?? 0,
                imageUrl: "/products/MONI-LG-29W/MonitorUltraWideLG29.jpg",
                weight: 4.5m,
                isFeatured: true),

            Product.Create(
                sku: "HEAD-HYPR-X",
                name: "Headset HyperX Cloud II",
                slug: "headset-hyperx-cloud-ii",
                description: "Headset Gamer HyperX Cloud II 7.1 Surround, drivers 53mm, microfone destacável",
                shortDescription: "Headset HyperX Cloud II 7.1 Surround",
                price: new Money(499.99m),
                compareAtPrice: new Money(599.99m),
                costPrice: new Money(340.00m),
                stockQuantity: 20,
                minStockAlert: 5,
                categoryId: gamesCategory?.Id ?? 0,
                imageUrl: "/products/HEAD-HYPR-X/HeadsetHyperXCloudII.jpg",
                weight: 0.31m,
                isFeatured: false),

            Product.Create(
                sku: "STOK-SSD-1TB",
                name: "SSD NVMe 1TB Kingston",
                slug: "ssd-nvme-1tb-kingston",
                description: "SSD NVMe PCIe 4.0 Kingston KC3000, 1TB, Leitura 7000MB/s",
                shortDescription: "SSD NVMe 1TB 7000MB/s",
                price: new Money(699.99m),
                compareAtPrice: null,
                costPrice: new Money(480.00m),
                stockQuantity: 35,
                minStockAlert: 8,
                categoryId: computersCategory?.Id ?? 0,
                imageUrl: "/products/STOK-SSD-1TB/SSDNVMeKingSpec1TB.jpg",
                weight: 0.01m,
                isFeatured: false),

            Product.Create(
                sku: "WEBC-LOGI-C920",
                name: "Webcam Logitech C920e HD",
                slug: "webcam-logitech-c920e-hd",
                description: "Webcam Logitech C920e Full HD 1080p, autofoco, microfone integrado",
                shortDescription: "Webcam Logitech 1080p Autofoco",
                price: new Money(499.99m),
                compareAtPrice: new Money(599.99m),
                costPrice: new Money(340.00m),
                stockQuantity: 18,
                minStockAlert: 4,
                categoryId: peripheralsCategory?.Id ?? 0,
                imageUrl: "/products/WEBC-LOGI-C920/WebcamLogitechC920e.jpg",
                weight: 0.16m,
                isFeatured: false),

            Product.Create(
                sku: "CADE-GAM-PRO",
                name: "Cadeira Gamer Ergonômica Pro",
                slug: "cadeira-gamer-ergonomica-pro",
                description: "Cadeira Gamer com suporte lombar ajustável, encosto reclinável 180°, braços 4D",
                shortDescription: "Cadeira Gamer Reclinável 180° Braços 4D",
                price: new Money(1_199.99m),
                compareAtPrice: new Money(1_499.99m),
                costPrice: new Money(780.00m),
                stockQuantity: 8,
                minStockAlert: 2,
                categoryId: gamesCategory?.Id ?? 0,
                imageUrl: "/products/CADE-GAM-PRO/CadeiraGamerErgonomica.jpg",
                weight: 22.0m,
                isFeatured: true),
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCouponsAsync(AppDbContext context)
    {
        if (await context.Coupons.AnyAsync()) return;

        var coupons = new List<Coupon>
        {
            Coupon.Create(
                code: "BEMVINDO10",
                description: "10% de desconto para novos clientes",
                discountType: Domain.Enums.DiscountType.Percentage,
                discountValue: 10m,
                minOrderValue: 100m,
                maxUsages: 1000,
                validFrom: DateTime.UtcNow,
                validTo: DateTime.UtcNow.AddYears(1)),

            Coupon.Create(
                code: "VERAO20",
                description: "20% de desconto — Promoção Verão",
                discountType: Domain.Enums.DiscountType.Percentage,
                discountValue: 20m,
                minOrderValue: 300m,
                maxUsages: 500,
                validFrom: DateTime.UtcNow,
                validTo: DateTime.UtcNow.AddMonths(3)),

            Coupon.Create(
                code: "FRETE0",
                description: "Frete grátis em pedidos acima de R$200",
                discountType: Domain.Enums.DiscountType.Fixed,
                discountValue: 30m,
                minOrderValue: 200m,
                maxUsages: 2000,
                validFrom: DateTime.UtcNow,
                validTo: DateTime.UtcNow.AddMonths(6)),
        };

        await context.Coupons.AddRangeAsync(coupons);
        await context.SaveChangesAsync();
    }
}
