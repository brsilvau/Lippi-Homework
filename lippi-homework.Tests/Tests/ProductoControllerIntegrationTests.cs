using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using lippi_homework.Models;
using System.Text;
using System.Text.Json;
using Xunit;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            var serviceProvider = services.BuildServiceProvider();
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }
        });
    }
}
/* 
public class ProductosControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductosControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAndGetProduct_ReturnsCreatedAndOk()
    {
        var newProduct = new
        {
            Nombre = "Producto Test",
            Precio = 19990.00M,
            Stock = 10,
        };
        var json = JsonSerializer.Serialize(newProduct);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Se crea un nuevo producto
        var createResponse = await _client.PostAsync("/api/productos", content);
        createResponse.EnsureSuccessStatusCode(); 

        // Se obtienen todos los productos
        var getResponse = await _client.GetAsync("/api/productos");
        getResponse.EnsureSuccessStatusCode();

        // Assert: se verifica que el producto se encuentre en la lista
        var responseString = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("Producto Test", responseString);
    }
}
 */