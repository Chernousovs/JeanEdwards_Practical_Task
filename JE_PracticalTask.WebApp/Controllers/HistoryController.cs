using JE_PracticalTask.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JE_PracticalTask.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IHistoryLogic _historyLogic;

        public HistoryController(IHistoryLogic historyLogic)
        {
            _historyLogic = historyLogic;
        }

        public IActionResult Index()
        {
            var queryHistoryList = _historyLogic.GetQueryHistory();
            return View(queryHistoryList);
        }
    }
}
