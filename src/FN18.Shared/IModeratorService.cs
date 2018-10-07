using System.Threading.Tasks;

namespace FN18.Shared
{ 
    public interface IModeratorService
    {
        Task<ModeratorResult> GetModeratorClient(string text);

    }
}