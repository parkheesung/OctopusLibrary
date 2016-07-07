using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OctopusLibrary.Repository
{
    public interface ISqlDB
    {
        void StoredProcedure(string spname);
        void RunQuery(string Query);
        void addParam(string paramName, int value);
        void addParam(string paramName, long value);
        void addParam(string paramName, float value);
        void addParam(string paramName, string value, int Size, SqlDbType sqltype);
        void addParam(string paramName, DateTime value);
        void Execute();
        DataTable ExecuteTable();
        Task<DataTable> ExecuteTableAsync();
        SqlDataReader ExecuteReader();
        Task<SqlDataReader> ExecuteReaderAsync();
        T ExecuteMapper<T>(T returnModel) where T : new();
        Task<T> ExecuteMapperAsync<T>(T returnModel) where T : new();
        List<T> ExecuteList<T>(List<T> returnModel) where T : new();
        Task<List<T>> ExecuteListAsync<T>(List<T> returnModel) where T : new();
    }
}
