using LABOOP4.Entities;
namespace LABOOP4.Factories
{

    internal interface ITransportFactory
    {
        public Transport CreateTransport();
    }

    internal class TruckFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Truck();
        }
    }
    internal class TrainFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Train();
        }
    }
    internal class TankerFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Tanker();
        }
    }
    internal class AirplaneFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Airplane();
        }

    }

    internal class HelicopterFactory : ITransportFactory
    {
        public Transport CreateTransport()
        {
            return new Helicopter();
        }

    }



    internal class Truck : Transport
    {
        public Truck()
            : base("Грузовик", TransportType.Land, 15.0, 80)
        {
        }
    }
    internal class Train : Transport
    {
        public Train()
            : base("Поезд", TransportType.Land, 5.0, 60)
        {
        }
    }
    internal class Tanker : Transport
    {
        public Tanker()
            : base("Танкер", TransportType.Water, 2.0, 35)
        {
        }
    }
    internal class Airplane : Transport
    {
        public Airplane()
            : base("Самолет", TransportType.Air, 150.0, 850)
        {
        }
    }
    internal class Helicopter : Transport
    {
        public Helicopter()
            : base("Вертолет", TransportType.Air, 200.0, 250)
        {
        }
    }

}
