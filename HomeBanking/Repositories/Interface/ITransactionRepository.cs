using HomeBanking.Models;

namespace HomeBanking.Repositories.Interface
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
    }
}