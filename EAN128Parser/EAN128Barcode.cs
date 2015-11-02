using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Drkstr.EAN128
{
    public class EAN128Barcode
    {
        private List<ElementString> _elements;
        /// <summary>
        /// Recupera tutti gli elementi correnti
        /// </summary>
        public List<ElementString> Elements
        {
            get { return _elements; }
        }

        public EAN128Barcode()
        {
            _elements = new List<ElementString>();
        }

        /// <summary>
        /// Aggiunge un barcode al corrente EAN128
        /// </summary>
        /// <param name="rawData">Il dat grezzo del barcode</param>
        /// <exception cref="It.Daxo.Ean128.EAN128Exception" /> se si verifica un errore di parsing
        public void AddBarcode(byte[] rawData)
        {
            EAN128Tokenizer t = new EAN128Tokenizer(rawData);

            while (t.HasMoreToken())
            {
                Elements.Add(t.NextToken());
            }
        }

        /// <summary>
        /// Aggiunge un barcode al corrente EAN128
        /// </summary>
        /// <param name="asciiData">Il barcode decodificato in ASCII</param>
        public void AddBarcode(char[] asciiData)
        {
            EAN128Tokenizer t = new EAN128Tokenizer(asciiData);

            while (t.HasMoreToken())
            {
                Elements.Add(t.NextToken());
            }
        }

        /// <summary>
        /// Stampa il barcode contenuto nel formato (AI1)Data1(AI2)Data2(...
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string res = String.Empty;

            foreach (ElementString es in Elements)
            {
                res += es.ToString();
            }

            return res;
        }
    }
}
