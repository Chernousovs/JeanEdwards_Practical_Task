using JE_PracticalTask.Models;
using System.Collections.Generic;

namespace JE_PracticalTask.Logic.Interfaces
{
    public interface IHistoryLogic
    {
        void UpdateQueryHistory(string queryString);
        List<QueryHistory> GetQueryHistory();
    }
}
