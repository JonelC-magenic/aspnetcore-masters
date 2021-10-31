using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.CustomAttribute
{
    public enum PasswordScore
    {
        Blank = 0,
        VeryWeak = 1,
        Weak = 2,
        Medium = 3,
        Strong = 4,
        VeryStrong = 5
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class PasswordStrengthAttribute : ValidationAttribute
    {
        readonly PasswordScore _minScore;

        public PasswordScore MinScore
        {
            get { return _minScore; }
        }

        public PasswordStrengthAttribute(PasswordScore minScore = PasswordScore.Medium)
        {
            _minScore = minScore;
        }


        public override bool IsValid(object value)
        {
            var password = (String)value;
            var score = CheckPasswordScore(password);

            // Validate score if is in minimum
            if ((int)score >= (int)_minScore)
                return true;
            else
                return false;
        }

        // Checks password score
        private PasswordScore CheckPasswordScore(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            // Check length greater than 8
            if (password.Length >= 8)
                score++;

            // Check length greater than 12
            if (password.Length >= 12)
                score++;

            // Check if has number/s
            if (password.Any(c => char.IsDigit(c)))
                score++;

            // Check if has lower or upper case characters
            if (password.Any(c => char.IsUpper(c))
                && password.Any(c => char.IsLower(c)))
                score++;

            // Check if has special characters
            if (password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1)
                score++;

            return (PasswordScore)score;
        }

        // Format custom message
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              "{0} does not meet minimum requirements - {1}", name, this.MinScore.ToString());
        }
    }
}
