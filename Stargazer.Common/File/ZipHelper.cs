using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Stargazer.Common.File
{
    public class ZipHelper
    {
        /// <summary>
        /// 添加并压缩文件
        /// </summary>
        /// <param name="zipPath">压缩文件地址</param>
        /// <param name="zipFiles">压缩文件列表地址</param>
        /// <param name="notZip">未添加压缩成功文件列表</param>
        /// <returns></returns>
        public static void CreateZip(ref string zipPath, List<string> zipFiles, out List<string> notZip)
        {
            if (System.IO.File.Exists(zipPath))
            {
                throw new Exception("The file already exists.");
            }
            
            notZip = new List<string>();
            string dirPath = Path.GetDirectoryName(zipPath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using (FileStream zipFileToOpen = new FileStream(zipPath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipFileToOpen, ZipArchiveMode.Create))
            {
                foreach (var item in zipFiles)
                {
                    if (!System.IO.File.Exists(item))
                    {
                        notZip.Add(item);
                    }
                    string fileName = Path.GetFileName(item);
                    ZipArchiveEntry readMeEntry = archive.CreateEntry(fileName);
                    using (Stream stream = readMeEntry.Open())
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(item);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 添加并压缩文件
        /// </summary>
        /// <param name="zipPath">压缩文件地址</param>
        /// <param name="zipFiles">压缩文件列表地址</param>
        /// <param name="notZip">未添加压缩成功文件列表</param>
        /// <returns></returns>
        public static void CreateZip(ref string zipPath, List<KeyValuePair<string, string>> zipFiles,
            out List<string> notZip)
        {
            if (System.IO.File.Exists(zipPath))
            {
                throw new Exception("The file already exists.");
            }

            notZip = new List<string>();
            string dirPath = Path.GetDirectoryName(zipPath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (var item in zipFiles)
                {
                    if (!System.IO.File.Exists(item.Key))
                    {
                        notZip.Add(item.Key);
                    }

                    string fileName = string.Empty;
                    if (string.IsNullOrWhiteSpace(item.Value))
                    {
                        Path.GetFileName(item.Key);
                    }
                    else
                    {
                        var ext = Path.GetExtension(item.Key);
                        fileName = string.Format("{0}{1}", item.Value, ext);
                    }

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    ZipArchiveEntry readMeEntry = archive.CreateEntry(fileName);
                    using (Stream stream = readMeEntry.Open())
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(item.Key);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 更新压缩包
        /// </summary>
        /// <param name="zipPath"></param>
        /// <param name="zipFiles"></param>
        /// <param name="notZip"></param>
        /// <returns></returns>
        public static void UpdateZip(ref string zipPath, List<string> zipFiles, out List<string> notZip)
        {
            notZip = new List<string>();
            if (!System.IO.File.Exists(zipPath))
            {
                throw new FileNotFoundException();
            }

            // 向现有的压缩文件中添加文件
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                foreach (var item in zipFiles)
                {
                    if (!System.IO.File.Exists(item))
                    {
                        notZip.Add(item);
                    }

                    string fileName = Path.GetFileName(item);
                    ZipArchiveEntry readMeEntry = archive.CreateEntry(fileName);
                    using (Stream stream = readMeEntry.Open())
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(item);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }


        /// <summary>
        /// 展示压缩包所有文件
        /// </summary>
        /// <param name="zipPath"></param>
        /// <returns></returns>
        public static List<string> DisplayZipList(string zipPath)
        {
            List<string> list = new List<string>();
            // 列出压缩压缩文件
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
            {

                foreach (var zipArchiveEntry in archive.Entries)
                {
                    list.Add(zipArchiveEntry.FullName);
                }
            }
            return list;
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipPath"></param>
        /// <param name="output"></param>
        public static void ExtractZip(string zipPath, string output)
        {
            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(output);
            }
        }
    }
}