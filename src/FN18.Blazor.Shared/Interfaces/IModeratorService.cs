using FN18.Blazor.Shared.Entities;
using System.Threading.Tasks;

namespace FN18.Blazor.Shared.Interfaces
{
    public interface IModeratorService
    {
        Task<ModeratorResult> TextScreen(string text);
    }
}
