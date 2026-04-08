using LABOOP4.Entities;
using System.Xml;
using System.Xml.Serialization;

namespace LABOOP4.FileService.Catalog
{
    public class CargoFullInfo
    {
        public string Name { get; set; }
        public CargoInfo CargoInfo { get; set; }
        public CargoFullInfo() { }
    }
    public class TransportFullInfo
    {
        public string Name { get; set; }
        public TransportInfo TransportInfo { get; set; }
        public TransportFullInfo() { }
    }
    public class XmlCatalogDTO
    {
        [XmlArrayItem("item")]
        public List<CargoFullInfo> Cargo { get; set; }

        [XmlArrayItem("item")]
        public List<TransportFullInfo> Transport { get; set; }

        public XmlCatalogDTO() { }
    }
    public class XmlCatalogService : ICatalogService
    {
        XmlCatalogDTO? Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(XmlCatalogDTO));

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var data = serializer.Deserialize(stream) as XmlCatalogDTO;
                return data;
            }
        }
        public Dictionary<string, CargoInfo> GetCargoCatalog(string fileName)
        {
            try
            {
                var data = Load(fileName);

                if (data is null)
                {
                    throw new XmlException();
                }

                return data.Cargo.ToDictionary(c => c.Name, c => c.CargoInfo) ?? new Dictionary<string, CargoInfo>();
            }
            catch (IOException ex)
            {
                throw new Exception($"Can't Read From File {fileName}: {ex.Message}");
            }
            catch (XmlException ex)
            {
                throw new Exception($"Can't Desiarize Xml File: {ex.Message}");
            }
        }

        public Dictionary<string, TransportInfo> GetTransportCatalog(string fileName)
        {
            try
            {
                var data = Load(fileName);

                if (data is null)
                {
                    throw new XmlException();
                }

                return data.Transport.ToDictionary(t => t.Name, t => t.TransportInfo) ?? new Dictionary<string, TransportInfo>();
            }
            catch (IOException ex)
            {
                throw new Exception($"Can't Read From File {fileName}: {ex.Message}");
            }
            catch (XmlException ex)
            {
                throw new Exception($"Can't Desiarize Xml File: {ex.Message}");
            }
        }
    }
}
