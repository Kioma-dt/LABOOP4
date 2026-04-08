using LABOOP4.Entities;
using System.Text.Json;

namespace LABOOP4.FileService.Catalog
{
    public interface ICatalogService
    {
        Dictionary<string, CargoInfo> GetCargoCatalog(string fileName);
        Dictionary<string, TransportInfo> GetTransportCatalog(string fileName);
    }
}
