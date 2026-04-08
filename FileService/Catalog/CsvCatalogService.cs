using LABOOP4.Entities;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace LABOOP4.FileService.Catalog
{
    public class CsvCatalogDTO
    {
        public Dictionary<string, CargoInfo> Cargo { get; set; }
        public Dictionary<string, TransportInfo> Transport { get; set; }
    }
    public class CsvCatalogService : ICatalogService
    {
        CsvCatalogDTO? Load(string fileName)
        {
            List<string> linesList = new List<string>();
                           

            using (var reader = new StreamReader(fileName, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    linesList.Add(line);
                }
            }

            var lines = linesList.Skip(1)
                            .Where(line => !String.IsNullOrEmpty(line))
                            .Select(line => line.Split(','))
                            .ToList();
            var Cargo = new Dictionary<string, CargoInfo>();
            var Transport = new Dictionary<string, TransportInfo>();
            try
            {
                foreach (var line in lines)
                {
                    if (line[0].Trim() == "Cargo")
                    {
                        var name = line[1].Trim();
                        var mass = double.Parse(line[2], CultureInfo.InvariantCulture);
                        var cost = double.Parse(line[3], CultureInfo.InvariantCulture);

                        Cargo[name] = new CargoInfo { Mass = mass, CostPerKg = cost };
                    }
                    else if (line[0].Trim() == "Transport")
                    {
                        var name = line[1].Trim();
                        var type = line[4].Trim();
                        var cost = double.Parse(line[5], CultureInfo.InvariantCulture);
                        var speed = double.Parse(line[6], CultureInfo.InvariantCulture);

                        Transport[name] = new TransportInfo { Type = type, CostPerKm = cost, Speed = speed };
                    }
                    else throw new Exception();
                }

                return new CsvCatalogDTO { Cargo = Cargo, Transport = Transport };
            }
            catch (Exception ex)
            {
                throw new JsonException();
            }
        }
        public Dictionary<string, CargoInfo> GetCargoCatalog(string fileName)
        {
            try
            {
                var data = Load(fileName);

                if (data is null)
                {
                    throw new JsonException();
                }

                return data.Cargo;
            }
            catch (IOException ex)
            {
                throw new Exception($"Can't Read From File {fileName}: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Can't Parse Csv File: {ex.Message}");
            }
        }

        public Dictionary<string, TransportInfo> GetTransportCatalog(string fileName)
        {
            try
            {
                var data = Load(fileName);

                if (data is null)
                {
                    throw new JsonException();
                }

                return data.Transport;
            }
            catch (IOException ex)
            {
                throw new Exception($"Can't Read From File {fileName}: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Can't Parse Csv File: {ex.Message}");
            }
        }
    }
}
