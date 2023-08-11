using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories.Interface
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
        void Save(Account account);
        IEnumerable<Account> GetAccountsByClient(long clientId);
        Account FindByAccountNumber(string number);
    }
}
