namespace LABOOP4.Entities
{
    enum TransportType { Air, Land, Water};
    internal abstract class Transport
    {
        double _costPerKm;
        double _speed;
        public string Name { get; }
        public TransportType Type { get; }
        public double CostPerKm 
        { 
            get => _costPerKm;
            private set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Cost Should Be Positive!");
                }
                _costPerKm = value;
            }
        } 
        public double Speed 
        { 
            get => _speed;
            private set 
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Speed Should Be Positive!");
                }
                _speed = value;
            }
        }

        public Transport(string name, TransportType type, double costPerKm, double speed)
        {
            Name = name;
            Type = type;
            CostPerKm = costPerKm;
            Speed = speed;
        }
    }
}
