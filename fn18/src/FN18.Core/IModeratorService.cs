using System.Threading.Tasks;

namespace FN18.Core
{ 
    public interface IModeratorService
    {
        Task<ModeratorResult> ScoreText(string text);

    }
}