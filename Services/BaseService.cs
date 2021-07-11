using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Services
{
    public class BaseService
    {
        /// <summary>
        /// Convert the given database table data to given list object
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public async Task<List<TN>> ConvertToList<TN>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(TN).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<TN>();
                foreach (var pro in properties)
                {
                    try
                    {
                        if (columnNames.Contains("c" + pro.Name.ToLower()))
                            pro.SetValue(objT, row["c" + pro.Name.ToLower()]);
                        else if (columnNames.Contains("n" + pro.Name.ToLower()))
                            pro.SetValue(objT, row["n" + pro.Name.ToLower()]);
                        else if (columnNames.Contains("b" + pro.Name.ToLower()))
                            if (pro.PropertyType.FullName == "System.Boolean")
                                pro.SetValue(objT, Convert.ToInt16(row["b" + pro.Name.ToLower()]) == 1 ? true : false);
                            else pro.SetValue(objT, row["b" + pro.Name.ToLower()].ToString());
                        else if (columnNames.Contains(pro.Name.ToLower()))
                            pro.SetValue(objT, row[pro.Name.ToLower()]);
                    }
                    catch (Exception ex) { }
                }
                return objT;
            }).ToList();

        }
    }
}
