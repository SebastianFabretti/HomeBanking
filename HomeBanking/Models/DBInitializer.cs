using System;
using System.ComponentModel;
using System.Linq;
using HomeBanking;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                       FirstName="Victor",
                       LastName="Coronado",
                       Email = "vcoronado@gmail.com",
                       Password="123456"
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
                    Account[] sebaAccounts = new Account[]
                    {
                        new Account
                        {
                            ClientId = accountSeba.Id,
                            CreationDate = DateTime.Now,
                            Number = "VIN002",
                            Balance = 99999,
                        }
                    };               

                    foreach (Account account in sebaAccounts)
                    {
                        context.Accounts.Add(account);
                    }
                   //context.SaveChanges();
                }
            }

            if (!context.Accounts.Any())
            {
                var accountVictor = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");

                if (accountVictor != null)
                {
                    Account[] victorAccounts = new Account[]
                    {
                        new Account
                        {
                            ClientId = accountVictor.Id,
                            CreationDate = DateTime.Now,
                            Number = "VIN001",
                            Balance = 100000,
                        }
                    };

                    foreach (Account account in victorAccounts)
                    {
                        context.Accounts.Add(account);
                    }
                    //context.SaveChanges();
                }
            }
            context.SaveChanges();
        }
    }
}
