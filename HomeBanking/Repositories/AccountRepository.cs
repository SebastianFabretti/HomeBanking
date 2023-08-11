﻿using HomeBanking.Models;
using HomeBanking.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class AccountRepository: RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext) 
        { 
        }
        
        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id)
            .Include(account => account.Transactions)
            .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
            .Include(account => account.Transactions)
            .ToList();
        }

        public void Save(Account account)
        {
            if (account.Id == 0)
            {
                Create(account);
            }
            else
            {
                Update(account);
            }
            SaveChanges();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account=>account.ClientId == clientId)
            .Include(account=>account.Transactions)
            .ToList();
        }

        public Account FindByAccountNumber(string number)
        {
            return FindByCondition(acc => acc.Number.ToUpper() == number.ToUpper())
            .Include(account => account.Transactions)
            .FirstOrDefault();
        }
    }
}