using Microsoft.CognitiveServices.ContentModerator.Models;
using System.Threading.Tasks;

namespace FN18.Blazor.Shared.Interfaces
{
    public interface IModeratorService
    {
        Task<Screen> GetModeratorClient(string text);
    }
}