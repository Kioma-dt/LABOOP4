namespace LABOOP4.Entities
{
    internal class Order
    {
        List<CargoBatch> _cargoBatches;
        public Transport _transport;
        public int _distance;

        public Order(List<CargoBatch> cargoBatches, Transport transport, int distance)
        {
            _cargoBatches = cargoBatches;
            _transport = transport;
            _distance = distance;
        }

        public double GetCost()
        {
            double batchesCost = _cargoBatches.Sum(x => x.GetCost());
            double deliveryCost = _transport.CostPerKm * _distance;

            return batchesCost + deliveryCost;
        }
        public int GetDekiveryTime()
        {
            return _transport.Speed * _distance;
        }
    }


}
