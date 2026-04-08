using LABOOP4.Entities;
namespace LABOOP4.Factories
{
    public interface ITransportFactoryProvider
    {
        public ITransportFactory GetFactory(TransportType type);
        public IEnumerable<ITransportFactory> GetAllFactories();
    }

    public class TransportFactoryProvider : ITransportFactoryProvider
    {
        readonly Dictionary<TransportType, ITransportFactory> _catalog;

        public TransportFactoryProvider(Dictionary<TransportType, ITransportFactory> catalog)
        {
            _catalog = catalog;
        }

        public IEnumerable<ITransportFactory> GetAllFactories()
        {
            return _catalog.Values.ToList();
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


    public interface ITransportFactory
    {
        public Transport CreateTransportByName(string name);
        public List<Transport> CreateAllTransports();
    }

    public class AirTransportFactory : ITransportFactory
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
            if (_catalog[name].Type.ToTransportType() != TransportType.Air) 
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
                if (info.Type.ToTransportType() == TransportType.Air)
                {
                    transports.Add(new  AirTransport(name, info));
                }
            }

            return transports;
        }
    }
    public class LandTransportFactory : ITransportFactory
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
            if (_catalog[name].Type.ToTransportType() != TransportType.Land)
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
                if (info.Type.ToTransportType() == TransportType.Land)
                {
                    transports.Add(new LandTransport(name, info));
                }
            }

            return transports;
        }
    }
    public class WaterTransportFactory : ITransportFactory
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
            if (_catalog[name].Type.ToTransportType() != TransportType.Water)
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
                if (info.Type.ToTransportType() == TransportType.Water)
                {
                    transports.Add(new WaterTransport(name, info));
                }
            }

            return transports;
        }
    }


    //public class TruckFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Truck();
    //    }
    //}
    //public class TrainFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Train();
    //    }
    //}
    //public class TankerFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Tanker();
    //    }
    //}
    //public class AirplaneFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Airplane();
    //    }

    //}

    //public class HelicopterFactory : ITransportFactory
    //{
    //    public Transport CreateTransport()
    //    {
    //        return new Helicopter();
    //    }

    //}



    //public class Truck : Transport
    //{
    //    public Truck()
    //        : base("Грузовик", TransportType.Land, 15.0, 80)
    //    {
    //    }
    //}
    //public class Train : Transport
    //{
    //    public Train()
    //        : base("Поезд", TransportType.Land, 5.0, 60)
    //    {
    //    }
    //}
    //public class Tanker : Transport
    //{
    //    public Tanker()
    //        : base("Танкер", TransportType.Water, 2.0, 35)
    //    {
    //    }
    //}
    //public class Airplane : Transport
    //{
    //    public Airplane()
    //        : base("Самолет", TransportType.Air, 150.0, 850)
    //    {
    //    }
    //}
    //public class Helicopter : Transport
    //{
    //    public Helicopter()
    //        : base("Вертолет", TransportType.Air, 200.0, 250)
    //    {
    //    }
    //}

}
