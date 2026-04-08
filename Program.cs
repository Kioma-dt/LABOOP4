using LABOOP4.Entities;
using LABOOP4.Factories;
using LABOOP4.Use_Cases;
using LABOOP4.FileService.Catalog;

namespace LABOOP4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ICatalogService catalogService = new CsvCatalogService();

                var cargoCatalog = catalogService.GetCargoCatalog("Files//catalog.csv");
                var transportCatalog = catalogService.GetTransportCatalog("Files//catalog.csv");

                foreach (var (cargoName, cargoInfo) in cargoCatalog)
                { 
                    Console.WriteLine($"{cargoName} {cargoInfo.CostPerKg} {cargoInfo.Mass}");
                }
                foreach (var (transportName, transportInfo) in transportCatalog)
                {
                    Console.WriteLine($"{transportName} {transportInfo.Type} {transportInfo.CostPerKm} {transportInfo.Speed}");
                }

                var transportFactories = new Dictionary<TransportType, ITransportFactory>()
                {
                    { TransportType.Air, new AirTransportFactory(transportCatalog) },
                    { TransportType.Land, new LandTransportFactory(transportCatalog) },
                    { TransportType.Water, new WaterTransportFactory(transportCatalog) }
                };

                ITransportFactory transportFactory = new TransportFactoryProvider(transportFactories).GetFactory(TransportType.Air);

                var sam = transportFactory.CreateTransportByName("Airplane");

                Console.WriteLine($"{sam.Name} {sam.Type} {sam.CostPerKm} {sam.Speed}");

                //var cargoCatalog = new Dictionary<string, CargoInfo>()
                //{
                //    { "Электроника", new CargoInfo(1.5, 50) },
                //    { "Одежда", new CargoInfo(0.8, 20) },
                //    { "Оборудование", new CargoInfo(120.0, 15) },
                //    { "Скоропортящиеся продукты", new CargoInfo(10.0, 100) }
                //};

                //var transportCatalog = new Dictionary<string, TransportInfo>()
                //{
                //    { "Грузовик", new TransportInfo(TransportType.Land, 15.0, 80) },
                //    { "Поезд", new TransportInfo(TransportType.Land, 5.0, 60) },
                //    { "Танкер", new TransportInfo(TransportType.Water, 2.0, 35) },
                //    { "Самолет", new TransportInfo(TransportType.Air, 150.0, 850) },
                //    { "Вертолет", new TransportInfo(TransportType.Air, 200.0, 250) }
                //};

                //var cargoFactory = new CargoFactory(cargoCatalog);

                //var transportFactories = new Dictionary<TransportType, ITransportFactory>()
                //{
                //    { TransportType.Air, new AirTransportFactory(transportCatalog) },
                //    { TransportType.Land, new LandTransportFactory(transportCatalog) },
                //    { TransportType.Water, new WaterTransportFactory(transportCatalog) }
                //};

                //var transportFactoryProvider = new TransportFactoryProvider(transportFactories);

                //var logisticSystem = new LogisticSystem(transportFactoryProvider, cargoFactory);

                //logisticSystem.RegisterOrder(
                //    new List<(string, int)>
                //    {
                //        ("Электроника", 10),
                //        ("Одежда", 50)
                //    },
                //    TransportType.Land,
                //    500,
                //    "Грузовик"
                //);

                //logisticSystem.RegisterOrder(
                //    new List<(string, int)>
                //    {
                //        ("Скоропортящиеся продукты", 20)
                //    },
                //    TransportType.Air,
                //    1200
                //);

                //foreach (var order in logisticSystem.Orders)
                //{
                //    Console.WriteLine($"Стоимость: {order.GetCost():F2}");
                //    Console.WriteLine($"Время доставки: {order.GetDeliveryTime():F2} часов");
                //    Console.WriteLine();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
