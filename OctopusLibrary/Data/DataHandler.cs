using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OctopusLibrary.Data
{
    public class DataHandler
    {
        /// <summary>
        /// 클래스간 동일 Propertie의 내용을 복사합니다.
        /// </summary>
        /// <typeparam name="T">반환받을 모델</typeparam>
        /// <param name="list">값을 가지고 있는 모델 리스트</param>
        /// <returns></returns>
        public static List<T> ModelToModel<T>(List<object> list) where T : new()
        {
            List<T> rtn = new List<T>();

            if (list != null && list.Count > 0)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    rtn.Add(ModelToModelBind<T>(list[i]));
                }
                
            }

            return rtn;
        }

        public static List<T> ModelToModel<T>(IQueryable<object> list) where T : new()
        {
            List<T> rtn = new List<T>();

            if (list != null)
            {
                foreach(var obj in list)
                {
                    rtn.Add(ModelToModelBind<T>(obj));
                }
            }

            return rtn;
        }

        public static T ModelToModelBind<T>(object data) where T : new()
        {
            T rtn = new T();

            if (data != null)
            {
                Type resultClass = typeof(T);
                Type requestClass = data.GetType();
                PropertyInfo[] resultInfo = resultClass.GetProperties();
                PropertyInfo[] requestInfo = requestClass.GetProperties();

                PropertyInfo result = null;
                System.Reflection.FieldInfo resultRow = null;
                System.Reflection.FieldInfo requestRow = null;

                foreach (PropertyInfo request in requestInfo)
                {
                    try
                    {
                        result = resultInfo.Where(x => x.Name == request.Name).FirstOrDefault();
                        if (result != null && result.Name != null)
                        {
                            resultRow = resultClass.GetField(result.Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                            requestRow = requestClass.GetField(request.Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                            resultRow.SetValue(rtn, request.GetValue(data));
                        }
                    }
                    catch
                    {

                    }
                }
            }

            return rtn;
        }


        /// <summary>
        /// Model을 기준으로 DataTable에 값을 Model에 매핑시킵니다.
        /// </summary>
        /// <typeparam name="T">매핑할 대상 모델</typeparam>
        /// <param name="dt">매핑할 데이터가 들은 DataTable</param>
        /// <returns></returns>
        public static List<T> ListToModel<T>(DataTable dt) where T : new()
        {
            return ListToModel(dt, () => new T());
        }

        /// <summary>
        /// Model을 기준으로 DataTable에 값을 Model에 매핑시킵니다.
        /// </summary>
        /// <typeparam name="T">매핑할 대상 모델</typeparam>
        /// <param name="dt">매핑할 데이터가 들은 DataTable</param>
        /// <param name="factory">매핑할 대상 모델의 인스턴스</param>
        /// <returns></returns>
        public static List<T> ListToModel<T>(DataTable dt, Func<T> factory)
        {
            List<T> result = new List<T>();
            Type resultClass = typeof(T);
            PropertyInfo[] pClass = resultClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn = factory();
            foreach (DataRow item in dt.Rows)
            {
                foreach (PropertyInfo pc in pClass)
                {
                    try
                    {
                        DataColumn d = dc.Find(c => c.ColumnName == pc.Name);
                        if (d != null && item[pc.Name] != null) pc.SetValue(cn, item[pc.Name], null);
                    }
                    catch
                    {

                    }
                }

                result.Add(cn);
            }
            return result;
        }

        /// <summary>
        /// Data를 기준으로 DataTable에 값을 Model에 매핑시킵니다.
        /// </summary>
        /// <typeparam name="T">매핑할 대상 모델</typeparam>
        /// <param name="dt">매핑할 데이터가 들은 DataTable</param>
        /// <returns></returns>
        public static List<T> ListToData<T>(DataTable dt) where T : new()
        {
            return ListToData(dt, () => new T());
        }

        /// <summary>
        /// Data를 기준으로 DataTable에 값을 Model에 매핑시킵니다.
        /// </summary>
        /// <typeparam name="T">매핑할 대상 모델</typeparam>
        /// <param name="dt">매핑할 데이터가 들은 DataTable</param>
        /// <param name="factory">매핑할 대상 모델의 인스턴스</param>
        /// <returns></returns>
        public static List<T> ListToData<T>(DataTable dt, Func<T> factory)
        {
            List<T> result = new List<T>();
            Type resultClass = typeof(T);
            PropertyInfo[] pClass = resultClass.GetProperties();
            List<DataColumn> dc = dt.Columns.Cast<DataColumn>().ToList();
            T cn;
            foreach (DataRow item in dt.Rows)
            {
                cn = factory();

                foreach (DataColumn c in dc)
                {
                    try
                    {
                        var pi = pClass.SingleOrDefault(p => p.Name == c.ColumnName);
                        if (pi != null && item[pi.Name] != null) pi.SetValue(cn, item[pi.Name], null);
                    }
                    catch
                    {

                    }
                }

                result.Add(cn);
            }
            return result;
        }

        public static string GetPropertyValue<T>(T cn, string FieldName) where T : new()
        {
            string result = String.Empty;

            if (!String.IsNullOrEmpty(FieldName))
            {
                Type resultClass = typeof(T);
                PropertyInfo[] pClass = resultClass.GetProperties();
                foreach (PropertyInfo pc in pClass)
                {
                    if (pc.Name.Equals(FieldName))
                    {
                        result = Convert.ToString(pc.GetValue(cn, null));
                        break;
                    }
                }
            }

            return result;
        }
    }
}
