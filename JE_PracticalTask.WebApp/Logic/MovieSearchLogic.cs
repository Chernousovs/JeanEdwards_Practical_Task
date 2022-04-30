using JE_PracticalTask.APIs;
using JE_PracticalTask.Logic.Interfaces;
using JE_PracticalTask.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using JE_PracticalTask.Exceptions;

namespace JE_PracticalTask.Logic
{
    public class MovieSearchLogic : IMovieSearchLogic
    {
        private readonly HttpClient _client;
        private readonly IHistoryLogic _historyLogic;

        public MovieSearchLogic(HttpClient client, IHistoryLogic historyLogic)
        {
            _client = client;
            _historyLogic = historyLogic;
        }
        public async Task<MovieSearchModel> SearchMoviesByTitle(string movieToSearch)
        {
            if (string.IsNullOrEmpty(movieToSearch))
            {
                throw new ArgumentException("Search string not provided.");
            }

            MovieSearchModel movieSearchModel;

            try
            {
                string responseBody =
                    await _client.GetStringAsync(string.Format(OMDb_API.FindByTitle, movieToSearch));

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                movieSearchModel = JsonSerializer.Deserialize<MovieSearchModel>(responseBody, options);
            }
            catch (HttpRequestException)
            {
                throw new HttpRequestException("Network connection problem.");
            }

            _historyLogic.UpdateQueryHistory(movieToSearch);

            if (movieSearchModel?.Response == "False")
            {
                throw new MovieListSearchException(movieSearchModel.Error);
            }

            return movieSearchModel;
        }
    }
}
