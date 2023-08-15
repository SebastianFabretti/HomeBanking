using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories.Interface
{
    public interface ILoanRepository
    {
        Loan FindById(long Id);
        IEnumerable<Loan> GetAllLoans();
    }
}
