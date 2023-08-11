using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Controller
{
    [Route("api")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;

        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        [HttpGet]

        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts();

                var accountsDTO = new List<AccountDTO>();

                foreach (Account account in accounts)
                {
                    AccountDTO newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = DateTime.Now,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            Date = tr.Date,
                        }).ToList()
                    };

                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[controller]/{id}")]

        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);

                if (account == null)
                {
                    return NotFound();
                }

                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = DateTime.Now,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(tr => new TransactionDTO
                    {
                        Id = tr.Id,
                        Type = tr.Type,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        Date = tr.Date,
                    }).ToList()
                };
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet ("clients/current/accounts")]
        public IActionResult GetCurrentAccounts()
        {
            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
            if (email == string.Empty)
            {
                return Forbid();
            }

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                return Forbid();
            }

            var currentAccounts = _accountRepository.GetAccountsByClient(client.Id);
            return Ok(currentAccounts);
        }

        [HttpPost("clients/current/accounts")]
        
        public IActionResult PostNewAccounts()
        {
            string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
            if (email == string.Empty)
            {
                return Forbid();
            }

            Client client = _clientRepository.FindByEmail(email);

            if (client == null)
            {
                return Forbid();
            }

            if (client.Accounts.Count >= 3) 
            {
                return Forbid();
            }

            var random = new Random();
            var account = new Account
            {
                ClientId = client.Id,
                Number = "VIN-" + random.Next(100000, 1000000).ToString(),
                CreationDate = DateTime.Now,
                Balance = 0,
            };
            
            _accountRepository.Save(account);
            
            return Created("", account);
        }

        [HttpPost]

        public AccountDTO PostNewClientNewAccount(long clientId)
        {
            try
            {
                var random = new Random();
                var newAccount = new Account
                {
                    ClientId = clientId,
                    Number = "VIN-" + random.Next(100000, 1000000).ToString(),
                    CreationDate = DateTime.Now,
                    Balance = 0,
                };
                _accountRepository.Save(newAccount);
                AccountDTO newAccountDTO = new AccountDTO()
                {
                    Id = newAccount.Id,
                    Balance = newAccount.Balance,
                    CreationDate = newAccount.CreationDate,
                    Number = newAccount.Number,
                };
                return newAccountDTO;
            }
            catch 
            {
                return null;
            }
        }
    }
}