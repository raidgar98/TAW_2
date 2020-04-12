using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Andek
{
    class PasswordEnchanter
    {
        private struct pair { public char c1; public char c2; public pair(char a1, char a2) { c1 = a1; c2 = a2; } };
        private pair[] translator = { new pair('1', '!'), new pair('a', '@'), new pair('3', 'E'), new pair('O', '0'), new pair('o', '0'), new pair('Q', 'O'), new pair('B', '8'), new pair('\'', '`'), new pair('l', '!'), new pair('S', '$') };

        private string input;
        private bool small_letters;
        private bool capital_letters;
        private bool digits;
        private bool ambigous_characters;

        public PasswordEnchanter( string _input, bool _small_letters, bool _capital_letters, bool _digits, bool _ambigous_characters)
        {
            input = _input;
            small_letters = _small_letters;
            capital_letters = _capital_letters;
            digits = _digits;
            ambigous_characters = _ambigous_characters;
        }

        public string generate_pass()
        {

            string output = input;

            output = handle_whitespaces(output);
            output = repair_capitalization(output);

            output = capitalize(output);
            output = ambigouise(output);

            output = deambigouise(output);
            output = dedigitalize(output);

            return output;
        }

        string capitalize(string var)
        {
            if (!capital_letters) return var;

            string data = "";
            for (int i = 0; i < var.Length; i++)
                if (new System.Random().Next(0, 100) < 50)
                    data += char.ToUpper(var[i]);
                else
                    if (!small_letters)
                    data += char.ToUpper(var[i]);
                else
                    data += var[i];
            return data;
        }

        string handle_whitespaces(string var)
        {
            string data = "";
            foreach (char x in var)
                if (char.IsWhiteSpace(x)) { if (ambigous_characters) data += "_"; } else data += x;
            return data;
        }

        string repair_capitalization(string var)
        {
            string data = "";
            foreach (char x in var)
            {
                if (char.IsLetter(x))
                {
                    if (!small_letters && !capital_letters)
                        data += repair(x);
                    else if (!capital_letters)
                        data += char.ToLower(x);
                    else if (!small_letters)
                        data += char.ToUpper(x);
                    else data += x;
                }
                else data += x;
            }
            return data;
        }

        string dedigitalize(string var)
        {
            if (digits) return var;
            string data = "";
            for (int i = 0; i < var.Length; i++)
                if (char.IsDigit(var[i])) data += repair(var[i]);
                else data += var[i];
            return data;
        }

        string ambigouise(string var)
        {
            if (!ambigous_characters) return var;
            string data = "";
            for (int i = 0; i < var.Length; i++)
                if (new System.Random().Next(0, 100) < 75)
                    data += get_alternative_character(var[i]);
                else data += var[i];
            return data;
        }

        string deambigouise(string var)
        {
            if (ambigous_characters) return var;
            string data = "";
            for (int i = 0; i < var.Length; i++)
                if (!char.IsLetterOrDigit(var[i])) data += repair(var[i]);
                else data += var[i];
            return data;
        }

        char get_alternative_character(char c)
        {
            foreach (pair p in translator)
                if (p.c1 == c)
                    return p.c2;
                else if (p.c2 == c)
                    return p.c1;
            return c;
        }

        char random_allowed_sign()
        {
            char[] ambi = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', ',', '.', '<', '>', '/', '?', '"', '\'', '\\', ';', ':', ']', '}', '{', '[' };
            List<char> results = new List<char>();
            if (small_letters) results.Add((char)new System.Random().Next(97, 123));
            if (capital_letters) results.Add((char)new System.Random().Next(65, 91));
            if (digits) results.Add((char)new System.Random().Next(48, 58));
            if (ambigous_characters) results.Add((char)ambi[new System.Random().Next(0, ambi.Length)]);
            return results[new System.Random().Next(0, results.Count)];
        }

        char repair(char c)
        {
            //first try to get alternative
            char x = get_alternative_character(c);
            if (x != c) return x;

            //if no hard replace
            return random_allowed_sign();
        }
    }
}