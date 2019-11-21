using System.Xml.Serialization;

namespace KeLi.RevitLoader.App.Entities
{
    public class AddIn
    {
        public string Name { get; set; }

        public string Assembly { get; set; }

        public string AddInId { get; set; }

        public string FullClassName { get; set; }

        public string VendorId { get; set; }

        public string VendorDescription { get; set; }

        [XmlAttribute]
        public string Type { get; set; }
    }
}