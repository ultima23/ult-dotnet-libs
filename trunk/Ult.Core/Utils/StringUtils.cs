using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ult.Util
{
    public class StringUtils
    {

        /// <summary>
        /// Capitalize the firs letter of a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Custom implementation of split that doesn't consider separator if included between in double quotes "
        /// </summary>
        /// <param name="source">Source string to split</param>
        /// <param name="separator">Separator to use to split the string</param>
        /// <returns>An array containing the list of the string splitted by the separator</returns>
        private string[] Split(string source, string separator)
        {
            int index = 0;
            bool in_string = false;
            string token = string.Empty;
            List<string> tokens = new List<string>();
            // Scorro tutta la stringa
            while (index < source.Length)
            {
                // Cerco terminatore stringa, non preceduto dal back slash \
                if (source.Substring(index, 1) == "\"" && (index == 0 || source.Substring(index - 1, 1) != "\\"))
                {
                    in_string = true;
                }
                // Cerco il separatore dal punto in cui sono arrivato se non sono dentro a una stringa
                if (!in_string)
                {
                    // verifica se sto sforando la stringa
                    if (index + separator.Length < source.Length)
                    {
                        // Recupero il token da comparare col separatore, verifica CASE SENSITIVE
                        string t = source.Substring(index, separator.Length);
                        if (t.Equals(separator, StringComparison.InvariantCulture))
                        {
                            // aggiungo il token alla lista (se non è vuoto)
                            if (token.Length > 0) tokens.Add(token);
                            token = string.Empty;
                            // incremento indice per saltare il separatore
                            index = index + separator.Length;
                        }
                        else
                        {
                            // aggiungo carattere corrente
                            token += source.Substring(index, 1);
                            index++;
                        }
                    }
                    else
                    {
                        // aggiungo tutta la stringa rimanente
                        token += source.Substring(index);
                        index = source.Length;
                    }
                }
                else
                {
                    // parto a cercare dal carattere successivo ai doppi apici
                    int s_index = index + 1;
                    // Cerco la chiusura della stringa
                    while (s_index < source.Length)
                    {
                        // Cerco terminatore stringa, non preceduto dal back slash \
                        if (source.Substring(s_index, 1) == "\"" && source.Substring(s_index - 1, 1) != "\\")
                        {
                            in_string = false;
                            break;
                        }
                        s_index++;
                    }
                    // Se non sono più in stringa, aggiungo tutta la stringa al token e continuo saltandola per intero
                    // altrimenti forzo non in stringa e considero " come un carattere e non la definizione di una stringa
                    if (!in_string)
                    {
                        // Aggiungo carattere corrente e incremento indice
                        token += source.Substring(index, s_index - index + 1);
                        // Vado al prossimo carattere
                        index = s_index + 1;
                    }
                    else
                    {
                        // Forzo non in stringa, apertore senza terminatore
                        in_string = false;
                        // aggiungo carattere corrente e incremento indice
                        token += source.Substring(index, 1);
                        index++;
                    }
                }
            }
            // Se non ho chiuso un token, chiudo e lo aggiungo alla lista
            if (token.Length > 0) tokens.Add(token);
            // ritorno i token trovati
            return tokens.ToArray();
        }

    }
}
