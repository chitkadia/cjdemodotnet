using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using MySql.Data.MySqlClient;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace FileUpload.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        public DbContext _context { get; set; }
        public BaseRepository(DbContext mySqlDatabase)
        {
            this._context = mySqlDatabase;
        }
        public MySqlCommand CreateCommand()
        {
            //MySqlConnection _connection = new MySqlConnection(_context._connectionString);
            //var cmd = _connection.CreateCommand();

            return _context._connection.CreateCommand();
            //return cmd;
        }
        public void AddParameter(MySqlCommand command, string parametername, object value)
        {
            command.Parameters.Add(new MySqlParameter("@" + parametername, value));
        }
        //public async Task<IEnumerable<T>> ToList(MySqlCommand command)
        //{
        //    using (var record = await command.ExecuteReaderAsync())
        //    {
        //        List<T> items = new List<T>();
        //        while (record.Read())
        //        {

        //            items.Add(Map<T>(record));
        //        }
        //        return items;
        //    }
        //}
        public async Task<DataTable> ExecuteDataTabelAsync(MySqlCommand command)
        {
            //if(command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            
            //command.Connection.Open();
            DataTable dt = new DataTable();
            var reader = await command.ExecuteReaderAsync();
            dt.Load(reader);

            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            return dt;
        }

        public DataTable NoAsyncExecuteDataTabelAsync(MySqlCommand command)
        {
            //if(command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();

            //command.Connection.Open();
            DataTable dt = new DataTable();
            var reader = command.ExecuteReader();
            dt.Load(reader);

            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            return dt;
        }

        public async Task<DataSet> ExecuteDataSetAsync(MySqlCommand command)
        {
            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            //command.Connection.Open();

            DataSet ds = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = command;

            da.Fill(ds);
            //await Task.Run(() => da.Fill(ds));

            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            return ds;
        }

        public async Task<object> ExecuteScalarAsync(MySqlCommand command)
        {
            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            //command.Connection.Open();
            var result =  await command.ExecuteScalarAsync();

            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            return result;
        }
        public async Task<object> ExecuteNonQueryAsync(MySqlCommand command)
        {
            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            //command.Connection.Open();
            var result = await command.ExecuteNonQueryAsync();

            //if (command.Connection.State == ConnectionState.Open)
            //    command.Connection.Close();
            return result; 
        }
        public T Map<T>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<T>();
            //foreach (var property in typeof(T).GetProperties())
            //{
            //    if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
            //        property.SetValue(objT, record[property.Name]);
            //}
            return objT;
        }

        public async Task<List<T>> ToList(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();

        }

        //Convert the given database table data to given list object
        public async Task<List<TN>> ConvertToList<TN>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(TN).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<TN>();
                foreach (var pro in properties)
                {
                    {
                        try
                        {
                            if (columnNames.Contains("c" + pro.Name.ToLower()))
                                pro.SetValue(objT, row["c" + pro.Name.ToLower()]);
                            else if (columnNames.Contains("n" + pro.Name.ToLower()))
                            {
                                Type tProp = pro.PropertyType;

                                if (row["n" + pro.Name.ToLower()] == null || row["n" + pro.Name.ToLower()] == DBNull.Value)
                                {
                                    pro.SetValue(objT, Convert.ChangeType(0, tProp), null);
                                }
                                else
                                {
                                    pro.SetValue(objT, Convert.ChangeType(row["n" + pro.Name.ToLower()], tProp), null);
                                }

                            }
                            else if (columnNames.Contains("b" + pro.Name.ToLower()))
                                if (pro.PropertyType.FullName == "System.Boolean")
                                    pro.SetValue(objT, Convert.ToInt16(row["b" + pro.Name.ToLower()]) == 1 ? true : false);
                                else pro.SetValue(objT, row["b" + pro.Name.ToLower()].ToString());
                            else if (columnNames.Contains(pro.Name.ToLower()))
                                pro.SetValue(objT, row[pro.Name.ToLower()]);
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                return objT;
            }).ToList();

        }


        public  List<TN> NotAsyncConvertToList<TN>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(TN).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<TN>();
                foreach (var pro in properties)
                {
                    {
                        try
                        {
                            if (columnNames.Contains("c" + pro.Name.ToLower()))
                                pro.SetValue(objT, row["c" + pro.Name.ToLower()]);
                            else if (columnNames.Contains("n" + pro.Name.ToLower()))
                            {
                                Type tProp = pro.PropertyType;

                                if (row["n" + pro.Name.ToLower()] == null || row["n" + pro.Name.ToLower()] == DBNull.Value)
                                {
                                    pro.SetValue(objT, Convert.ChangeType(0, tProp), null);
                                }
                                else
                                {
                                    pro.SetValue(objT, Convert.ChangeType(row["n" + pro.Name.ToLower()], tProp), null);
                                }

                            }
                            else if (columnNames.Contains("b" + pro.Name.ToLower()))
                                if (pro.PropertyType.FullName == "System.Boolean")
                                    pro.SetValue(objT, Convert.ToInt16(row["b" + pro.Name.ToLower()]) == 1 ? true : false);
                                else pro.SetValue(objT, row["b" + pro.Name.ToLower()].ToString());
                            else if (columnNames.Contains(pro.Name.ToLower()))
                                pro.SetValue(objT, row[pro.Name.ToLower()]);
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                return objT;
            }).ToList();

        }

        /// <summary>
        /// Convert the given dataset table data to given list object
        /// </summary>
        /// <typeparam name="TN"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        public async Task<List<TN>> ConvertToList<TN>(DataSet ds)
        {
            DataTable dt = ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();

            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(TN).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<TN>();
                foreach (var pro in properties)
                {
                    {
                        try
                        {
                            if (columnNames.Contains("c" + pro.Name.ToLower()))
                                pro.SetValue(objT, row["c" + pro.Name.ToLower()]);
                            else if (columnNames.Contains("n" + pro.Name.ToLower()))
                            {
                                Type tProp = pro.PropertyType;

                                if (row["n" + pro.Name.ToLower()] == null || row["n" + pro.Name.ToLower()] == DBNull.Value)
                                {
                                    pro.SetValue(objT, Convert.ChangeType(0, tProp), null);
                                }
                                else
                                {
                                    pro.SetValue(objT, Convert.ChangeType(row["n" + pro.Name.ToLower()], tProp), null);
                                }

                            }
                            else if (columnNames.Contains("b" + pro.Name.ToLower()))
                                if (pro.PropertyType.FullName == "System.Boolean")
                                    pro.SetValue(objT, Convert.ToInt16(row["b" + pro.Name.ToLower()]) == 1 ? true : false);
                                else pro.SetValue(objT, row["b" + pro.Name.ToLower()].ToString());
                            else if (columnNames.Contains(pro.Name.ToLower()))
                                pro.SetValue(objT, row[pro.Name.ToLower()]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();

        }


    }
}
