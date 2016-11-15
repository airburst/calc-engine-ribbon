using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationEngine
{
    public class ExcelSupportedFunctions
    {
        // Fields
        public List<string> FunctionList;
        
        // Constructor
        public ExcelSupportedFunctions()
        {
            FunctionList = PopulateFunctionList();
        }

        // Methods
        // Return a list of functions supported by POI
        public List<string> PopulateFunctionList()
        {
            List<string> fList = new List<string>();

            // Add 141 Supported Functions
            fList.Add("ABS");
            fList.Add("ACOS");
            fList.Add("ACOSH");
            fList.Add("ADDRESS");
            fList.Add("AND");
            fList.Add("ASIN");
            fList.Add("ASINH");
            fList.Add("ATAN");
            fList.Add("ATAN2");
            fList.Add("ATANH");
            fList.Add("AVEDEV");
            fList.Add("AVERAGE");
            fList.Add("CEILING");
            fList.Add("CHAR");
            fList.Add("CHOOSE");
            fList.Add("CLEAN");
            fList.Add("COLUMN");
            fList.Add("COLUMNS");
            fList.Add("COMBIN");
            fList.Add("CONCATENATE");
            fList.Add("COS");
            fList.Add("COSH");
            fList.Add("COUNT");
            fList.Add("COUNTA");
            fList.Add("COUNTBLANK");
            fList.Add("COUNTIF");
            fList.Add("DATE");
            fList.Add("DAY");
            fList.Add("DAYS360");
            fList.Add("DEGREES");
            fList.Add("DEVSQ");
            fList.Add("DOLLAR");
            fList.Add("ERROR.TYPE");
            fList.Add("EVEN");
            fList.Add("EXACT");
            fList.Add("EXP");
            fList.Add("FACT");
            fList.Add("FALSE");
            fList.Add("FIND");
            fList.Add("FLOOR");
            fList.Add("FV");
            fList.Add("HLOOKUP");
            fList.Add("HOUR");
            fList.Add("HYPERLINK");
            fList.Add("IF");
            fList.Add("INDEX");
            fList.Add("INDIRECT");
            fList.Add("INT");
            fList.Add("IRR");
            fList.Add("ISBLANK");
            fList.Add("ISERROR");
            fList.Add("ISEVEN");
            fList.Add("ISLOGICAL");
            fList.Add("ISNA");
            fList.Add("ISNONTEXT");
            fList.Add("ISNUMBER");
            fList.Add("ISODD");
            fList.Add("ISREF");
            fList.Add("ISTEXT");
            fList.Add("LARGE");
            fList.Add("LEFT");
            fList.Add("LEN");
            fList.Add("LN");
            fList.Add("LOG");
            fList.Add("LOG10");
            fList.Add("LOOKUP");
            fList.Add("LOWER");
            fList.Add("MATCH");
            fList.Add("MAX");
            fList.Add("MAXA");
            fList.Add("MEDIAN");
            fList.Add("MID	");
            fList.Add("MIN");
            fList.Add("MINA");
            fList.Add("MINUTE");
            fList.Add("MOD");
            fList.Add("MODE");
            fList.Add("MONTH");
            fList.Add("MROUND");
            fList.Add("NA");
            fList.Add("NETWORKDAYS");
            fList.Add("NOT");
            fList.Add("NOW");
            fList.Add("NPER");
            fList.Add("NPV");
            fList.Add("ODD");
            fList.Add("OFFSET");
            fList.Add("OR");
            fList.Add("PI");
            fList.Add("PMT");
            fList.Add("POISSON");
            fList.Add("POWER");
            fList.Add("PRODUCT");
            fList.Add("PV");
            fList.Add("RADIANS");
            fList.Add("RAND");
            fList.Add("RANDBETWEEN");
            fList.Add("RANK");
            fList.Add("RATE");
            fList.Add("REPLACE");
            fList.Add("RIGHT");
            fList.Add("ROUND");
            fList.Add("ROUNDDOWN");
            fList.Add("ROUNDUP");
            fList.Add("ROW");
            fList.Add("ROWS");
            fList.Add("SEARCH");
            fList.Add("SECOND	");
            fList.Add("SIGN");
            fList.Add("SIN");
            fList.Add("SINH");
            fList.Add("SMALL");
            fList.Add("SQRT");
            fList.Add("STDEV");
            fList.Add("SUBSTITUTE");
            fList.Add("SUBTOTAL");
            fList.Add("SUM");
            fList.Add("SUMIF");
            fList.Add("SUMIFS");
            fList.Add("SUMPRODUCT");
            fList.Add("SUMSQ");
            fList.Add("SUMX2MY2");
            fList.Add("SUMX2PY2");
            fList.Add("SUMXMY2");
            fList.Add("T");
            fList.Add("TAN");
            fList.Add("TANH");
            fList.Add("TEXT");
            fList.Add("TIME");
            fList.Add("TODAY");
            fList.Add("TRIM");
            fList.Add("TRUE");
            fList.Add("TRUNC");
            fList.Add("UPPER");
            fList.Add("VALUE");
            fList.Add("VAR");
            fList.Add("VARP");
            fList.Add("VLOOKUP");
            fList.Add("WORKDAY");
            fList.Add("YEAR");
            fList.Add("YEARFRAC");

            return fList;
        }
    }
}
