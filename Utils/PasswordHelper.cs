﻿using Microsoft.AspNetCore.Identity;

namespace DotNetCardsServer.Utils
{
    public static class PasswordHelper
    {
        private static readonly PasswordHasher<object> passwordHasher = new PasswordHasher<object>();

        public static string GeneratePassword(string password)
        {
            return passwordHasher.HashPassword(null, password);
        }
        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword)
                   != PasswordVerificationResult.Failed;
        }

    }
}
