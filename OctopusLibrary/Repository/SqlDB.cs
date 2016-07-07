using OctopusLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OctopusLibrary.Repository
{
    /// <summary>
    /// ADO.NET
    /// </summary>
    public class SqlDB : ISqlDB, IDisposable
    {
        private SqlConnection conn;
        protected SqlCommand cmd;
        private bool disposed;

        public SqlDB()
        {
            this.disposed = false;
        }

        /// <summary>
        /// 초기화 합니다
        /// </summary>
        /// <param name="_conn">SqlConnection 객체</param>
        public SqlDB(SqlConnection _conn)
        {
            this.disposed = false;
            this.conn = _conn;
            this.conn.Open();
        }
        
        public virtual void StoredProcedure(string spname)
        {
            this.cmd = new SqlCommand();
            this.cmd.Connection = this.conn;
            this.cmd.CommandText = spname;
            this.cmd.CommandType = CommandType.StoredProcedure;
        }

        public virtual void RunQuery(string Query)
        {
            this.cmd = new SqlCommand();
            this.cmd.Connection = this.conn;
            this.cmd.CommandText = Query;
            this.cmd.CommandType = CommandType.Text;
        }

        public virtual void addParam(string paramName, int value)
        {
            this.cmd.Parameters.Add(paramName, SqlDbType.Int);
            this.cmd.Parameters[paramName].Value = value;
        }

        public virtual void addParam(string paramName, long value)
        {
            this.cmd.Parameters.Add(paramName, SqlDbType.BigInt);
            this.cmd.Parameters[paramName].Value = value;
        }

        public virtual void addParam(string paramName, float value)
        {
            this.cmd.Parameters.Add(paramName, SqlDbType.Float);
            this.cmd.Parameters[paramName].Value = value;
        }

        public virtual void addParam(string paramName, string value, int Size, SqlDbType sqltype)
        {
            this.cmd.Parameters.Add(paramName, sqltype, Size);
            this.cmd.Parameters[paramName].Value = value;
        }

        public virtual void addParam(string paramName, DateTime value)
        {
            this.cmd.Parameters.Add(paramName, SqlDbType.DateTime2);
            this.cmd.Parameters[paramName].Value = value;
        }

        public virtual void Execute()
        {
            this.cmd.ExecuteNonQuery();
        }

        public virtual Task ExecuteAsync()
        {
            return this.cmd.ExecuteNonQueryAsync();
        }

        public virtual DataTable ExecuteTable()
        {
            DataTable result = new DataTable();

            using (SqlDataAdapter Adp = new SqlDataAdapter(this.cmd))
            {
                Adp.Fill(result);
            }

            return result;
        }

        public virtual Task<DataTable> ExecuteTableAsync()
        {
            return Task.Factory.StartNew(() => ExecuteTable());
        }

        public virtual SqlDataReader ExecuteReader()
        {
            return this.cmd.ExecuteReader();
        }

        public virtual Task<SqlDataReader> ExecuteReaderAsync()
        {
            return Task.Factory.StartNew(() => ExecuteReader());
        }

        public virtual T ExecuteMapper<T>() where T : new()
        {
            T result = new T();

            DataTable Dt = this.ExecuteTable();
            if (Dt != null && Dt.Rows != null && Dt.Rows.Count > 0)
            {
                result = DataHandler.ListToModel<T>(Dt).FirstOrDefault();
                if (result == null)
                {
                    result = new T();
                }
            }

            return result;
        }

        public virtual Task<T> ExecuteMapperAsync<T>() where T : new()
        {
            return Task.Factory.StartNew(() => ExecuteMapper<T>());
        }

        public virtual List<T> ExecuteList<T>() where T : new()
        {
            List<T> result = new List<T>();

            DataTable Dt = this.ExecuteTable();
            if (Dt != null && Dt.Rows != null && Dt.Rows.Count > 0)
            {
                result = DataHandler.ListToData<T>(Dt);
                if (result == null)
                {
                    result = new List<T>();
                }
            }

            return result;
        }

        public virtual Task<List<T>> ExecuteListAsync<T>() where T : new()
        {
            return Task.Factory.StartNew(() => ExecuteList<T>());
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                try
                {
                    this.cmd.Dispose();
                }
                catch
                {

                }
                finally
                {
                    this.cmd = null;
                }
            }
            this.disposed = disposing;
        }

        ~SqlDB()
        {
            this.Dispose(true);
        }
    }
}
