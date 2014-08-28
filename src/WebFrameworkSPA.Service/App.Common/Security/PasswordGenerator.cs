using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Common.Security
{
    public class PasswordGenerator : IPasswordGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Random password that fulfills all DISA requirements and at least 14 characters in length. </returns>
        public virtual string GeneratePassword()
        {
            StringBuilder password = new StringBuilder();
            string upperCase = "ABCDEFGHJKLMNPQRSTUVWXYZ";	// avoid I and O because they look similar to other chars
            string lowerCase = "abcdefghijkmnpqrstuvwxyz";	// avoid l and o because they look similar to other chars
            string numbers = "23456789";					// avoid 0 and 1 because they look similar to other chars
            string specialChars = "!#$%^&*()_+";          //avoid @ to gurantee user's email would not be part of the password

            Random random = new Random();
            password.Append(upperCase[random.Next() % upperCase.Length]);
            password.Append(numbers[random.Next() % numbers.Length]);
            password.Append(specialChars[random.Next() % specialChars.Length]);
            password.Append(lowerCase[random.Next() % lowerCase.Length]);
            password.Append(upperCase[random.Next() % upperCase.Length]);
            password.Append(numbers[random.Next() % numbers.Length]);
            password.Append(specialChars[random.Next() % specialChars.Length]);
            password.Append(lowerCase[random.Next() % lowerCase.Length]);
            password.Append(upperCase[random.Next() % upperCase.Length]);
            password.Append(numbers[random.Next() % numbers.Length]);
            password.Append(specialChars[random.Next() % specialChars.Length]);
            password.Append(lowerCase[random.Next() % lowerCase.Length]);
            password.Append(upperCase[random.Next() % upperCase.Length]);
            password.Append(numbers[random.Next() % numbers.Length]);
            password.Append(specialChars[random.Next() % specialChars.Length]);
            password.Append(lowerCase[random.Next() % lowerCase.Length]);
            password.Append(upperCase[random.Next() % upperCase.Length]);

            return password.ToString();
        }
    }
}
