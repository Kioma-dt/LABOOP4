using LABOOP4.Factories;
using LABOOP4.Entities;

namespace LABOOP4.Use_Cases
{

    internal class LogisticSystem
    {
        readonly ITransportFactory _transportFactory;
        readonly ICargoFactory _cargoFactory;
        readonly List<Order> _orders = new List<Order>();
        public LogisticSystem(ITransportFactory transportFactory, ICargoFactory cargoFactory) 
        {
            _transportFactory = transportFactory;
            _cargoFactory = cargoFactory;
        }

        public Order RegisterOrder(IEnumerable<(string cargoName, int amount)> cargoBatches, 
            string transportName, int distance)
        {
            var batches = new List<(Cargo, int)>();
            foreach (var (cargoName, amount) in cargoBatches)
            {
                batches.Add((_cargoFactory.CreateCargo(cargoName),amount));
            }

            var transport = _transportFactory.CreateTransport(transportName);
            var order = new Order(batches, transport, distance);
            _orders.Add(order);
            return order;
        }
    }
}
