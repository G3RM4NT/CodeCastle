using WorkTestAPI.Models;

namespace WorkTestAPI.Repositories
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAll();
        Task<Producto?> GetById(int id);
        Task<Producto> Create(Producto producto);
        Task<Producto> Update(Producto producto);
        Task<bool> Delete(int id);

    }
}