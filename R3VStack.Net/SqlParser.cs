using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace R3VStack.Net
{
    /// <summary>
    /// Class to store one sql statement data for the specified table
    /// </summary>
    public class SqlQuery : Hashtable
    {
        public String tableName { get; set; }
        public SqlQuery(string table)
        {
            tableName = table;
        }
    }

    /// <summary>
    /// Class to write sql statements to a SQL file
    /// </summary>
    public class SqlWriter : StreamWriter
    {
        public SqlWriter(Stream stream)
            : base(stream)
        {
        }

        public SqlWriter(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Writes a sql insert statement to a SQL file.
        /// </summary>
        /// <param name="query">The insert column/value object list to be written</param>
        public void WriteSqlInsert(SqlQuery query)
        {
            string eolFilter = "<br>";
            string sngQuoteFilter = "^";
            WriteSqlInsert(query,eolFilter,sngQuoteFilter);
        }

        /// <summary>
        /// Writes a sql insert statement to a SQL file. Overload Method
        /// </summary>
        /// <param name="query">The insert column/value object list to be written</param>
        /// <param name="eolFilter">carriage return/EOL filter string replacement</param>
        /// <param name="sngQuoteFilter">single-quote filter string replacement</param>
        public void WriteSqlInsert(SqlQuery query,string eolFilter, string sngQuoteFilter)
        {
            string table = query.tableName;
            var typeString = typeof(string);
            StringBuilder s1 = new StringBuilder();
            StringBuilder s2 = new StringBuilder();
            bool isFirst = true;
            foreach (string key in query.Keys)
            {
                if (isFirst) isFirst = false;
                else
                {
                    s1.Append(", ");
                    s2.Append(", ");
                }
                s1.Append(key);
                var val = query[key];
                if (val.GetType() == typeString) //sql syntax: string values have to enclosed by single quotes
                {
                    string strVal = val.ToString();
                    strVal = strVal.Replace("'", sngQuoteFilter);  //filter single-quote
                    strVal = strVal.Replace("\r\n", eolFilter);  //filter carriage return, EOL
                    strVal = "'" + strVal + "'";
                    s2.Append(strVal);
                }
                else
                {
                    s2.Append(val);
                }
               
                
            }

            //string q= "INSERT INTO " + table + " (" + s1.ToString() + ") VALUES (" + s2.ToString() + ");";
            string q = "INSERT INTO " + table + " (" + s1.ToString() + ") VALUES (" + s2.ToString() + ")";

            WriteLine(q);
        }

        /// <summary>
        /// Writes a sql update statement to a SQL file.
        /// </summary>
        /// <param name="query">The update column/value object list to be written</param>
        public void WriteSqlUpdate(SqlQuery query)
        {
            string eolFilter = "<br>";
            string sngQuoteFilter = "^";
            WriteSqlUpdate(query, eolFilter, sngQuoteFilter);
        }


        /// <summary>
        /// Writes a sql update statement to a SQL file. Method Overload
        /// </summary>
        /// <param name="query">The update column/value object list to be written</param>
        /// <param name="eolFilter">carriage return/EOL filter string replacement</param>
        /// <param name="sngQuoteFilter">single-quote filter string replacement</param>
        public void WriteSqlUpdate(SqlQuery query, string eolFilter, string sngQuoteFilter)
        {
            string table = query.tableName;
            var typeString = typeof(string);
            StringBuilder s1 = new StringBuilder();
            bool isFirst = true;
            foreach (string key in query.Keys)
            {
                if (isFirst) isFirst = false;
                else
                {
                    s1.Append(", ");
                }
                var val = query[key];
                if (val.GetType() == typeString)  //sql syntax: string values have to enclosed by single quotes
                {
                    string strVal = val.ToString();
                    strVal = strVal.Replace("'", "^");
                    strVal = strVal.Replace("\r\n", "<br>");
                    var strColumn = key + "='" + strVal + "'";
                    s1.Append(strColumn);
                }
                else
                {
                    var strColumn = key + "=" + val.ToString();
                    s1.Append(strColumn);
                }
            }

            //string q = "Update " + table + " SET " + s1.ToString() + ";";
            string q = "Update " + table + " SET " + s1.ToString();
            WriteLine(q);
        }

        /// <summary>
        /// Writes a sql delete statement to a SQL file.
        /// </summary>
        /// <param name="query">The delete column/value object list to be written as where clause</param>
        public void WriteDelete(SqlQuery query)
        {
            string table = query.tableName;
            var typeString = typeof(string);
            string q = "";
            StringBuilder s1 = new StringBuilder();
            bool isFirst = true;
            foreach (string key in query.Keys)
            {
                if (isFirst) isFirst = false;
                else
                {
                    s1.Append("AND ");
                }
                var val = query[key];
                if (val.GetType() == typeString)  //sql syntax: string values have to enclosed by single quotes
                {
                    string strVal = val.ToString();
                    strVal = strVal.Replace("'", "^");
                    strVal = strVal.Replace("\r\n", "<br>");
                    var strColumn = key + "='" + strVal + "'";
                    s1.Append(strColumn);
                }
                else
                {
                    var strColumn = key + "=" + val.ToString();
                    s1.Append(strColumn);
                }
            }
            if (query.Keys.Count < 1)
            {
                q = "Delete From " + table;
            }
            else
            {
                //q = "Delete From " + table + " WHERE " + s1.ToString() + ";";
                q = "Delete From " + table + " WHERE " + s1.ToString();
            }
            
            WriteLine(q);
        }
       

    }//end class

}//end namespace
