using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HomeBanking.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        private readonly ITransactionRepository _transactionRepository;

        public LoansController(IClientRepository clientRepository, IAccountRepository accountRepository, ILoanRepository loanRepository,
        IClientLoanRepository clientLoanRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
            _transactionRepository = transactionRepository;

        }

        [HttpGet]
        
        public IActionResult GetLoans()
        {
            try
            {
                var loans = _loanRepository.GetAllLoans();
                var loansDTO = new List<LoanDTO>();
                
                foreach (Loan loan in loans) 
                {
                    LoanDTO newLoanDTO = new LoanDTO
                    {
                        Id = loan.Id,
                        Name = loan.Name,
                        MaxAmount = loan.MaxAmount,
                        Payments = loan.Payments,
                    };

                    loansDTO.Add(newLoanDTO);
                }
                return Ok(loansDTO);
            }

            catch(Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }
                    
        [HttpPost]

        public IActionResult Post(LoanApplicationDTO loanApplicationDTO)
        {
            try
            {
                const int loadInterestRate = 20;

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

                var loan = _loanRepository.FindById(loanApplicationDTO.LoanId);

                if (loan == null)
                {
                    return StatusCode(403, "Prestamo no encontrado");
                }

                if (loanApplicationDTO.Amount > loan.MaxAmount)
                {
                    return StatusCode(403, "El prestamo que usted esta pidiendo es mayor al maximo permitido");
                }

                if (1 > loanApplicationDTO.Amount)
                {
                    return StatusCode(403, "Usted puso un monto invalido");
                }

                if (loanApplicationDTO.Payments == null)
                {
                    return StatusCode(403, "Usted no puso en cuantas cuotas va a pagar el prestamo");
                }

                var account = _accountRepository.FindByAccountNumber(loanApplicationDTO.ToAccountNumber);                

                if (account == null)
                {
                    return StatusCode(403, "La cuenta seleccionada es inexistente");
                }

                var clientLoan = new ClientLoan
                {
                    ClientId = client.Id,
                    Amount = loanApplicationDTO.Amount,
                    Payments = loanApplicationDTO.Payments,
                    LoanId = loanApplicationDTO.LoanId,
                };

                var newTransaction = new Transaction
                {
                    AccountId = account.Id,
                    Amount = loanApplicationDTO.Amount,
                    Type = CardType.CREDIT.ToString(),
                    Date = DateTime.Now,
                    Description = "Prestamo"
                };

                account.Balance = account.Balance + clientLoan.Amount;
                clientLoan.Amount = clientLoan.Amount * loadInterestRate;

                _transactionRepository.Save(newTransaction);
                _accountRepository.Save(account);
                _clientLoanRepository.Save(clientLoan);
             
                return Created("", clientLoan); //Transaccion no se guarda, chequear si hay un problema en la logica o en el front.                
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
