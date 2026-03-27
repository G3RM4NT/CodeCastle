using WorkTestAPI.Models;
using WorkTestAPI.Repositories;

namespace WorkTestAPI.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;

        public ProductoService(IProductoRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Producto>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Producto?> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<Producto> Create(Producto producto)
        {
            return await _repo.Create(producto);
        }

        public async Task<Producto> Update(int id, Producto producto)
        {
            producto.Id = id;
            return await _repo.Update(producto);
        }

        public async Task<bool> Delete(int id)
        {
            return await _repo.Delete(id);
        }
    }
}