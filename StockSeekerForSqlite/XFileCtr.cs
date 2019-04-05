using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XjsStock
{
    public class XFileCtr
    {
        public static void OutFile(String vPath, string vContent)
        {
            FileStream _Stream = new FileStream(vPath, FileMode.Create);//新建文件
            StreamWriter _Writer = new StreamWriter(_Stream,Encoding.Default);
            _Writer.WriteLine(vContent);
            _Writer.Close();
            _Stream.Close();
        }

        /// <summary>
        ///  读取文本文件的所有内容
        /// </summary>
        /// <param name="vFileName"></param>
        /// <returns></returns>
        public static String getContent(String vFileName)
        {
            StreamReader _Reader = new StreamReader(vFileName);
            String _Result = "";
            string line = _Reader.ReadLine();
            while (line != null)
            {
                _Result += line;
                line = _Reader.ReadLine();
            }
            _Reader.Close();
            return _Result;
        }

        /// <summary>
        ///  读取文本文件的所有内容
        /// </summary>
        /// <param name="vFileName"></param>
        /// <returns></returns>
        public static List<String> getContentList(String vFileName)
        {
            StreamReader _Reader = new StreamReader(vFileName,Encoding.Default);
            List<String> _Result = new List<String>();
            string line = _Reader.ReadLine();
            while (line != null)
            {
                if (!_Result.Contains(line))
                {
                    _Result.Add(line);
                }
                line = _Reader.ReadLine();
            }
            _Reader.Close();
            return _Result;
        }
    }
}