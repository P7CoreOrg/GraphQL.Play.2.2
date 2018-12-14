using System.Threading.Tasks;

namespace TheApp.Services
{
    public interface IJsonFileLoader
    {
        Task<object> LoadAsync(string pathFragment);

    }
}