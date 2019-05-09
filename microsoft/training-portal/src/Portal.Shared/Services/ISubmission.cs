using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portal.Shared
{
    public interface ISubmission
    {
        Task<List<Submission>> ListSubmissions();
        Task<Submission> GetSubmission(string id);
        //Task SaveSubmission(Submission submission);
        //Task DeleteSubmission(Submission submission);
        //Task DeleteSubmission(string id);

        Task<string> GetBlobSas();
    }
}