using LABOOP4.Entities;
using LABOOP4.Factories;
using LABOOP4.Use_Cases;
using LABOOP4.FileService.Catalog;
using LABOOP4.FileService.Save;
using System.Security.Cryptography;

namespace LABOOP4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using var aes = Aes.Create();

                byte[] key = aes.Key;
                byte[] iv = aes.IV;
                ICatalogService catalogService = new JsonCatalogService();

                var cargoCatalog = catalogService.GetCargoCatalog("Files//catalog.json");
                var transportCatalog = catalogService.GetTransportCatalog("Files//catalog.json");

                var cargoFactory = new CargoFactory(cargoCatalog);

                var transportFactories = new Dictionary<TransportType, ITransportFactory>()
                {
                    { TransportType.Air, new AirTransportFactory(transportCatalog) },
                    { TransportType.Land, new LandTransportFactory(transportCatalog) },
                    { TransportType.Water, new WaterTransportFactory(transportCatalog) }
                };


                var transportFactoryProvider = new TransportFactoryProvider(transportFactories);

                var logisticSystem = new LogisticSystem(transportFactoryProvider, cargoFactory);

                logisticSystem.RegisterOrder(
                    new List<(string, int)>
                    {
                        ("Электроника", 10),
                        ("Одежда", 50)
                    },
                    500,
                    TransportType.Land,
                    "Грузовик"
                );

                logisticSystem.RegisterOrder(
                    new List<(string, int)>
                    {
                        ("Электроника", 10),
                        ("Одежда", 50)
                    },
                    100,
                    TransportType.Air
                );

                var saveDTOs = new List<SaveDTO>();
                foreach (var order in logisticSystem.Orders)
                {
                    Console.WriteLine($"Стоимость: {order.GetCost():F2}");
                    Console.WriteLine($"Время доставки: {order.GetDeliveryTime():F2} часов");
                    Console.WriteLine();
                    saveDTOs.Add(new SaveDTO(order));
                }
                SaveService saveService = new JsonSaveService();
                saveService = new EncryptionDecorator(saveService, key, iv);
                saveService = new ZipDecorator(saveService, "Files//archive.zip");
                saveService.Save(saveDTOs, "Files//output.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
