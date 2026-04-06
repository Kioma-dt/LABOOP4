using LABOOP4.Entities;
using System.Text.Json;

namespace LABOOP4.FileService
{
    public interface ICatalogService
    {
        Dictionary<string, CargoInfo> GetCargoCatalog(string fileName);
        Dictionary<string, TransportInfo> GetTransportCatalog(string fileName);
    }

    public class JsonCatalogDTO
    {
        public Dictionary<string, CargoInfo> Cargo { get; set; }
        public Dictionary<string, TransportInfo> Transport { get; set; }
    }
    public class JsonCatalogService : ICatalogService
    {
        JsonCatalogDTO? Load(string fileName)
        {
            var json = File.ReadAllText(fileName);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<JsonCatalogDTO>(json, options);
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
                throw new Exception($"Can't Desiarize Json File: {ex.Message}");
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
                throw new Exception($"Can't Desiarize Json File: {ex.Message}");
            }
        }
    }
}
