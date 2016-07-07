using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Web.Mvc;

namespace OctopusLibrary.Results
{
    public class ZipResult : ActionResult
    {
        private string zipFileName;
        private List<string> filesToZip;
        public string ZipFileName
        {
            get { return zipFileName ?? "unnamed.zip"; }
            set { zipFileName = value; }
        }
        public ZipResult(string _zipFileName, List<string> filesToZip)
        {
            if (filesToZip == null)
            {
                throw new ArgumentException("No file is specified to compress and download.");
            }
            this.filesToZip = filesToZip;
            if (!string.IsNullOrEmpty(_zipFileName))
            {
                zipFileName = _zipFileName;
            }
        }
        public override void ExecuteResult(ControllerContext context)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipArchive fileContainer = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (string filePath in filesToZip)
                    {
                        if (File.Exists(filePath))
                        {
                            fileContainer.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                        }
                    }
                }  // ZipArchive object is disposed and automatically backed in the memory stream

                var response = context.HttpContext.Response;
                response.ContentType = "application/zip";
                response.AppendHeader("content-disposition", "attachment; filename=" + ZipFileName);
                ms.WriteTo(response.OutputStream);
            }
        }
    }
}
