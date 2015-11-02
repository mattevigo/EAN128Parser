using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Drkstr.EAN128
{
    public enum CharType { ALPHANUMERIC, NUMERIC }

    public struct ElementStringFormat : ICloneable
    {
        /// <summary>
        /// Application Identifier
        /// </summary>
        public string AI { get; set; }

        /// <summary>
        /// Tipo di dato (Alfanumerico o Numerico)
        /// </summary>
        public CharType DataType { get; set; }

        /// <summary>
        /// Lunghezza dei dati (fissa o variabile)
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// Indica se è necessario il carattere FNC1 per delimitare l'ElementString
        /// </summary>
        public bool FNC1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Numero di cifre decimali nel campo data (solo se numerico)
        /// </summary>
        public int NumberOfDecimal { get; set; }
        //{
        //    get
        //    {
        //        if (AI.Length > 4)
        //        {
        //            return Convert.ToInt32(AI.Substring(3));
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}

        public override string ToString()
        {
            return AI + "-" + Description;
        }

        public static ElementStringFormat GetFormat(string applicationIdentifier, CharType dataType, int dataLength, bool fnc1, string description)
        {
            ElementStringFormat format = new ElementStringFormat();

            format.AI = applicationIdentifier;
            format.DataType = dataType;
            format.DataLength = dataLength;
            format.FNC1 = fnc1;
            format.Description = description;
            format.NumberOfDecimal = -1;

            return format;
        }

        public static ElementStringFormat GetFormat(string applicationIdentifier, CharType dataType, int dataLength, bool fnc1, string description, int numberOfDecimals)
        {
            ElementStringFormat format = ElementStringFormat.GetFormat(applicationIdentifier, dataType, dataLength, fnc1, description);
            format.NumberOfDecimal = numberOfDecimals;

            return format;
        }

        public Object Clone()
        {
            return ElementStringFormat.GetFormat(this.AI, this.DataType, this.DataLength, this.FNC1, this.Description);
        }
    }
}
