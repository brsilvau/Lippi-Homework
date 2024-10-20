using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using lippi_homework.Controllers;
using lippi_homework.Models;

namespace lippi_homework.Tests
{
    [TestFixture]
    public class ProductosControllerTests
    {
        private Mock<AppDbContext>? _mockContext;
        private ProductosController? _controller;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<AppDbContext>();
            _controller = new ProductosController(_mockContext.Object);
        }
        // GET api/productos RETORNA 200
        [Test]
        public async Task GetProductos_Returns200()
        {
            var products = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Producto1", Precio = 15990.9m, Stock = 2 },
                new Producto { Id = 2, Nombre = "Producto2", Precio = 17990.9m, Stock = 3 }
            };
            var mockSet = new Mock<DbSet<Producto>>();
            mockSet.As<IQueryable<Producto>>().Setup(m => m.Provider).Returns(products.AsQueryable().Provider);
            mockSet.As<IQueryable<Producto>>().Setup(m => m.Expression).Returns(products.AsQueryable().Expression);
            mockSet.As<IQueryable<Producto>>().Setup(m => m.ElementType).Returns(products.AsQueryable().ElementType);
            mockSet.As<IQueryable<Producto>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Producto>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(new AsyncEnumerableMock<Producto>(products).GetAsyncEnumerator());
            _mockContext?.Setup(c => c.Producto).Returns(mockSet.Object);
            var result = await _controller!.GetProductos();

            // Asserts
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);
            Assert.IsInstanceOf<IEnumerable<Producto>>(okResult?.Value);
            var productList = okResult?.Value as IEnumerable<Producto>;
            Assert.AreEqual(2, productList?.Count());
        }
        // GET api/productos/{id} RETORNA 200
        [Test]
        public async Task GetProductoById_Returns200()
        {
            var product = new Producto { Id = 1, Nombre = "Producto1", Precio= 17.0m,  Stock= 1};
            var mockSet = new Mock<DbSet<Producto>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(product);
            _mockContext?.Setup(c => c.Producto).Returns(mockSet.Object);
            var result = await _controller!.GetProductById(1);

            // Asserts
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);
            Assert.IsInstanceOf<Producto>(okResult?.Value);
            var returnedProduct = okResult?.Value as Producto;
            Assert.AreEqual(1, returnedProduct?.Id);
            Assert.AreEqual("Producto1", returnedProduct?.Nombre);
        }
        // GET api/productos/{id} retorna 404
        [Test]
        public async Task GetProductoById_ReturnsNotFound()
        {
            var mockSet = new Mock<DbSet<Producto>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync((Producto)null!);
            _mockContext?.Setup(c => c.Producto).Returns(mockSet.Object);
            var result = await _controller!.GetProductById(1);
            var notFoundResult = result.Result as NotFoundObjectResult;
            // Asserts
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult?.StatusCode);
            Assert.AreEqual("No se encontraron resultados", notFoundResult?.Value?.GetType().GetProperty("mensaje")?.GetValue(notFoundResult.Value));
        }
        // POST api/productos RETORNA 201
        [Test]
        public async Task CreateProducto_Returns201()
        {
            var newProduct = new Producto { Id = 1, Nombre = "ProductoNuevo" };
            var mockSet = new Mock<DbSet<Producto>>();
            _mockContext?.Setup(c => c.Producto).Returns(mockSet.Object);
            _mockContext?.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1); // Simula guardar cambios
            var result = await _controller!.CreateProduct(newProduct);

            // Asserts
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult?.StatusCode);
            Assert.AreEqual(nameof(ProductosController.GetProductById), createdResult?.ActionName);
            Assert.IsInstanceOf<Producto>(createdResult?.Value);
            var createdProduct = createdResult?.Value as Producto;
            Assert.AreEqual(newProduct.Id, createdProduct?.Id);
            Assert.AreEqual(newProduct.Nombre, createdProduct?.Nombre);
        }
        // PUT api/productos/{id} RETORNA 400
        [Test]
        public async Task UpdateProducto_ReturnsBadRequest()
        {
            var productId = 1;
            var mismatchedProduct = new Producto { Id = 2, Nombre = "Producto Incongruente" };
            _mockContext?.Setup(c => c.Producto).Returns(Mock.Of<DbSet<Producto>>());

            var result = await _controller!.UpdateProduct(productId, mismatchedProduct);

            // Asserts
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult?.StatusCode);
            Assert.AreEqual("El ID del producto no coincide.", badRequestResult?.Value);
        }
        // PUT api/productos/{id} RETURNS 404
        [Test]
        public async Task UpdateProducto_ReturnsNotFound()
        {
            var productId = 1;
            var productToUpdate = new Producto { Id = productId, Nombre = "Producto No Existente" };
            var mockSet = new Mock<DbSet<Producto>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Producto)null!);
            _mockContext?.Setup(c => c.Producto).Returns(mockSet.Object);

            var result = await _controller!.UpdateProduct(productId, productToUpdate);

            // Asserts
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult?.StatusCode);
        }
        // DELETE api/productos/{id} OK
        [Test]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            var productId = 1;
            var productToDelete = new Producto { Id = productId, Nombre = "Producto Para Borrar" };
            var mockSet = new Mock<DbSet<Producto>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(productToDelete);
            _mockContext?.Setup(c => c.Producto).Returns(mockSet.Object);

            var result = await _controller!.DeleteProduct(productId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
        // DELETE api/productos/{id} RETURNS 404
        [Test]
        public async Task DeleteProduct_ReturnsNotFound()
        {
            var productId = 999;
            Producto? productToDelete = null;
            var mockSet = new Mock<DbSet<Producto>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(productToDelete);
            _mockContext?.Setup(c => c.Producto).Returns(mockSet.Object);

            var result = await _controller!.DeleteProduct(productId);

            // Asserts
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult?.StatusCode);
            Assert.IsNotNull(notFoundResult?.Value);
        }
    }
}
