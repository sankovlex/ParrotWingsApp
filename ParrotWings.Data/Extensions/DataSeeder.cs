using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Storage;
using ParrotWings.Data.Core;
using ParrotWings.Models.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParrotWings.Data.Extensions
{
    public static class DataSeeder
    {
        public static void Seed(this EfContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {

                var user = new User
                {
                    Password = "W9Bc9zBj0RuNHWPMWIAmbaEpbzU/rN4LBg7tbEjvbwE=",
                    Salt = "1w/O24AgRC6LMsx7T/QkNw==",
                    Name = "Parrot Wings",
                    Email = "parrot@wings.app"
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
