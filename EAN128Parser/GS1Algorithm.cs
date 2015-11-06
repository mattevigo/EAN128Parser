using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Drkstr.EAN128
{
    public enum GS1State
        {
            E,  // Ingresso con stringa vuota
            L2, // Possibile uscita di lunghezza 2
            L3, // Possibile uscita di lunghezza 3
            L4, // Uscita di lunghezza 4
            EXT // Uscita (parsing dei dati)
        }

    public class GS1Algorithm
    {
        private static char[] FNC1Values = new char[] { (char)29, (char)32 };

        private string _ai;
        /// <summary>
        /// Recupera l'Application Identifier corrente
        /// </summary>
        public string AI { get { return _ai; } }

        private GS1State _state;
        /// <summary>
        /// Recupera lo stato corrente dell'algoritmo
        /// </summary>
        public GS1State State { get { return _state; } }

        private char[] _barcode;
        /// <summary>
        /// Recupera il barcode per cui l'algoritmo è stato istanziato
        /// </summary>
        public char[] Barcode 
        {
            get { return _barcode; }
        }

        public int Index { get; set; }

        /// <summary>
        /// Inizializza un nuovo algoritmo per il parsing di uno specifico barcode
        /// </summary>
        /// <param name="barcode"></param>
        public GS1Algorithm(char[] barcode)
        {
            _ai = "";
            _state = GS1State.E;
            Index = 0;

            _barcode = barcode;
        }

        /// <summary>
        /// Esegue il reset dell'avanzamento del corrente algoritmo ma non cambia il barcode
        /// N.B.: per utilizzare l'algoritmo su un altro barcode è necessario istanziarne uno nuovo
        /// </summary>
        public void ResetAI()
        {
            _ai = "";
            _state = GS1State.E;
        }

        /// <summary>
        /// Recupera il prossimo formato a partire dal primo carattere di Barcode
        /// con indice Index ed avanza l'indice alla prima possizione utile per i dati
        /// </summary>
        /// <returns>Il corrispondente ElementStringFormat</returns>
        /// <exception cref="Drkstr.EAN128.GS1AlgorithmException" /> se si è verificato un errore di parsing
        public ElementStringFormat NextFormat()
        {
            try
            {
                while (this._state != GS1State.EXT)
                {
                    SetNextChar(Barcode[Index++]);
                }

                try
                {
                    ElementStringFormat format = (ElementStringFormat)EAN128Tokenizer.Formats[AI].Clone();

                    // Gestione numero di cifre decimali
                    if (StringEquals(AI.Substring(0, 2), new string[] { "31", "32", "33", "34", "35", "36", "39" }))
                    {
                        // Si recupera il numero di cifre decimali e si avanza l'indice al carattere successivo
                        int nod = Convert.ToInt32(Barcode[Index++].ToString());
                        format.NumberOfDecimal = nod;
                    }

                    return format;
                }
                catch (KeyNotFoundException)
                {
                    throw new GS1AlgorithmException("AI not found: " + AI);
                }
            }
            catch (Exception)
            {
                throw new GS1AlgorithmException("Error occured while parsing format");
            }
        }

        /// <summary>
        /// Fa avanzare l'algoritmo per il riconoscimento dell'AI
        /// </summary>
        /// <param name="next">Il carattere successivo</param>
        /// <returns></returns>
        /// <exception cref="Drkstr.GS1AlgorithmException" /> in caso di errore
        private GS1Algorithm SetNextChar(char next)
        {
            if (State == GS1State.E)
            {
                _state = GS1State.L2;
                _ai = _ai + next;
            }
            else if (State == GS1State.L2)
            {
                if (AI.Equals("0"))
                {
                    if (next == '0' || next == '1' || next == '2')
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (AI.Equals("1"))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2', '3', '5', '7' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (AI.Equals("2"))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else if (CharEquals(next, new char[] { '4', '5' }))
                    {
                        _state = GS1State.L3;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (AI.Equals("3"))
                {
                    if (CharEquals(next, new char[] { '0', '7' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else if (CharEquals(next, new char[] { '0', '1', '2', '3', '4', '5', '6', '9' }))
                    {
                        _state = GS1State.L3;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (StringEquals(AI, new string[] { "4", "7", "8" }))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }))
                    {
                        _state = GS1State.L2;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (AI.Equals("9"))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (StringEquals(AI, new string[] { "40", "41", "42" }))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (StringEquals(AI, new string[] { "70", "80", "81", "82" }))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }))
                    {
                        _state = GS1State.L4;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
            }
            else if (State == GS1State.L3)
            {
                if (AI.Equals("24"))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (AI.Equals("25"))
                {
                    if (CharEquals(next, new char[] { '0', '1' , '3', '4'}))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (AI.Equals("25"))
                {
                    if (CharEquals(next, new char[] { '0', '1', '3', '5' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
                else if (StringEquals(AI, new string[] { "31", "32", "33", "34", "35", "36", "39" }))
                {
                    if (CharEquals(next, new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }))
                    {
                        _state = GS1State.EXT;
                        _ai = _ai + next;
                    }
                    else
                    {
                        throw new GS1AlgorithmException(Index);
                    }
                }
            }
            else if (State == GS1State.L4)
            {
                if(CharEquals(next, new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 's' }))
                {
                    _state = GS1State.EXT;
                    _ai = _ai + next;
                }
                else
                {
                    throw new GS1AlgorithmException(Index);
                }
            }

            return this;
        }

        /// <summary>
        /// Restituisce il successivo ElementString a partire dall'indice corrente dell'algoritmo
        /// </summary>
        /// <param name="format">Il formato dell'ElementString da parsare</param>
        /// <returns></returns>
        /// <exception cref="Drkstr.GS1AlgorithmException" /> in caso di errore
        public ElementString NextElementWithFormat(ElementStringFormat format)
        {
            ElementString element = new ElementString(format);

            try
            {
                if (format.FNC1 == true) // Lunghezza variabile
                {
                    int startIndex = Index;

                    while (Index < Barcode.Length && (Index - startIndex) <= format.DataLength)
                    {
                        if (CharIsFNC1(Barcode[Index]))
                        {
                            // Carattere di escape: il token e' pronto e si avanza l'indice di uno
                            Index++;
                            break;
                        }
                        else
                        {
                            element.Data = element.Data + Barcode[Index];
                            Index++;
                        }
                    }
                }
                else // Lunghezza fissa
                {
                    for (int i = 0; i < element.Format.DataLength; i++)
                    {
                        element.Data = element.Data + Barcode[Index];
                        Index++;
                    }
                }

                return element;
            }
            catch (IndexOutOfRangeException)
            {
                throw new GS1AlgorithmException(String.Format("Unexpected end of format (AI={0})", AI));
            }
        }

        #region Utility

        /// <summary>
        /// Controlla se un carattere è uno uguale a uno dei caratteri specificati
        /// </summary>
        /// <param name="c"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool CharEquals(char c, char[] values)
        {
            foreach (char v in values)
            {
                if (c == v) return true;
            }

            return false;
        }

        /// <summary>
        /// Controlla se una stringa è uguale a una delle stringhe specificate
        /// </summary>
        /// <param name="s"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool StringEquals(string s, string[] values)
        {
            foreach (string v in values)
            {
                if (s.Equals(v)) return true;
            }

            return false;
        }

        /// <summary>
        /// Controlla se un dato carattere e' FNC1 (tra i valori ammessi)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool CharIsFNC1(char c)
        {
            return CharEquals(c, FNC1Values);
        }

        #endregion
    }

    public class GS1AlgorithmException : EAN128Exception
    {
        public GS1AlgorithmException() : base() {}

        public GS1AlgorithmException(int index) : base(String.Format("Parse error at index {0}", index)) { }

        public GS1AlgorithmException(string msg) : base(msg) { }
    }
}
