using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASPNetCoreMastersTodoList.Api.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class CheckNumberWordsAttribute : ValidationAttribute
    {
        readonly int _minLength;

        public int MinLength
        {
            get { return _minLength; }
        }

        public CheckNumberWordsAttribute(int minLength = 3)
        {
            _minLength = minLength;
        }


        public override bool IsValid(object value)
        {
            var text = (String)value;
            var len = CheckNumberWords(text);

            // Validate length if is in minimum
            if (len >= _minLength)
                return true;
            else
                return false;
        }

        // Checks number of words
        private int CheckNumberWords(string text)
        {
            int length = 0;

            if (!string.IsNullOrEmpty(text))
            {
                var words = text.Split(' ');
                length = words.Count();
            }

            return length;
        }

        // Format custom message
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
              "{0} does not meet minimum number of words - {1}", name, this.MinLength);
        }
    }
}
