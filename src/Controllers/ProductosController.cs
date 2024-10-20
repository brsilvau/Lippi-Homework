using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lippi_homework.Models;
using Microsoft.Data.SqlClient;

namespace lippi_homework.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context) {
            _context = context;
        }
        // Endpoiint para Obtener todos los productos o filtrar por id y nombre
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos([FromQuery] string? nombre = null, [FromQuery] int? id = null) 
        {
            try 
            {
                IQueryable<Producto> query = _context.Producto;
                if (id.HasValue) 
                {
                    query = query.Where(p => p.Id == id.Value);
                }
                if (!string.IsNullOrWhiteSpace(nombre)) 
                {
                    query = query.Where(p => p.Nombre.Contains(nombre));
                }
                var productos = await query.ToListAsync();
                if (productos == null || productos.Count == 0) 
                {
                    return NotFound(new { mensaje = "No se encontraron resultados" });
                }
                return Ok(productos);
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine(sqlEx);
                return StatusCode(500, new { mensaje = "Error al acceder a la base de datos." });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado." });
            }
        }
        // Obtener un producto por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductById(int id) 
        {
            try 
            {
                var product = await _context.Producto.FindAsync(id);
                if (product == null) 
                {
                    return NotFound(new { mensaje = "No se encontraron resultados" });
                }
                return Ok(product);
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine(sqlEx);
                return StatusCode(500, new { mensaje = "Error al acceder a la base de datos." });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado." });
            }
        }
        // Crear un nuevo producto
        [HttpPost]
        public async Task<ActionResult<Producto>> CreateProduct(Producto producto) 
        {
            try 
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }
                _context.Producto.Add(producto);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProductById), new { id = producto.Id }, producto);
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine(sqlEx);
                return StatusCode(500, new { mensaje = "Error al acceder a la base de datos." });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado." });
            }
        }
        // Actualizar un producto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Producto producto)
        {
            try
            {
                if (producto == null) 
                {
                    return BadRequest("El producto no puede ser nulo.");
                }
                if (id != producto.Id)
                {
                    return BadRequest("El ID del producto no coincide.");
                }
                var existingProduct = await _context.Producto.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { mensaje = "No se encontró el producto a actualizar" });
                }
                _context.Entry(existingProduct).State = EntityState.Detached;
                
                _context.Producto.Update(producto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx);
                return StatusCode(500, new { mensaje = "Error al acceder a la base de datos." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado." });
            }
        }
        // Eliminar un producto existente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) 
        {
            try 
            {
                var product = await _context.Producto.FindAsync(id);
                if (product == null)
                {
                    return NotFound(new { mensaje = "No se encontró el producto a eliminar" });
                }
                _context.Producto.Remove(product);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (SqlException sqlEx) 
            {
                Console.WriteLine(sqlEx);
                return StatusCode(500, new { mensaje = "Error al acceder a la base de datos." });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado." });
            }
        }
    }
}
