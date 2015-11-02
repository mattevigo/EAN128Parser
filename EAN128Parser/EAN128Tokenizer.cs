using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Drkstr.EAN128
{
    public class EAN128Tokenizer
    {
        private static Dictionary<string, ElementStringFormat> _formats = null;
        public static Dictionary<string, ElementStringFormat> Formats
        {
            get
            {
                if (_formats == null)
                {
                    _formats = new Dictionary<string, ElementStringFormat>();
                    _formats.Add("00",      ElementStringFormat.GetFormat("00",     CharType.NUMERIC, 18, false, "SSCC"));
                    _formats.Add("01",      ElementStringFormat.GetFormat("01",     CharType.NUMERIC, 14, false, "GTIN"));
                    _formats.Add("02",      ElementStringFormat.GetFormat("02",     CharType.NUMERIC, 14, false, "CONTENT"));
                    _formats.Add("10",      ElementStringFormat.GetFormat("10",     CharType.ALPHANUMERIC, 20, true, "BATCH/LOT"));
                    _formats.Add("11",      ElementStringFormat.GetFormat("11",     CharType.ALPHANUMERIC, 6, false, "PROD DATE"));
                    _formats.Add("12",      ElementStringFormat.GetFormat("12",     CharType.NUMERIC, 6, false, "DUE DATE"));
                    _formats.Add("13",      ElementStringFormat.GetFormat("13",     CharType.NUMERIC, 6, false, "PACK DATE"));
                    _formats.Add("15",      ElementStringFormat.GetFormat("15",     CharType.NUMERIC, 6, false, "BEST BEFORE or SELL BY"));
                    _formats.Add("17",      ElementStringFormat.GetFormat("17",     CharType.NUMERIC, 6, false, "USE BY OR EXPIRY"));
                    _formats.Add("20",      ElementStringFormat.GetFormat("20",     CharType.NUMERIC, 2, false, "VARIANT"));
                    _formats.Add("21",      ElementStringFormat.GetFormat("21",     CharType.ALPHANUMERIC, 20, true, "SERIAL"));
                    _formats.Add("22",      ElementStringFormat.GetFormat("22",     CharType.ALPHANUMERIC, 29, true, "QTY/DATE/BATCH"));
                    _formats.Add("240",     ElementStringFormat.GetFormat("240",    CharType.ALPHANUMERIC, 30, true, "ADDITIONAL ID"));
                    _formats.Add("241",     ElementStringFormat.GetFormat("241",    CharType.ALPHANUMERIC, 30, true, "CUST. PART NO."));
                    _formats.Add("242",     ElementStringFormat.GetFormat("242",    CharType.NUMERIC, 6, true, "MTO VARIANT"));
                    _formats.Add("250",     ElementStringFormat.GetFormat("250",    CharType.ALPHANUMERIC, 30, true, "SECONDARY SERIAL"));
                    _formats.Add("251",     ElementStringFormat.GetFormat("251",    CharType.ALPHANUMERIC, 30, true, "REF. TO SOURCE"));
                    _formats.Add("253",     ElementStringFormat.GetFormat("253",    CharType.ALPHANUMERIC, 30, true, "DOC. ID"));
                    _formats.Add("254",     ElementStringFormat.GetFormat("254",    CharType.ALPHANUMERIC, 20, true, "GLN EXTENSION"));
                    _formats.Add("30",      ElementStringFormat.GetFormat("30",     CharType.NUMERIC, 8, true, "VAR. COUNT"));
                    _formats.Add("310",     ElementStringFormat.GetFormat("310",    CharType.NUMERIC, 6, false, "NET WEIGHT (kg)"));
                    _formats.Add("311",     ElementStringFormat.GetFormat("311",    CharType.NUMERIC, 6, false, "LENGTH (m)"));
                    _formats.Add("312",     ElementStringFormat.GetFormat("312",    CharType.NUMERIC, 6, false, "WIDTH (m)"));
                    _formats.Add("313",     ElementStringFormat.GetFormat("313",    CharType.NUMERIC, 6, false, "HEIGHT (m)"));
                    _formats.Add("314",     ElementStringFormat.GetFormat("314",    CharType.NUMERIC, 6, false, "AREA (m2)"));
                    _formats.Add("315",     ElementStringFormat.GetFormat("315",    CharType.NUMERIC, 6, false, "NET VOLUME (l)"));
                    _formats.Add("316",     ElementStringFormat.GetFormat("316",    CharType.NUMERIC, 6, false, "NET VOLUME (m3)"));
                    _formats.Add("320",     ElementStringFormat.GetFormat("320",    CharType.NUMERIC, 6, false, "NET WEIGHT (lb)"));
                    _formats.Add("321",     ElementStringFormat.GetFormat("321",    CharType.NUMERIC, 6, false, "LENGHT (i)"));
                    _formats.Add("322",     ElementStringFormat.GetFormat("322",    CharType.NUMERIC, 6, false, "LENGHT (f)"));
                    _formats.Add("323",     ElementStringFormat.GetFormat("323",    CharType.NUMERIC, 6, false, "LENGHT (y)"));
                    _formats.Add("324",     ElementStringFormat.GetFormat("324",    CharType.NUMERIC, 6, false, "WIDTH (i)"));
                    _formats.Add("325",     ElementStringFormat.GetFormat("325",    CharType.NUMERIC, 6, false, "WIDTH (f)"));
                    _formats.Add("326",     ElementStringFormat.GetFormat("326",    CharType.NUMERIC, 6, false, "WIDTH (y)"));
                    _formats.Add("327",     ElementStringFormat.GetFormat("327",    CharType.NUMERIC, 6, false, "HEIGHT (i)"));
                    _formats.Add("328",     ElementStringFormat.GetFormat("328",    CharType.NUMERIC, 6, false, "HEIGHT (f)"));
                    _formats.Add("329",     ElementStringFormat.GetFormat("329",    CharType.NUMERIC, 6, false, "HEIGHT (y)"));
                    _formats.Add("330",     ElementStringFormat.GetFormat("330",    CharType.NUMERIC, 6, false, "GROSS WEIGHT (kg)"));
                    _formats.Add("331",     ElementStringFormat.GetFormat("331",    CharType.NUMERIC, 6, false, "LENGTH (m), log"));
                    _formats.Add("332",     ElementStringFormat.GetFormat("332",    CharType.NUMERIC, 6, false, "WIDTH (m), log"));
                    _formats.Add("333",     ElementStringFormat.GetFormat("333",    CharType.NUMERIC, 6, false, "HEIGHT (m), log"));
                    _formats.Add("334",     ElementStringFormat.GetFormat("334",    CharType.NUMERIC, 6, false, "AREA (m2), log"));
                    _formats.Add("335",     ElementStringFormat.GetFormat("335",    CharType.NUMERIC, 6, false, "VOLUME (l), log"));
                    _formats.Add("336",     ElementStringFormat.GetFormat("336",    CharType.NUMERIC, 6, false, "VOLUME (m3), log"));
                    _formats.Add("337",     ElementStringFormat.GetFormat("337",    CharType.NUMERIC, 6, false, "KG PER m2"));
                    _formats.Add("340",     ElementStringFormat.GetFormat("340",    CharType.NUMERIC, 6, false, "GROSS WEIGHT (lb)"));
                    _formats.Add("341",     ElementStringFormat.GetFormat("341",    CharType.NUMERIC, 6, false, "LENGTH (i), log"));
                    _formats.Add("342",     ElementStringFormat.GetFormat("342",    CharType.NUMERIC, 6, false, "LENGTH (f), log"));
                    _formats.Add("343",     ElementStringFormat.GetFormat("343",    CharType.NUMERIC, 6, false, "LENGTH (y), log"));
                    _formats.Add("344",     ElementStringFormat.GetFormat("344",    CharType.NUMERIC, 6, false, "WIDTH (i), log"));
                    _formats.Add("345",     ElementStringFormat.GetFormat("345",    CharType.NUMERIC, 6, false, "WIDTH (f), log"));
                    _formats.Add("346",     ElementStringFormat.GetFormat("346",    CharType.NUMERIC, 6, false, "WIDTH (y), log"));
                    _formats.Add("347",     ElementStringFormat.GetFormat("347",    CharType.NUMERIC, 6, false, "HEIGHT (i), log"));
                    _formats.Add("348",     ElementStringFormat.GetFormat("348",    CharType.NUMERIC, 6, false, "HEIGHT (f), log"));
                    _formats.Add("349",     ElementStringFormat.GetFormat("349",    CharType.NUMERIC, 6, false, "HEIGHT (t), log"));
                    _formats.Add("350",     ElementStringFormat.GetFormat("350",    CharType.NUMERIC, 6, false, "AREA (i2)"));
                    _formats.Add("351",     ElementStringFormat.GetFormat("351",    CharType.NUMERIC, 6, false, "AREA (f2)"));
                    _formats.Add("352",     ElementStringFormat.GetFormat("352",    CharType.NUMERIC, 6, false, "AREA (y2)"));
                    _formats.Add("353",     ElementStringFormat.GetFormat("353",    CharType.NUMERIC, 6, false, "AREA (i2), log"));
                    _formats.Add("354",     ElementStringFormat.GetFormat("354",    CharType.NUMERIC, 6, false, "AREA (f2), log"));
                    _formats.Add("355",     ElementStringFormat.GetFormat("355",    CharType.NUMERIC, 6, false, "AREA (y2), log"));
                    _formats.Add("356",     ElementStringFormat.GetFormat("356",    CharType.NUMERIC, 6, false, "NET WEIGHT (t)"));
                    _formats.Add("357",     ElementStringFormat.GetFormat("357",    CharType.NUMERIC, 6, false, "NET VOLUME (oz)"));
                    _formats.Add("360",     ElementStringFormat.GetFormat("360",    CharType.NUMERIC, 6, false, "NET VOLUME (q)"));
                    _formats.Add("361",     ElementStringFormat.GetFormat("361",    CharType.NUMERIC, 6, false, "NET VOLUME (g)"));
                    _formats.Add("362",     ElementStringFormat.GetFormat("362",    CharType.NUMERIC, 6, false, "VOLUME (q), log"));
                    _formats.Add("363",     ElementStringFormat.GetFormat("363",    CharType.NUMERIC, 6, false, "VOLUME (g), log"));
                    _formats.Add("364",     ElementStringFormat.GetFormat("364",    CharType.NUMERIC, 6, false, "VOLUME (i3)"));
                    _formats.Add("365",     ElementStringFormat.GetFormat("365",    CharType.NUMERIC, 6, false, "VOLUME (f3)"));
                    _formats.Add("366",     ElementStringFormat.GetFormat("366",    CharType.NUMERIC, 6, false, "VOLUME (y3)"));
                    _formats.Add("367",     ElementStringFormat.GetFormat("367",    CharType.NUMERIC, 6, false, "VOLUME (i3), log"));
                    _formats.Add("368",     ElementStringFormat.GetFormat("368",    CharType.NUMERIC, 6, false, "VOLUME (f3), log"));
                    _formats.Add("369",     ElementStringFormat.GetFormat("369",    CharType.NUMERIC, 6, false, "VOLUME (y3), log"));
                    _formats.Add("37",      ElementStringFormat.GetFormat("37",     CharType.NUMERIC, 8, true, "COUNT"));
                    _formats.Add("390",     ElementStringFormat.GetFormat("390",    CharType.NUMERIC, 15, true, "AMOUNT"));
                    _formats.Add("391",     ElementStringFormat.GetFormat("391",    CharType.NUMERIC, 18, true, "AMOUNT"));
                    _formats.Add("392",     ElementStringFormat.GetFormat("392",    CharType.NUMERIC, 15, true, "PRICE"));
                    _formats.Add("393",     ElementStringFormat.GetFormat("393",    CharType.NUMERIC, 18, true, "PRICE"));
                    _formats.Add("400",     ElementStringFormat.GetFormat("400",    CharType.ALPHANUMERIC, 30, true, "ORDER NUMBER"));
                    _formats.Add("401",     ElementStringFormat.GetFormat("401",    CharType.ALPHANUMERIC, 30, true, "CONSIGNMENT"));
                    _formats.Add("402",     ElementStringFormat.GetFormat("402",    CharType.NUMERIC, 17, true, "SHIPMENT NO."));
                    _formats.Add("403",     ElementStringFormat.GetFormat("403",    CharType.ALPHANUMERIC, 30, true, "ROUTE"));
                    _formats.Add("410",     ElementStringFormat.GetFormat("410",    CharType.NUMERIC, 13, false, "SHIP TO LOC"));
                    _formats.Add("411",     ElementStringFormat.GetFormat("411",    CharType.NUMERIC, 13, false, "BILL TO"));
                    _formats.Add("412",     ElementStringFormat.GetFormat("412",    CharType.NUMERIC, 13, false, "PURCHASE FROM"));
                    _formats.Add("413",     ElementStringFormat.GetFormat("413",    CharType.NUMERIC, 13, false, "SHIP FOR LOC"));
                    _formats.Add("414",     ElementStringFormat.GetFormat("414",    CharType.NUMERIC, 13, false, "LOC No"));
                    _formats.Add("415",     ElementStringFormat.GetFormat("415",    CharType.NUMERIC, 13, false, "PAY TO"));
                    _formats.Add("420",     ElementStringFormat.GetFormat("420",    CharType.ALPHANUMERIC, 20, true, "SHIP TO POST"));
                    _formats.Add("421",     ElementStringFormat.GetFormat("421",    CharType.ALPHANUMERIC, 12, true, "SHIP TO POST"));
                    _formats.Add("422",     ElementStringFormat.GetFormat("422",    CharType.NUMERIC, 3, true, "ORIGIN"));
                    _formats.Add("423",     ElementStringFormat.GetFormat("423",    CharType.NUMERIC, 15, true, "COUNTRY-INITIAL PROCESS."));
                    _formats.Add("424",     ElementStringFormat.GetFormat("424",    CharType.NUMERIC, 3, true, "COUNTRY-PROCESS."));
                    _formats.Add("425",     ElementStringFormat.GetFormat("425",    CharType.NUMERIC, 3, true, "COUNTRY-DISASSEMBLY"));
                    _formats.Add("426",     ElementStringFormat.GetFormat("426",    CharType.NUMERIC, 3, true, "COUNTRY-FULL PROCESS"));
                    _formats.Add("7001",    ElementStringFormat.GetFormat("7001",   CharType.NUMERIC, 13, true, "NSN"));
                    _formats.Add("7002",    ElementStringFormat.GetFormat("7002",   CharType.ALPHANUMERIC, 30, true, "MEAT CUT"));
                    _formats.Add("7003",    ElementStringFormat.GetFormat("7003",   CharType.NUMERIC, 10, true, "EXPIRY TIME"));
                    _formats.Add("7004",    ElementStringFormat.GetFormat("7004",   CharType.NUMERIC, 4, true, "ACTIVE POTENCY"));
                    _formats.Add("703s",    ElementStringFormat.GetFormat("7004",   CharType.ALPHANUMERIC, 30, true, "PROCESSO # s"));
                    _formats.Add("8001",    ElementStringFormat.GetFormat("8001",   CharType.NUMERIC, 14, true, "DIMENSIONS"));
                    _formats.Add("8002",    ElementStringFormat.GetFormat("8002",   CharType.ALPHANUMERIC, 20, true, "CMT No"));
                    _formats.Add("8003",    ElementStringFormat.GetFormat("8003",   CharType.ALPHANUMERIC, 30, true, "GRAI"));
                    _formats.Add("8004",    ElementStringFormat.GetFormat("8004",   CharType.ALPHANUMERIC, 30, true, "GIAI"));
                    _formats.Add("8005",    ElementStringFormat.GetFormat("8005",   CharType.NUMERIC, 6, true, "PRICE PER UNIT"));
                    _formats.Add("8006",    ElementStringFormat.GetFormat("8006",   CharType.NUMERIC, 18, true, "GCTIN"));
                    _formats.Add("8007",    ElementStringFormat.GetFormat("8007",   CharType.ALPHANUMERIC, 30, true, "IBAN"));
                    _formats.Add("8008",    ElementStringFormat.GetFormat("8008",   CharType.NUMERIC, 12, true, "PROD TIME"));
                    _formats.Add("8018",    ElementStringFormat.GetFormat("8018",   CharType.NUMERIC, 18, true, "GSRN"));
                    _formats.Add("8020",    ElementStringFormat.GetFormat("8020",   CharType.ALPHANUMERIC, 25, true, "REF No"));
                    _formats.Add("8100",    ElementStringFormat.GetFormat("8100",   CharType.NUMERIC, 6, true, ""));
                    _formats.Add("8101",    ElementStringFormat.GetFormat("8101",   CharType.NUMERIC, 10, true, "-"));
                    _formats.Add("8102",    ElementStringFormat.GetFormat("8102",   CharType.NUMERIC, 2, true, "-"));
                    _formats.Add("8110",    ElementStringFormat.GetFormat("8110",   CharType.ALPHANUMERIC, 70, false, ""));
                    _formats.Add("8200",    ElementStringFormat.GetFormat("8200",   CharType.ALPHANUMERIC, 70, true, "PRODUCT URL"));
                    _formats.Add("90",      ElementStringFormat.GetFormat("90",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("91",      ElementStringFormat.GetFormat("91",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("92",      ElementStringFormat.GetFormat("92",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("93",      ElementStringFormat.GetFormat("93",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("94",      ElementStringFormat.GetFormat("94",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("95",      ElementStringFormat.GetFormat("95",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("96",      ElementStringFormat.GetFormat("96",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("97",      ElementStringFormat.GetFormat("97",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("98",      ElementStringFormat.GetFormat("98",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                    _formats.Add("99",      ElementStringFormat.GetFormat("99",     CharType.ALPHANUMERIC, 30, true, "INTERNAL"));
                }

                return _formats;
            }
        }

        private char[] _barcode;
        private GS1Algorithm _gs1Algorithm;

        /// <summary>
        /// Inizializza un tokenizer a partire dal dato grezzo contenuto nel barcode
        /// </summary>
        /// <param name="rowData"></param>
        public EAN128Tokenizer(byte[] rowData)
        {
            _barcode = Encoding.ASCII.GetChars(rowData);
            _gs1Algorithm = new GS1Algorithm(_barcode);
        }

        /// <summary>
        /// Inizializza un tokenizer a partire dal barcode codificato in ASCII
        /// </summary>
        /// <param name="asciiData"></param>
        public EAN128Tokenizer(char[] asciiData)
        {
            _barcode = asciiData;
            _gs1Algorithm = new GS1Algorithm(_barcode);
        }

        /// <summary>
        /// Restituisce il prossimo ElementString se questo esiste
        /// </summary>
        /// <returns>Il prossimo ElementString contenente i dati</returns>
        /// <exception cref="It.Daxo.Ean128.GS1AlgorithmException" /> in caso in cui si sia verificato un errore di parsing nella traduzione dell'AI
        public ElementString NextToken()
        {
            _gs1Algorithm.ResetAI();
            ElementStringFormat format = _gs1Algorithm.NextFormat();
            return _gs1Algorithm.NextElementWithFormat(format);
        }

        /// <summary>
        /// Indica se esistono ancora degli ElementString da parsare
        /// </summary>
        /// <returns></returns>
        public bool HasMoreToken()
        {
            return _gs1Algorithm.Index < _gs1Algorithm.Barcode.Length;
        } 
    }

    public class EAN128Exception : Exception
    {
        public EAN128Exception() : base() { }

        public EAN128Exception(string msg) : base(msg) { }
    }
}
