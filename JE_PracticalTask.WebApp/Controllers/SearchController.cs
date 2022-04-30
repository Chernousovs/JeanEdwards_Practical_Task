using JE_PracticalTask.Logic.Interfaces;
using JE_PracticalTask.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using JE_PracticalTask.Exceptions;

namespace JE_PracticalTask.Controllers
{
    public class SearchController : Controller
    {
        private readonly IMovieSearchLogic _movieSearchLogic;

        public SearchController(IMovieSearchLogic movieSearchLogic)
        {
            _movieSearchLogic = movieSearchLogic;
        }

        public IActionResult Index()
        {
            MovieSearchModel model = new MovieSearchModel();
            return View(model);
        }

        [Route("SearchString")]
        public async Task<IActionResult> Index(string searchString)
        {
            MovieSearchModel model = new MovieSearchModel();

            try
            {
                model = await _movieSearchLogic.SearchMoviesByTitle(searchString);
            }
            catch (ArgumentException e)
            {
                ViewBag.Information = e.Message;
            }
            catch (MovieListSearchException e)
            {
                ViewBag.Information = e.Message;
            }
            catch (HttpRequestException e)
            {
                return View("Error", new Error { ErrorMessage = e.Message });
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
