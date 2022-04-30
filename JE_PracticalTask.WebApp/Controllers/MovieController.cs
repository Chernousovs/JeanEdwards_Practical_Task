using JE_PracticalTask.Logic.Interfaces;
using JE_PracticalTask.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JE_PracticalTask.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieDetailsLogic _movieDetailsLogic;

        public MovieController(IMovieDetailsLogic movieDetailsLogic)
        {
            _movieDetailsLogic = movieDetailsLogic;
        }

        public async Task<IActionResult> Details(string imdbId)
        {
            try
            {
                MovieDetails movieDetails = await _movieDetailsLogic.SearchMoviesByImdbId(imdbId);

                return View(movieDetails);
            }
            catch (Exception e)
            {
                return View("Error", new Error { ErrorMessage = e.Message });
            }
        }
    }
}
