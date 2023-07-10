using System;
using System.ComponentModel.DataAnnotations;

namespace MyActualWebsite.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IllegalCharecters : ValidationAttribute
    {
        private char[] chars;
        public IllegalCharecters(params char[] charecters)
        {
            chars = charecters;
        }
        public IllegalCharecters()
        {
            chars = new char[] { ' ', '%', '$', '&', '!', '@', '*', '^','#','<','>','+','`','\'','|','{','}','?','\"','=',':' };
        }

        public override bool IsValid(object value)
        {
            if (!(value is string)) return false;
            string s = (string)value;
            foreach (char c in chars) if (s.Contains(c)) return false;
            return true;
        }
    }
}

