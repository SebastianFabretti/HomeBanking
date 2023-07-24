using System;
using System.ComponentModel;
using System.Linq;
using HomeBanking;

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

            if (!context.Accounts.Any())
            {
                var accountSeba = context.Clients.FirstOrDefault(c => c.Email == "seba@gmail.com");
                if (accountSeba != null)
                {
                    var accounts = new Account[]
                    {
                        new Account
                        {
                            ClientId = accountSeba.Id,
                            CreationDate = DateTime.Now,
                            Number = string.Empty,
                            Balance = 0,
                        }
                    };
                    foreach (Account account in accounts)
                    {
                        context.Accounts.Add(account);
                    }
                   context.SaveChanges();
                }
            }
        }
    }
}
