using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace XjsStock.Bean
{
    [Serializable]
    public abstract class BaseBean
    {
        public BaseBean() { }
        public string Xml()
        {
            Encoding code = Encoding.GetEncoding("gb2312");
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = code;
            settings.Indent = true;
            var memoryStream = new MemoryStream();
            using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
            {
                //去除默认命名空间xmlns:xsd和xmlns:xsi
                var serializerNamespaces = new XmlSerializerNamespaces();
                serializerNamespaces.Add("", "");
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this, serializerNamespaces);
            }
            return code.GetString(memoryStream.ToArray());
        }
        /// <summary>
        /// 欢迎实现
        /// </summary>
        /// <returns></returns>
        public string Json()
        {
            return string.Empty;
        }
    }
}
