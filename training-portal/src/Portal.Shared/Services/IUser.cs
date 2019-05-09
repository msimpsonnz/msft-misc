using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portal.Shared
{
    public interface IUser
    {
        Task<List<PortalUser>> ListUsers();
        Task<PortalUser> GetUser(string id);
        //Task SaveSubmission(Submission submission);
        //Task DeleteSubmission(Submission submission);
        //Task DeleteSubmission(string id);
    }
}