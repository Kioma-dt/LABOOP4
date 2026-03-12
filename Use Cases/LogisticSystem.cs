using LABOOP4.Factories;
using LABOOP4.Entities;

namespace LABOOP4.Use_Cases
{

    internal class LogisticSystem
    {
        Dictionary<string, ITransportFactory> _transportCatalog;
        Dictionary<string, ICargoFactory> _cargoCatalog;

        public LogisticSystem(Dictionary<string, ITransportFactory> transportCatalog, 
            Dictionary<string, ICargoFactory> cargoCatalog) 
        {
            _transportCatalog = transportCatalog;
            _cargoCatalog = cargoCatalog;
        }

        public Order RegisterOrder(IEnumerable<(string cargoName, int amount)> cargoBatches, 
            string transportName, int distance)
        {
            var batches = new List<(Cargo, int)>();
            foreach (var (cargoName, amount) in cargoBatches)
            {
                if (!_cargoCatalog.ContainsKey(cargoName))
                {
                    throw new Exception($"No Such Cargo: {cargoName} in Catalog!");
                }

                batches.Add((_cargoCatalog[cargoName].CreateCargo(), amount));
            }

            if (!_transportCatalog.ContainsKey(transportName))
            {
                throw new Exception($"No Such Cargo: {transportName} in Catalog!");
            }

            var transport = _transportCatalog[transportName].CreateTransport();

            return new Order(batches, transport, distance);
        }
    }
}
