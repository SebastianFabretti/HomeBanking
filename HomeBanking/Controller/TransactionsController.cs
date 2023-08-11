using HomeBanking.Repositories.Interface;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeBanking.DTOS;
using HomeBanking.Models;
using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Security.Principal;

namespace HomeBanking.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return Forbid("No hay email");
                }
                Client client = _clientRepository.FindByEmail(email);

                if (client == null)
                {
                    return Forbid("Cliente inexistente");
                }

                if (transferDTO.FromAccountNumber == string.Empty || transferDTO.ToAccountNumber == string.Empty)
                {
                    return Forbid("Falto la cuenta de origen o de destino"); 
                }

                if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
                {
                    return Forbid("Transferencia invalida, estas transfiriendo dinero de una cuenta a esa misma cuenta");
                }

                if (transferDTO.Amount == 0 || transferDTO.Description == string.Empty)
                {
                    return Forbid("Falto el monto o la descripcion de la transferencia");
                }

                Account fromAccount = _accountRepository.FindByAccountNumber(transferDTO.FromAccountNumber);
                
                if (fromAccount == null)
                {
                    return Forbid("Cuenta de origen no existe");
                }

                if (transferDTO.Amount > fromAccount.Balance)
                {
                    return Forbid("Fondos insuficientes");
                }

                Account toAccount = _accountRepository.FindByAccountNumber(transferDTO.ToAccountNumber);
                
                if (toAccount == null)
                {
                    return Forbid("Cuenta de destino no existente");
                }

               _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.DEBIT.ToString(),
                    Amount = transferDTO.Amount * -1,
                    Description = transferDTO.Description + " " + toAccount.Number,
                    AccountId = fromAccount.Id,
                    Date = DateTime.Now,
                });

                
                _transactionRepository.Save(new Transaction
                {
                    Type = TransactionType.CREDIT.ToString(),
                    Amount = transferDTO.Amount,
                    Description = transferDTO.Description + " " + fromAccount.Number,
                    AccountId = toAccount.Id,
                    Date = DateTime.Now,
                });
                
                fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;                
                _accountRepository.Save(fromAccount);               
                toAccount.Balance = toAccount.Balance + transferDTO.Amount;                
                _accountRepository.Save(toAccount);

                return Created("Creado con exito", fromAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }
        }
    }
}
