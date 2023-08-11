using HomeBanking.DTOS;
using HomeBanking.Models;
using HomeBanking.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Controller
{
    [Route("api")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardRepository _cardRepository;
        private readonly IClientRepository _clientRepository;

        public CardsController(ICardRepository cardRepository, IClientRepository clientRepository)
        {
            _cardRepository = cardRepository;
            _clientRepository = clientRepository;
        }

        [HttpPost("clients/current/cards")]
        public IActionResult PostCard([FromBody] Card card)
        {
            try
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

                Random random = new Random();

                int FourRandomCardNumbers()
                {
                    return random.Next(1000, 10000);
                }

                int totalCards = client.Cards.Where(cl => cl.Type == card.Type).Count();
                if (totalCards >= 3)
                {
                    return StatusCode(403, "Usted alcanzo el maximo de tarjetas disponibles");
                }

                var newCard = new Card
                {
                    ClientId = client.Id,
                    CardHolder = client.FirstName + client.LastName,
                    Type = card.Type,
                    Color = card.Color,                    
                    FromDate = DateTime.Now,
                    ThruDate = DateTime.Now.AddYears(6),
                    Number = $"{FourRandomCardNumbers()}-{FourRandomCardNumbers()}-{FourRandomCardNumbers()}-{FourRandomCardNumbers()}",
                    Cvv = random.Next(100, 1000),
                };

                _cardRepository.Save(newCard);


                return Created("", newCard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
