using System.Collections.Generic;

namespace LABOOP4.Entities
{
    internal class Order
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

        public double GetCost()
        {
            double batchesCost = _cargoBatches.Sum(x => x.GetCost());
            double deliveryCost = _transport.CostPerKm * _distance;

            return batchesCost + deliveryCost;
        }
        public int GetDeliveryTime()
        {
            return _distance / _transport.Speed;
        }
    }
}
