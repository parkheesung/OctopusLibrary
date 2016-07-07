using OctopusLibrary.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OctopusLibrary.Utility
{
    public class FileHandler : IDisposable
    {
        private bool disposed;
        #region Dispose
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
            }
        }

        ~FileHandler()
        {
            Dispose(true);
        }
        #endregion

        public int MaxContentSize { get; set; }
        public string UseExtension { get; set; }
        public string ServerPath { get; set; }
        public string UrlPath { get; set; }

        public enum Byte
        {
            KB = 1024, MB = 1024 * 1024
        }

        public FileHandler()
        {
            this.MaxContentSize = 10 * (int)Byte.MB;
            this.UseExtension = "{|.jpg|.jpeg|.png|}";
            this.UrlPath = String.Empty;
            this.ServerPath = String.Empty;
        }

        /// <summary>
        /// 파일 읽기
        /// </summary>
        /// <param name="fileURL">파일 경로</param>
        /// <returns></returns>
        public static string ReadFile(string fileURL, Encoding EncMode)
        {
            StringBuilder result = new StringBuilder();

            try
            {
                if (!(String.IsNullOrEmpty(fileURL)))
                {
                    FileInfo fi = new FileInfo(fileURL);
                    if (fi.Exists)
                    {
                        using (StreamReader reader = new StreamReader(fileURL, EncMode))
                        {
                            result.Append(reader.ReadToEnd());
                            reader.Close();
                        }
                    }
                    fi = null;
                }
            }
            catch (Exception ex)
            {
                result.Clear();
                result.Append(ex.Message.ToString());
            }

            return result.ToString();
        }

        public static Task<string> ReadFileAsync(string fileURL, Encoding EncMode)
        {
            return Task.Factory.StartNew(() => ReadFile(fileURL, EncMode));
        }

        /// <summary>
        /// 파일쓰기
        /// </summary>
        /// <param name="fileURL">파일경로</param>
        /// <param name="bodyText">파일내용</param>
        /// <param name="EncMode">Encoding Type</param>
        /// <param name="overwrite">Over Write 여부</param>
        /// <returns></returns>
        public static bool WriteFile(string fileURL, string bodyText, Encoding EncMode, bool overwrite = false)
        {
            bool result = false;

            try
            {
                if (!(String.IsNullOrEmpty(fileURL)))
                {
                    FileInfo fi = new FileInfo(fileURL);
                    if (fi.Exists)
                    {
                        using (StreamWriter writer = new StreamWriter(fileURL, overwrite, EncMode))
                        {
                            writer.WriteLine(bodyText);
                            writer.Close();
                            result = true;
                        }
                    }
                    fi = null;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 새폴더 만들기
        /// </summary>
        /// <param name="root">생성할 위치에 상위 폴더</param>
        /// <param name="folder">폴더명</param>
        /// <returns></returns>
        public static string CreateFolder(string root, string folder)
        {
            string result = "";

            result = root + "\\" + folder;
            DirectoryInfo di = new DirectoryInfo(result);
            if (!di.Exists) di.Create();
            di = null;

            return result;
        }

        /// <summary>
        /// 업로드 폴더 지정
        /// </summary>
        /// <param name="urlPath">업로드할 대상 폴더값</param>
        public void SetFolder(string urlPath)
        {
            StringBuilder builder = new StringBuilder();
            this.UrlPath = String.Empty;
            if (urlPath.Substring(0, 1) != "/")
            {
                builder.Append("/");
            }
            builder.Append(urlPath);
            if (urlPath.Substring(urlPath.Length - 1) == "/")
            {
                builder.AppendFormat("{0}/", DateTime.Now.ToString("yyyyMM"));
            }
            else
            {
                builder.AppendFormat("/{0}/", DateTime.Now.ToString("yyyyMM"));
            }

            this.UrlPath = builder.ToString();
            this.ServerPath = HttpContext.Current.Server.MapPath(this.UrlPath);
            DirectoryInfo di = new DirectoryInfo(this.ServerPath);
            if (!di.Exists)
            {
                di.Create();
            }
            di = null;
        }

        /// <summary>
        /// 파일 업로드
        /// </summary>
        /// <param name="RequestFile"></param>
        /// <returns></returns>
        public ReturnData SaveFile(HttpPostedFileBase RequestFile)
        {
            ReturnData result = new ReturnData();

            try
            {
                if (RequestFile.ContentLength > this.MaxContentSize)
                {
                    result.Error("파일이 용량({0}byte)을 초과했습니다. 대상 파일 용량은 {1}byte 입니다.", this.MaxContentSize, RequestFile.ContentLength);
                }
                else
                {
                    string fileNameExtension = Path.GetExtension(RequestFile.FileName);
                    string fileName = Path.GetFileName(RequestFile.FileName);
                    if (fileName.IndexOf(fileNameExtension) > -1)
                    {
                        fileName = fileName.Substring(0, fileName.LastIndexOf(fileNameExtension));
                    }

                    if (String.IsNullOrEmpty(this.UseExtension) || this.UseExtension.Equals("*") || this.UseExtension.ToLower().IndexOf(fileNameExtension.ToLower()) > -1)
                    {
                        fileName = CreateNewFileName(fileNameExtension);
                        string fullPath = Path.Combine(this.ServerPath, String.Format("{0}{1}", fileName, fileNameExtension));
                        RequestFile.SaveAs(fullPath);
                        result.Check = true;
                        result.Value = String.Format("{0}{1}{2}", this.UrlPath, fileName, fileNameExtension);
                    }
                    else
                    {
                        result.Error("업로드할 수 없는 파일 속성입니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// 지정 폴더내에서 고유파일 생성
        /// </summary>
        /// <param name="fileName">파일명</param>
        /// <param name="fileNameExtension">파일확장자</param>
        /// <returns></returns>
        public string UniqueFileName(string fileName, string fileNameExtension)
        {
            string result = fileName;
            string original = Path.Combine(this.ServerPath, fileName) + fileNameExtension;
            string tmp = original;
            FileInfo fi = new FileInfo(tmp);
            if (fi.Exists)
            {
                int n = 1;
                tmp = String.Format("{0}[{1}]{2}", fileName, n, fileNameExtension);
                while (fi.Exists)
                {
                    tmp = String.Format("{0}[{1}]{2}", fileName, n, fileNameExtension);
                    fi = new FileInfo(tmp);
                    n++;
                }

                result = fi.Name;
                if (result.IndexOf(fileNameExtension) > -1)
                {
                    result = result.Substring(0, result.LastIndexOf(fileNameExtension));
                }
            }
            return result;
        }

        public static string CreateNewFileName(string ExtName)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            builder.Append(DateTime.Now.Millisecond);
            return builder.ToString();
        }

        public static string GetExtendName(string fileName)
        {
            string result = String.Empty;

            try
            {
                string tmp = fileName;
                if (tmp.IndexOf("\\") > -1)
                {
                    var arr1 = tmp.Split('\\');
                    tmp = arr1[arr1.Length - 1];
                }

                if (tmp.IndexOf("/") > -1)
                {
                    var arr2 = tmp.Split('/');
                    tmp = arr2[arr2.Length - 1];
                }

                if (tmp.IndexOf(".") > -1)
                {
                    var arr3 = tmp.Split('.');
                    tmp = arr3[arr3.Length - 1];
                }

                result = tmp;
            }
            catch
            {
                result = "";
            }

            return result;
        }

        public static Task<string> CreateNewFileNameAsync(string ExtName)
        {
            return Task.Factory.StartNew(() => CreateNewFileName(ExtName));
        }

        public static Task<string> GetExtendNameAsync(string fileName)
        {
            return Task.Factory.StartNew(() => GetExtendName(fileName));
        }
    }
}
