using HomeBanking.Models;

namespace HomeBanking.Repositories.Interface
{
    public interface IClientLoanRepository
    {
        public void Save(ClientLoan clientLoan);
    }
}
