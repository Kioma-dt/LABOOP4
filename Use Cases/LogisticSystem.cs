using LABOOP4.Factories;
using LABOOP4.Entities;

namespace LABOOP4.Use_Cases
{

    internal class LogisticSystem
    {
        readonly ITransportFactoryProvider _transportFactoryProvider;
        readonly ICargoFactory _cargoFactory;
        readonly List<Order> _orders = new List<Order>();
        public LogisticSystem(ITransportFactoryProvider transportFactoryProvider, ICargoFactory cargoFactory) 
        {
            _transportFactoryProvider = transportFactoryProvider;
            _cargoFactory = cargoFactory;
        }

        public void RegisterOrder(IEnumerable<(string cargoName, int amount)> cargoBatches, 
            int distance,
            TransportType transportType = TransportType.None,
            string? transportName = null)
        {
            _orders.Clear();
            var batches = new List<(Cargo, int)>();
            foreach (var (cargoName, amount) in cargoBatches)
            {
                batches.Add((_cargoFactory.CreateCargo(cargoName),amount));
            }

            if (transportType == TransportType.None)
            {
                foreach(var factory in _transportFactoryProvider.GetAllFactories())
                {
                    foreach (var transport in factory.CreateAllTransports())
                    {
                        var order = new Order(batches, transport, distance);
                        _orders.Add(order);
                    }
                }
            }
            var transportFactory = _transportFactoryProvider.GetFactory(transportType);

            if (transportName is not null)
            {
                var transport = transportFactory.CreateTransportByName(transportName);
                var order = new Order(batches, transport, distance);
                _orders.Add(order);
            }
            else
            {
                foreach (var transport in transportFactory.CreateAllTransports())
                {
                    var order = new Order(batches, transport, distance);
                    _orders.Add(order);
                }
            }
            
        }

        public List<Order> Orders 
        {
            get => new List<Order>(_orders); 
        }
    }
}
