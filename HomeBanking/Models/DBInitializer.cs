using System.ComponentModel;
using System.Linq;

namespace HomeBanking.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                Client[] clients = new Client[]
                {
                   new Client
                   {
                       FirstName="Seba",
                       LastName="Fabre",
                       Email = "seba@gmail.com",
                       Password="1111"
                   },
                   new Client
                   {
                       FirstName="Sebas",
                       LastName="Fabres",
                       Email = "sebas@gmail.com",
                       Password="2222"
                   },
                };

                foreach (Client client in clients)
                {
                    context.Clients.Add(client);
                }
                context.SaveChanges();
            }
        }
    }
}
