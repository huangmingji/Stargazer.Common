using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Stargazer.Common.File
{
    /// <summary>
    /// 计算文件的hash值
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 计算文件的MD5
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static async Task<string> GetFileHashMd5Async(string fileName)
        {
            if (!System.IO.File.Exists(fileName)) 
            {
                throw new FileNotFoundException();
            }
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return await GetFileHashMd5Async(stream);
            }
        }

        public static async Task<string> GetFileHashSha1(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return await GetFileHashSha1Async(stream);
            }
        }

        /// <summary>
        /// 计算文件的MD5
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<string> GetFileHashMd5Async(this Stream stream)
        {
            var hashBytes = await HashDataAsync(stream, "md5");
            return ByteArrayToHexString(hashBytes);
        }

        public static async Task<string> GetFileHashSha1Async(this Stream stream)
        {
            var hashBytes = await HashDataAsync(stream, "sha1");
            return ByteArrayToHexString(hashBytes);
        }

        /// <summary>
        /// 计算哈希值
        /// </summary>
        /// <param name="stream">要计算哈希值的 Stream</param>
        /// <param name="algName">算法:sha1,md5</param>
        /// <returns>哈希值字节数组</returns>
        private static async Task<byte[]> HashDataAsync(Stream stream, string algName)
        {
            System.Security.Cryptography.HashAlgorithm hashAlgorithm;
            if (String.Compare(algName, "sha1", StringComparison.OrdinalIgnoreCase) == 0)
            {
                hashAlgorithm = System.Security.Cryptography.SHA1.Create();
            }
            else if (String.Compare(algName, "md5", StringComparison.OrdinalIgnoreCase) == 0)
            {
                hashAlgorithm = System.Security.Cryptography.MD5.Create();
            }
            else
            {
                throw new ArgumentException();
            }

            var hashTypes = await hashAlgorithm.ComputeHashAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);
            hashAlgorithm.Dispose();
            return hashTypes;
        }

        /// <summary>
        /// 字节数组转换为16进制表示的字符串
        /// </summary>
        private static string ByteArrayToHexString(byte[] buffer)
        {
            //将字节数组转换成十六进制的字符串形式
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var t in buffer)
            {
                stringBuilder.Append(t.ToString("x2"));
            }
            return stringBuilder.ToString().Replace("-", "");
        }
    }
}
