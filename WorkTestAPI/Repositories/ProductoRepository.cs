using Microsoft.EntityFrameworkCore;
using WorkTestAPI.Data;
using WorkTestAPI.Models;

namespace WorkTestAPI.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAll()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto?> GetById(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<Producto> Create(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> Update(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}