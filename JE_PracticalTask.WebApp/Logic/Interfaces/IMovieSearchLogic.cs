using JE_PracticalTask.Models;
using System.Threading.Tasks;

namespace JE_PracticalTask.Logic.Interfaces
{
    public interface IMovieSearchLogic
    {
        Task<MovieSearchModel> SearchMoviesByTitle(string movieToSearch);
    }
}
