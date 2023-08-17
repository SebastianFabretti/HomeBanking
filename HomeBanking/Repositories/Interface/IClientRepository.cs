﻿using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories.Interface
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
        Client FindByEmail(string email);
        bool ValidatePassword(string password);
    }
}