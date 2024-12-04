using System.IO;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace Stargazer.Common.File
{
    public class UploadFileInfo
    {
        public UploadFileInfo(FileStream fileStream, string fileName, string extension, string contentType = "")
        {
            long fileSize = fileStream.Length;
            string hash = fileStream.GetFileHashMd5Async().Result;

            byte[] fileBytes = new byte [fileStream.Length];
            fileStream.Read(fileBytes, 0, fileBytes.Length);
            fileStream.Seek(0, SeekOrigin.Begin);
            
            FileName = fileName.Replace(extension, "");;
            FileExtension = extension;
            FilePath = hash;
            FileHash = hash;
            FileSize = fileSize;
            FileBytes = fileBytes;
            FileType = contentType;
        }

        public UploadFileInfo(IBrowserFile file)
        {
            string extension = Path.GetExtension(file.Name)?.ToLower() ?? "";

            long fileSize = file.Size;
            Stream stream = file.OpenReadStream();

            string hash = stream.GetFileHashMd5Async().Result;

            byte[] fileBytes = new byte [stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            FileName = file.Name.Replace(extension, "");
            FileExtension = extension;
            FilePath = hash;
            FileHash = hash;
            FileSize = fileSize;
            FileBytes = fileBytes;
            FileType = file.ContentType;
        }

        public UploadFileInfo(IFormFile formFile)
        {
            string extension = Path.GetExtension(formFile.FileName)?.ToLower() ?? "";

            long fileSize = formFile.Length;
            Stream stream = formFile.OpenReadStream();

            string hash = stream.GetFileHashMd5Async().Result;

            byte[] fileBytes = new byte [stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            FileName = formFile.FileName.Replace(extension, "");
            FileExtension = extension;
            FilePath = hash;
            FileHash = hash;
            FileSize = fileSize;
            FileBytes = fileBytes;
            FileType = formFile.ContentType;
        }
        
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] FileBytes { get; set; }
        
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; } = "";

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension { get; set; }
        
        /// <summary>
        /// 文件的hash值
        /// </summary>
        public string FileHash { get; set; } = "";

        /// <summary>
        /// 文件保存路径
        /// </summary>
        public string FilePath { get; set; } = "";

        /// <summary>
        /// file length in bytes
        /// </summary>
        public long FileSize { get; set; }
    }
}