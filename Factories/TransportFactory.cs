using LABOOP4.Entities;
namespace LABOOP4.Factories
{
    internal interface ITransportFactoryProvider
    {
        public ITransportFactory GetFactory(TransportType type);
    }
    internal interface ITransportFactory
    {
        public Transport CreateTransportByName(string name);
        public List<Transport> CreateAllTransports();
    }

    internal class AirTransportFactory : ITransportFactory
    {
        Dictionary<string, TransportInfo> _catalog;
        public AirTransportFactory(Dictionary<string, TransportInfo> catalog)
        {
            _catalog = catalog;
        }

        public Transport CreateTransportByName(string name)
        {
            if (!_catalog.ContainsKey(name))
            {
                throw new Exception($"No Such Transport: {name} in Catalog!");
            }
            if (_catalog[name].Type != TransportType.Air) 
            {
                throw new Exception($"No Transport {name} in not Air!");
            }

            return new AirTransport(name, _catalog[name]);
        }

        public List<Transport> CreateAllTransports() 
        {
            var transports = new List<Transport>();

            foreach (var (name, info) in _catalog) 
            {
                if (info.Type == TransportType.Air)
                {
                    transports.Add(new  AirTransport(name, info));
                }
            }

            return transports;
        }
    }
    internal class LandTransportFactory : ITransportFactory
    {
        Dictionary<string, TransportInfo> _catalog;
        public LandTransportFactory(Dictionary<string, TransportInfo> catalog)
        {
            _catalog = catalog;
        }

        public Transport CreateTransportByName(string name)
        {
            if (!_catalog.ContainsKey(name))
            {
                throw new Exception($"No Such Transport: {name} in Catalog!");
            }
            if (_catalog[name].Type != TransportType.Land)
            {
                throw new Exception($"No Transport {name} in not Air!");
            }

            return new LandTransport(name, _catalog[name]);
        }

        public List<Transport> CreateAllTransports()
        {
            var transports = new List<Transport>();

            foreach (var (name, info) in _catalog)
            {
                if (info.Type == TransportType.Land)
                {
                    transports.Add(new LandTransport(name, info));
                }
            }

            return transports;
        }
    }
    internal class WaterTransportFactory : ITransportFactory
    {
        Dictionary<string, TransportInfo> _catalog;
        public WaterTransportFactory(Dictionary<string, TransportInfo> catalog)
        {
            _catalog = catalog;
        }

        public Transport CreateTransportByName(string name)
        {
            if (!_catalog.ContainsKey(name))
            {
                throw new Exception($"No Such Transport: {name} in Catalog!");
            }
            if (_catalog[name].Type != TransportType.Water)
            {
                throw new Exception($"No Transport {name} in not Air!");
            }

            return new WaterTransport(name, _catalog[name]);
        }
        public List<Transport> CreateAllTransports()
        {
            var transports = new List<Transport>();

            foreach (var (name, info) in _catalog)
            {
                if (info.Type == TransportType.Water)
                {
                    transports.Add(new WaterTransport(name, info));
                }
            }

            return transports;
        }
    }

    internal class TransportFactoryProvider : ITransportFactoryProvider 
    {
        readonly Dictionary<TransportType, ITransportFactory> _catalog;

        public TransportFactoryProvider(Dictionary<TransportType, ITransportFactory> catalog)
        {
            _catalog = catalog;
        }

        public ITransportFactory GetFactory(TransportType type)
        {
            if (!_catalog.ContainsKey(type))
            {
                throw new ArgumentException($"No Type: {type} Implementation");
            }

            return _catalog[type];
        }
    }

    //internal class TruckFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Truck();
    //    }
    //}
    //internal class TrainFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Train();
    //    }
    //}
    //internal class TankerFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Tanker();
    //    }
    //}
    //internal class AirplaneFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Airplane();
    //    }

    //}

    //internal class HelicopterFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Helicopter();
    //    }

    //}



    //internal class Truck : Transport
    //{
    //    public Truck()
    //        : base("Грузовик", TransportType.Land, 15.0, 80)
    //    {
    //    }
    //}
    //internal class Train : Transport
    //{
    //    public Train()
    //        : base("Поезд", TransportType.Land, 5.0, 60)
    //    {
    //    }
    //}
    //internal class Tanker : Transport
    //{
    //    public Tanker()
    //        : base("Танкер", TransportType.Water, 2.0, 35)
    //    {
    //    }
    //}
    //internal class Airplane : Transport
    //{
    //    public Airplane()
    //        : base("Самолет", TransportType.Air, 150.0, 850)
    //    {
    //    }
    //}
    //internal class Helicopter : Transport
    //{
    //    public Helicopter()
    //        : base("Вертолет", TransportType.Air, 200.0, 250)
    //    {
    //    }
    //}

}
