using HomeBanking.Models;
using System.Collections;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAllAccounts();
        Account FindById(long id);
        void Save (Account account);
    }
}
