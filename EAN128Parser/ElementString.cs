using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Drkstr.EAN128
{
    public class ElementString
    {
        private ElementStringFormat _format;
        public ElementStringFormat Format
        {
            get { return _format; }
            set
            {
                _format = value;
                _ai = value.AI;

                if (_format.NumberOfDecimal >= 0)
                {
                    _ai += Format.NumberOfDecimal;
                }
            }
        }

        private string _ai;
        public string AI
        {
            get
            {
                return _ai;
            }
            set
            {
                _ai = value;
            }
        }

        public string Description
        {
            get
            {
                return _format.Description;
            }
        }

        private string _data;
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public ElementString(ElementStringFormat esf)
        {
            Format = esf;
            Data = "";
        }

        /// <summary>
        /// Esegue il parsing di Data e restituisce il valore in DateTime
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Drkstr.EAN128.EAN128Exception" /> Se il valore dei dati non corrisponde a una data
        public DateTime ParseDate()
        {
            if (GS1Algorithm.StringEquals(_format.AI, new string[] { "11", "12", "13", "15", "17" }))
            {
                int y = Convert.ToInt32("20" + Data.Substring(0, 2));
                int m = Convert.ToInt32(Data.Substring(2, 2));
                int d = Convert.ToInt32(Data.Substring(4, 2));

                if (d == 0) d = 1; // If first day not specified

                return new DateTime(y, m, d);
            }
            else
            {
                throw new EAN128Exception("AI("+_format.AI+") is not a date");
            }
        }

        /// <summary>
        /// Esegue il parsing di Data e restituisce il valore intero
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Drkstr.EAN128.EAN128Exception" /> Se il valore dei dati non corrisponde a un intero
        public int ParseInt()
        {
            return 0;
        }

        /// <summary>
        /// Esegue il parsing di Data e restituisce il valore in double
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Drkstr.EAN128.EAN128Exception" /> Se il valore dei dati non corrisponde a un double
        public double ParseDouble()
        {
            return 0D;
        }

        public override string ToString()
        {
            string ai = AI;

            if (Format.NumberOfDecimal >= 0)
            {
                ai += Format.NumberOfDecimal;
            }

            return String.Format("({0}){1}", ai, Data);
        }
    }
}
