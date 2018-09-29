using System.Threading.Tasks;

namespace FN18.Core.Interfaces
{
    public interface IModeratorService
    {
        Task GetModeratorClient(string text);
    }
}
