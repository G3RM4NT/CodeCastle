using WorkTestAPI.Models;

namespace WorkTestAPI.Services
{
    public interface IProductoService
    {
        Task<List<Producto>> GetAll();
        Task<Producto?> GetById(int id);
        Task<Producto> Create(Producto producto);
        Task<Producto> Update(int id, Producto producto);
        Task<bool> Delete(int id);
    }
}