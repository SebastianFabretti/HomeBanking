using HomeBanking.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Card> FindAllCards()
        {
            return FindAll();
        }

        public Card FindById(long id) 
        {
            return FindByCondition(c => c.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Card> GetCardsByClient(long id)
        {
            return FindByCondition(cl => cl.ClientId == id)
                .ToList();
        }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
    }
}
