using System;
using System.Text;

namespace OctopusLibrary.Data
{
    public class MssqlQuery
    {
        public static string Paging(MssqlQueryData paramData)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("DECLARE @RowsPerPage2 INT = {0}, @PageNumber2 INT = {1}", paramData.PageSize, paramData.CurrentNo);
            builder.Append("SELECT TOP (@RowsPerPage2) resultTable.* FROM ");
            builder.AppendFormat(" ( SELECT TOP (@RowsPerPage2 * @PageNumber2) ROW_NUMBER() OVER (ORDER BY {0} ", paramData.TargetColumn);
            if (!String.IsNullOrEmpty(paramData.WhereString))
            {
                builder.AppendFormat("{0}", paramData.OrderString);
            }
            builder.Append(") AS rownumber ");
            builder.AppendFormat(" , {0} FROM [{1}] ", paramData.Columns, paramData.TableName);
            if (!String.IsNullOrEmpty(paramData.WhereString))
            {
                builder.AppendFormat(" where {0} ", paramData.WhereString);
            }
            builder.AppendFormat(" ) AS resultTable ");
            builder.Append(" WHERE rownumber > (@PageNumber2 - 1) * @RowsPerPage2; ");

            return builder.ToString();
        }
    }

    public class MssqlQueryData
    {
        public const string ORDER_DESC = "desc";
        public const string ORDER_ASC = "asc";

        public string TableName { get; set; }
        public string Columns { get; set; }
        public string TargetColumn { get; set; }
        public string WhereString { get; set; }
        public string OrderString { get; set; }
        public int PageSize { get; set; }
        public int CurrentNo { get; set; }

        public MssqlQueryData()
        {
            this.TableName = String.Empty;
            this.Columns = String.Empty;
            this.TargetColumn = String.Empty;
            this.WhereString = String.Empty;
            this.OrderString = ORDER_DESC;
            this.PageSize = 10;
            this.CurrentNo = 1;
        }
    }
}
