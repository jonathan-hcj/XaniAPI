using Microsoft.Data.SqlClient;
using System.Collections;
using System.Data;

namespace XaniAPI.Business
{
    public class DatabaseBusiness
    {
        public static SqlParameter Int64IdParameter(string parameterName, IEnumerable idList)
        {
            var dataIds = new DataTable();
            dataIds.Columns.Add("Id", typeof(int));

            foreach (var item in idList)
            {
                var insertDR = dataIds.NewRow();
                insertDR["Id"] = item;
                dataIds.Rows.Add(insertDR);
            }
            return new SqlParameter(parameterName, SqlDbType.Structured)
            {
                Value = dataIds,
                TypeName = "[dbo].IdList64"
            };
        }
    }
}
