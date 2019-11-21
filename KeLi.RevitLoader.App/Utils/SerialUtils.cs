using System.IO;
using System.Xml.Serialization;

namespace KeLi.RevitLoader.App.Utils
{
    public class SerialUtils
    {
        public static T GetObject<T>(string xmlFile)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(xmlFile))
                return (T)serializer.Deserialize(reader);
        }

        public static void ToFile(string xmlFile, object obj)
        {
            var xs = new XmlSerializer(obj.GetType());

            using (var fs = new FileStream(xmlFile, FileMode.Create))
                xs.Serialize(fs, obj);
        }
    }
}