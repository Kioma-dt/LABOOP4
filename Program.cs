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


                        if (catalogFile.ToLower() == "def")
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

                        if (cargoName.ToLower() == "def")
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

                IOrderSort? sort = null;
                OrdersFilter filter = new OrdersFilter();

                Console.WriteLine("Press S to Add Sort");
                var sortAnswer = Console.ReadLine();

                if (sortAnswer.ToLower() == "s")
                {
                    bool sortContinue = true;
                    while (sortContinue)
                    {
                        Console.Clear();
                        Console.WriteLine("1. Delivery Cost Sort");
                        Console.WriteLine("2. Delivery Time Sort");
                        Console.WriteLine("3. Transport Type Sort");
                        Console.WriteLine("4. Transport Name Sort");
                        var answer = Console.ReadLine();

                        switch (answer)
                        {
                            case "1":
                            {
                                    sortContinue = false;
                                    sort = new DeliveryCostSort();
                                    break;
                            }
                            case "2":
                                {
                                    sortContinue = false;
                                    sort = new DeliveryTimeSort();
                                    break;
                                }
                            case "3":
                                {
                                    sortContinue = false;
                                    sort = new TransportTypeSort();
                                    break;
                                }
                            case "4":
                                {
                                    sortContinue = false;
                                    sort = new TransportNameSort();
                                    break;
                                }
                            default:
                                {
                                    sortContinue = true;
                                    Console.WriteLine("Wrong Option Format!\nPress Any Key...");
                                    Console.ReadLine();
                                    Console.Clear();
                                    break;
                                }
                        }
                    }
                }

                var continueFilter = true;

                while (continueFilter) 
                {
                    Console.Clear();
                    Console.WriteLine("Press F to add Filter");
                    var filterAnwer = Console.ReadLine();

                    if (filterAnwer.ToLower() != "f") 
                    {
                        continueFilter = false;
                        break;
                    }

                    var continueChoose = true;
                    var filterName = String.Empty;
                    while (continueChoose)
                    {
                        Console.Clear();
                        Console.WriteLine("1. Delivery Cost Filter");
                        Console.WriteLine("2. Delivery Time Filrer");
                        Console.WriteLine("3. Transport Type Filter");
                        var answer = Console.ReadLine();



                        switch (answer)
                        {
                            case "1":
                                {
                                    continueChoose = false;
                                    filterName = "Cost";
                                    break;
                                }
                            case "2":
                                {
                                    continueChoose = false;
                                    filterName = "Time";
                                    break;
                                }
                            case "3":
                                {
                                    continueChoose = false;
                                    filterName = "Type";
                                    break;
                                }
                            default:
                                {
                                    continueChoose = true;
                                    Console.WriteLine("Wrong Option Format!\nPress Any Key...");
                                    Console.ReadLine();
                                    Console.Clear();
                                    break;
                                }
                        }

                    }

                    if (filterName == "Type")
                    {
                        continueChoose = true;
                        while (continueChoose) 
                        {
                            Console.Clear();
                            Console.WriteLine("1. Land");
                            Console.WriteLine("2. Air");
                            Console.WriteLine("3. Water");
                            var answer = Console.ReadLine();



                            switch (answer)
                            {
                                case "1":
                                    {
                                        continueChoose = false;
                                        filter.AddFilter(new TransportTypeFilter(TransportType.Land));
                                        break;
                                    }
                                case "2":
                                    {
                                        continueChoose = false;
                                        filter.AddFilter(new TransportTypeFilter(TransportType.Air));
                                        break;
                                    }
                                case "3":
                                    {
                                        continueChoose = false;
                                        filter.AddFilter(new TransportTypeFilter(TransportType.Water));
                                        break;
                                    }
                                default:
                                    {
                                        continueChoose = true;
                                        Console.WriteLine("Wrong Option Format!\nPress Any Key...");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;
                                    }
                            }
                        }
                        
                    }
                    else
                    {
                        var compType = ComparisonType.Greater;
                        continueChoose = true;
                        while (continueChoose)
                        {
                            Console.Clear();
                            Console.WriteLine("1. Greater");
                            Console.WriteLine("2. Greater Or Equal");
                            Console.WriteLine("3. Equal");
                            Console.WriteLine("4. Less Or Equal");
                            Console.WriteLine("5. Less");
                            var answer = Console.ReadLine();



                            switch (answer)
                            {
                                case "1":
                                    {
                                        continueChoose = false;
                                        compType = ComparisonType.Greater;
                                        break;
                                    }
                                case "2":
                                    {
                                        continueChoose = false;
                                        compType = ComparisonType.GreaterOrEqual;
                                        break;
                                    }
                                case "3":
                                    {
                                        continueChoose = false;
                                        compType = ComparisonType.Equal;
                                        break;
                                    }
                                case "4":
                                    {
                                        continueChoose = false;
                                        compType = ComparisonType.LessOrEqual;
                                        break;
                                    }
                                case "5":
                                    {
                                        continueChoose = false;
                                        compType = ComparisonType.Less;
                                        break;
                                    }
                                default:
                                    {
                                        continueChoose = true;
                                        Console.WriteLine("Wrong Option Format!\nPress Any Key...");
                                        Console.ReadLine();
                                        Console.Clear();
                                        break;
                                    }
                            }
                        }

                        continueChoose = true;
                        double value = 0;
                        while (continueChoose)
                        {
                            try
                            {
                                Console.Clear();
                                Console.WriteLine("Input Compare Value: ");
                                value = Double.Parse(Console.ReadLine() ?? String.Empty);
                                continueChoose = false;
                            }
                            catch (Exception)
                            {
                                Console.WriteLine($"Distance Error: Distance Should Be Pos Int\nPress Any Key...");
                                Console.ReadLine();
                                Console.Clear();
                                continueChoose = true;
                            }
                            
                        }

                        if (filterName == "Cost")
                        {
                            filter.AddFilter(new DeliveryCostFilter(value, compType));
                        }
                        else
                        {
                            filter.AddFilter(new DeliveryTimeFilter(value, compType));
                        }
                    }

                }

                Console.Clear();
                var orders = logisticSystem.Orders;
                orders = filter.ApplyFilters(orders);
                orders = sort?.Sort(orders) ?? orders;
                var saveDTOs = new List<SaveDTO>();
                foreach (var order in orders)
                {
                    Console.WriteLine($"Transport Name: {order.TrasnportName}");
                    Console.WriteLine($"Transport Type: {order.TrasnportType}");
                    Console.WriteLine($"Delivery Cost: {order.GetCost():F2}");
                    Console.WriteLine($"Delivery Time: {order.GetDeliveryTime():F2} hours");
                    Console.WriteLine();
                    saveDTOs.Add(new SaveDTO(order));
                }
                Console.WriteLine("Press Any Key...");
                Console.ReadLine();



                SaveService? saveService = null;
                string? saveFile = null;
                //    //
                //    
                //   

                while (saveService is null)
                {
                    try
                    {
                        Console.WriteLine("Input Save File: ");
                        saveFile = Console.ReadLine();


                        if (saveFile is null) throw new Exception("Inputed Null String");


                        if (saveFile.ToLower() == "def")
                        {
                            saveService = new JsonSaveService();
                            saveFile = "Files//output.json";
                            break;
                        }

                        if (saveFile.EndsWith(".json")) saveService = new JsonSaveService();
                        else if (saveFile.EndsWith(".csv")) saveService = new CsvSaveService();
                        else throw new Exception("Invalid Save File Format");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Save File Error: {ex.Message}\nPress Any Key...");
                        saveService = null;
                        saveFile = null;
                        Console.ReadLine();
                        Console.Clear();
                    }

                }

                Console.Clear();
                Console.WriteLine("Press E to Encrypt");
                var encAnswer = Console.ReadLine();

                if (encAnswer.ToLower() == "e") 
                {
                    saveService = new EncryptionDecorator(saveService, key, iv);
                }

                Console.Clear();
                Console.WriteLine("Press Z to Zip");
                var zipAnswer = Console.ReadLine();

                if (zipAnswer.ToLower() == "z")
                {
                    string? zipFile = null;
                    while (zipFile is null)
                    {
                        try
                        {
                            Console.WriteLine("Input Zip File: ");
                            zipFile = Console.ReadLine();


                            if (zipFile is null) throw new Exception("Inputed Null String");


                            if (zipFile.ToLower() == "def")
                            {
                                zipFile = "Files//archive.zip";
                                break;
                            }

                            if (!zipFile.EndsWith(".zip")) throw new Exception("Invalid Zip File Format");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Zip File Error: {ex.Message}\nPress Any Key...");
                            zipFile = null;
                            Console.ReadLine();
                            Console.Clear();
                        }

                    }

                    saveService = new ZipDecorator(saveService, zipFile);
                }

                saveService.Save(saveDTOs, saveFile);
                Console.Clear();
                Console.WriteLine("Saved!!!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
