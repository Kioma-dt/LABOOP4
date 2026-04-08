using System.Collections.Generic;

namespace LABOOP4.Entities
{
    public class Order
    {
        readonly List<CargoBatch> _cargoBatches;
        readonly Transport _transport;
        readonly int _distance;

        public Order(List<(Cargo, int)> cargoBatches, Transport transport, int distance)
        {
            _cargoBatches = new List<CargoBatch>();
            foreach (var (cargo, amount) in cargoBatches)
            {
                _cargoBatches.Add(new CargoBatch(cargo, amount));
            }

            _transport = transport;
            _distance = distance;
        }

        public string TrasnportName => _transport.Name;
        public TransportType TrasnportType => _transport.Type;
        public int Distance => _distance;
        public double Cost => GetCost();
        public double DeliveryTime => GetDeliveryTime();

        public double GetCost()
        {
            double batchesCost = _cargoBatches.Sum(x => x.GetCost());
            double deliveryCost = _transport.CostPerKm * _distance;

            return batchesCost + deliveryCost;
        }
        public double GetDeliveryTime()
        {
            return _distance / _transport.Speed;
        }
    }
}
