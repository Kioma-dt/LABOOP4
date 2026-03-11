using LABOOP4.Entities;
namespace LABOOP4.Factories
{
    internal class TransportProvider
    {
        Dictionary<string, ITransportFactory> _catalog;

        public TransportProvider(Dictionary<string, ITransportFactory> catalog)
        {
            _catalog = catalog;
        }
        public Transport GetTransport(string name)
        {
            if (!_catalog.ContainsKey(name))
            {
                throw new ArgumentException("No Such Transport!");
            }

            return _catalog[name].CreateTransport();
        }
    }

    internal interface ITransportFactory
    {
        public Transport CreateTransport();
    }

    internal class TrackFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Transport("Track", TransportType.Land, 15.0, 80);
        }
    }
    internal class TrainFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Transport("Train", TransportType.Land, 5.0, 60);
        }
    }
    internal class TankerFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Transport("Tanker", TransportType.Water, 2.0, 35);
        }
    }
    internal class AirshipFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Transport("Airship", TransportType.Air, 150.0, 850);
        }

    }

    internal class HelicopterFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Transport("Helicopter", TransportType.Air, 200.0, 250);
        }

    }

}
