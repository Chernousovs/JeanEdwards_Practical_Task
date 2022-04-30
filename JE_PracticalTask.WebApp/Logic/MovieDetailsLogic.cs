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
    public class MovieDetailsLogic : IMovieDetailsLogic
    {
        private readonly HttpClient _client;

        public MovieDetailsLogic(HttpClient client)
        {
            _client = client;
        }

        public async Task<MovieDetails> SearchMoviesByImdbId(string imdbIdToSearch)
        {
            if (string.IsNullOrEmpty(imdbIdToSearch))
            {
                throw new ArgumentException("Imdb ID is not provided.");
            }

            try
            {
                string responseBody =
                    await _client.GetStringAsync(string.Format(OMDb_API.FindByImdbId, imdbIdToSearch));

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                MovieDetails movieInfoModel = JsonSerializer.Deserialize<MovieDetails>(responseBody, options);

                if (movieInfoModel?.Response == "False")
                {
                    throw new MovieDetailsSearchException(movieInfoModel.Error);
                }

                return movieInfoModel;
            }
            catch (HttpRequestException)
            {
                throw new HttpRequestException("Network connection problem.");
            }
        }
    }
}
