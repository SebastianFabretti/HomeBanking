using System;
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
                        FirstName="Victor",
                        LastName="Coronado",
                        Email = "vcoronado@gmail.com",
                        Password="123456"
                    },

                    new Client
                    {
                        FirstName="Seba",
                        LastName="Fabre",
                        Email = "seba@gmail.com",
                        Password="1111"
                    },

                    new Client
                    {
                        FirstName="Lionel",
                        LastName="Messi",
                        Email = "messi@gmail.com",
                        Password="101010"
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
                    context.SaveChanges();
                }               
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
                    context.SaveChanges();
                }              
            }

            if (!context.Accounts.Any())
            {
                var accountMessi = context.Clients.FirstOrDefault(c => c.Email == "messi@gmail.com");

                if (accountMessi != null)
                {
                    Account[] sebaAccounts = new Account[]
                    {
                        new Account
                        {
                            ClientId = accountMessi.Id,
                            CreationDate = DateTime.Now,
                            Number = "VIN003",
                            Balance = 99999999,
                        }
                    };

                    foreach (Account account in sebaAccounts)
                    {
                        context.Accounts.Add(account);
                    }
                }
                context.SaveChanges();
            }

            if (!context.Transactions.Any())
            {
                var account1 = context.Accounts.FirstOrDefault(c => c.Number == "VIN001");

                if (account1 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = 10000,
                            Date = DateTime.Now.AddHours(-5),
                            Description = "Transferencia Recibida",
                            Type = TransactionType.CREDIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = -2000,
                            Date = DateTime.Now.AddHours(-6),
                            Description = "Compra en la tienda de mercado libre",
                            Type = TransactionType.DEBIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account1.Id,
                            Amount = -3000,
                            Date = DateTime.Now.AddHours(-7),
                            Description = "Compra en carrefour",
                            Type = TransactionType.DEBIT.ToString()
                        },
                    };
                   
                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }       
            }

            if (!context.Transactions.Any())
            {
                var account2 = context.Accounts.FirstOrDefault(c => c.Number == "VIN002");

                if (account2 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction
                        {
                            AccountId = account2.Id,
                            Amount = 5000,
                            Date = DateTime.Now.AddHours(-35),
                            Description = "Transferencia Recibida",
                            Type = TransactionType.CREDIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account2.Id,
                            Amount = -20000,
                            Date = DateTime.Now.AddHours(-16),
                            Description = "Compra en la tienda de levis",
                            Type = TransactionType.DEBIT.ToString()
                        },
                        new Transaction
                        {
                            AccountId = account2.Id,
                            Amount = -30000,
                            Date = DateTime.Now.AddHours(-1),
                            Description = "Compra en cerveceria gluck",
                            Type = TransactionType.DEBIT.ToString()
                        },
                    };

                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }
            }
            
            if (!context.Transactions.Any())
            {
                var account3 = context.Accounts.FirstOrDefault(c => c.Number == "VIN003");

                if (account3 != null)
                {
                    var transactions = new Transaction[]
                    {
                        new Transaction
                        {
                            AccountId = account3.Id,
                            Amount = 1000000,
                            Date = DateTime.Now.AddHours(-35),
                            Description = "Deposito sueldo del Inter Miami",
                            Type = TransactionType.CREDIT.ToString()
                        },
                    };

                    foreach (Transaction transaction in transactions)
                    {
                        context.Transactions.Add(transaction);
                    }
                    context.SaveChanges();
                }                             
            }

            if (!context.Loans.Any())
            {                
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };

                foreach (Loan loan in loans)
                {
                    context.Loans.Add(loan);
                }

                context.SaveChanges();

                var client1 = context.Clients.FirstOrDefault(c => c.Email == "vcoronado@gmail.com");
                if (client1 != null)
                {
                    
                    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
                    if (loan1 != null)
                    {
                        var clientLoan1 = new ClientLoan
                        {
                            Amount = 400000,
                            ClientId = client1.Id,
                            LoanId = loan1.Id,
                            Payments = "60"
                        };
                        context.ClientLoans.Add(clientLoan1);
                    }

                    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
                    if (loan2 != null)
                    {
                        var clientLoan2 = new ClientLoan
                        {
                            Amount = 50000,
                            ClientId = client1.Id,
                            LoanId = loan2.Id,
                            Payments = "12"
                        };
                        context.ClientLoans.Add(clientLoan2);
                    }

                    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
                    if (loan3 != null)
                    {
                        var clientLoan3 = new ClientLoan
                        {
                            Amount = 100000,
                            ClientId = client1.Id,
                            LoanId = loan3.Id,
                            Payments = "24"
                        };
                        context.ClientLoans.Add(clientLoan3);
                    }                    
                    context.SaveChanges();
                }
            }
        }
    }
}
