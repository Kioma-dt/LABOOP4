using LABOOP4.Entities;
using LABOOP4.Factories;
using LABOOP4.Use_Cases;
using LABOOP4.FileService.Catalog;
using LABOOP4.FileService.Save;
using System.Security.Cryptography;
using System.Globalization;
using LABOOP4.Sorts;
using LABOOP4.Filters;

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

                ICatalogService? catalogService = null;
                Dictionary<string, CargoInfo>? cargoCatalog = null;
                Dictionary<string, TransportInfo>? transportCatalog = null;


                while (catalogService is null)
                {
                    try
                    {
                        Console.WriteLine("Input Catalog File: ");
                        var catalogFile = Console.ReadLine();


                        if (catalogFile is null) throw new Exception("Inputed Null String");


                        if (catalogFile == "Def")
                        {
                            catalogService = new JsonCatalogService();
                            cargoCatalog = catalogService.GetCargoCatalog("Files//catalog.json");
                            transportCatalog = catalogService.GetTransportCatalog("Files//catalog.json");

                            break;
                        }

                        if (catalogFile.EndsWith(".json")) catalogService = new JsonCatalogService();
                        else if (catalogFile.EndsWith(".xml")) catalogService = new XmlCatalogService();
                        else if (catalogFile.EndsWith(".csv")) catalogService = new CsvCatalogService();
                        else throw new Exception("Invalid Catalog Format");

                        cargoCatalog = catalogService.GetCargoCatalog(catalogFile);
                        transportCatalog = catalogService.GetTransportCatalog(catalogFile);
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine($"Catalog Error: {ex.Message}\nPress Any Key...");
                        catalogService = null;
                        cargoCatalog = null;
                        transportCatalog = null;
                        Console.ReadLine();
                        Console.Clear();
                    }
                                        
                }

                Console.Clear();
                var cargoBatches = new List<(string CargoName, int Amount)>();

                while (true)
                {
                    try
                    {
                        foreach (var (cargo, info) in cargoCatalog)
                        {
                            Console.WriteLine($"Cargo: {cargo} {info}");
                        }
                        Console.WriteLine("Input Cargo Batch Name: ");
                        var cargoName = Console.ReadLine();

                        if (cargoName == "Def")
                        {
                            cargoBatches = new List<(string, int)>
                                {
                                    ("Electronic", 10),
                                    ("Cloth", 50)
                                };

                            break;
                        }

                        if (cargoName is null || !cargoCatalog.ContainsKey(cargoName))
                        {
                            throw new Exception("No Such Cargo!");
                        }

                        Console.WriteLine("Input Cargo Amount: ");
                        var amount = UInt32.Parse(Console.ReadLine() ?? String.Empty);

                        cargoBatches.Add(new (cargoName, Convert.ToInt32(amount)));

                        Console.WriteLine("Input A to add another Batch");
                        var answer = Console.ReadLine();

                        if (answer is null || answer.ToLower() != "a")
                        {
                            break;
                        }
                        Console.Clear();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Cargo Batch Error: Amount Should Be Pos Int\nPress Any Key...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Cargo Batch Error: {ex.Message}\nPress Any Key...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                Console.Clear();


                var transportType = TransportType.None;
                string? transportName = null;
                while (true)
                {
                    try
                    {
                        foreach (var (transport, info) in transportCatalog)
                        {
                            Console.WriteLine($"Transport: {transport} {info}");
                        }
                        Console.WriteLine("Input Transport Type (Enter for nothing): ");
                        var transportTypeString = Console.ReadLine();

                        if (String.IsNullOrEmpty(transportTypeString))
                        {
                            break;
                        }

                        transportType = transportTypeString.ToTransportType();

                        Console.WriteLine("Input Transport Name (Enter for nothing): ");
                        transportName = Console.ReadLine();

                        if (String.IsNullOrEmpty(transportName)) 
                        {
                            transportName = null;
                            break;
                        }

                        if(!transportCatalog.ContainsKey(transportName)) 
                        {
                            throw new Exception("No Such Transport");
                        }

                        if (transportCatalog[transportName].Type.ToTransportType() != transportType)
                        {
                            throw new Exception("Transport Type Don't Match");
                        }
                        break;
                        Console.Clear();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Trasnport Error: {ex.Message}\nPress Any Key...");
                        Console.ReadLine();
                        Console.Clear();
                        transportType = TransportType.None;
                        transportName = null;
                    }
                }
                Console.Clear();

                Console.Clear();
                var distance = 0;

                while (true)
                {
                    try
                    {
                       
                        Console.WriteLine("Input Distance: ");
                        distance = Convert.ToInt32(UInt32.Parse(Console.ReadLine() ?? String.Empty));

                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"Distance Error: Distance Should Be Pos Int\nPress Any Key...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Distance Error Error: {ex.Message}\nPress Any Key...");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                Console.Clear();

                var cargoFactory = new CargoFactory(cargoCatalog);

                    var transportFactories = new Dictionary<TransportType, ITransportFactory>()
                    {
                        { TransportType.Air, new AirTransportFactory(transportCatalog) },
                        { TransportType.Land, new LandTransportFactory(transportCatalog) },
                        { TransportType.Water, new WaterTransportFactory(transportCatalog) }
                    };


                    var transportFactoryProvider = new TransportFactoryProvider(transportFactories);

                    var logisticSystem = new LogisticSystem(transportFactoryProvider, cargoFactory);

                logisticSystem.RegisterOrder(cargoBatches, distance, transportType, transportName);


                //    //IOrderSort sort = new DeliveryCostSort();
                //    OrdersFilter filter = new OrdersFilter();
                //    filter.AddFilter(new TransportTypeFilter(TransportType.Air));

                var orders = logisticSystem.Orders;
                //    orders = filter.ApplyFilters(orders);
                //    //orders = sort.Sort(orders);
                //    var saveDTOs = new List<SaveDTO>();
                foreach (var order in orders)
                {
                    Console.WriteLine($"Стоимость: {order.GetCost():F2}");
                    Console.WriteLine($"Время доставки: {order.GetDeliveryTime():F2} часов");
                    Console.WriteLine();
                    //saveDTOs.Add(new SaveDTO(order));
                }
                //    SaveService saveService = new CsvSaveService();
                //    //saveService = new EncryptionDecorator(saveService, key, iv);
                //    //saveService = new ZipDecorator(saveService, "Files//archive.zip");
                //    saveService.Save(saveDTOs, "Files//output.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}


//    //logisticSystem.RegisterOrder(
//    //    new List<(string, int)>
//    //    {
//    //        ("Электроника", 10),
//    //        ("Одежда", 50)
//    //    },
//    //    500,
//    //    TransportType.Land,
//    //    "Грузовик"
//    //);
