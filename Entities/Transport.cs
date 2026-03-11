namespace LABOOP4.Entities
{
    enum TransportType { Air, Land, Water};
    internal class Transport
    {
        public string Name { get; }
        public TransportType Type { get; }
        public double CostPerKm { get; } 
        public int Speed { get;}

        public Transport(string name, TransportType type, double costPerKm, int speed)
        {
            Name = name;
            Type = type;
            CostPerKm = costPerKm;
            Speed = speed;
        }
    }
}
