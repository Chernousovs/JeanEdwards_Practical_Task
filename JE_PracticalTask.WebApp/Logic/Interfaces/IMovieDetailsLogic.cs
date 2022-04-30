using JE_PracticalTask.Models;
using System.Threading.Tasks;

namespace JE_PracticalTask.Logic.Interfaces
{
    public interface IMovieDetailsLogic
    {
        Task<MovieDetails> SearchMoviesByImdbId(string imdbIdToSearch);
    }
}
