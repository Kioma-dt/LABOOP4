using System.Text.Json;
using System.Text.Json.Serialization;

namespace LABOOP4.Entities
{
    public enum TransportType { Air, Land, Water};

    public static class TransportTypeConverter
    {
        public static TransportType ToTransportType(this string value)
        {
            return value.ToLower() switch
            {
                "земля"=> TransportType.Land,
                "land" => TransportType.Land,
                "вода" => TransportType.Water,
                "water" => TransportType.Water,
                "air" => TransportType.Air,
                "воздух" => TransportType.Air,
                _ => throw new ArgumentException($"Unknown transport type: {value}")
            };
        }
    };

    public abstract class Transport
    {
        double _costPerKm;
        double _speed;
        public string Name { get; set; }
        public TransportType Type { get; set; }
        public double CostPerKm 
        { 
            get => _costPerKm;
            set
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
            set 
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
        public Transport(string name, TransportInfo info)
            : this(name, info.Type.ToTransportType(), info.CostPerKm, info.Speed)
        {

        }
    }

    public class AirTransport : Transport
    {
        public AirTransport(string name, double costPerKm, double speed) 
            :base(name, TransportType.Air, costPerKm, speed)
        { }
        public AirTransport(string name, TransportInfo info)
            :base(name, info)
        {
            if (info.Type.ToTransportType() != TransportType.Air)
            {
                throw new ArgumentException("Types Don't Match");
            }
        }
    }

    public class LandTransport : Transport
    {
        public LandTransport(string name, double costPerKm, double speed)
            : base(name, TransportType.Land, costPerKm, speed)
        { }
        public LandTransport(string name, TransportInfo info)
            : base(name, info)
        {
            if (info.Type.ToTransportType() != TransportType.Land)
            {
                throw new ArgumentException("Types Don't Match");
            }
        }
    }
    public class WaterTransport : Transport
    {
        public WaterTransport(string name, double costPerKm, double speed)
            : base(name, TransportType.Water, costPerKm, speed)
        { }
        public WaterTransport(string name, TransportInfo info)
            : base(name, info)
        {
            if (info.Type.ToTransportType() != TransportType.Water)
            {
                throw new ArgumentException("Types Don't Match");
            }
        }
    }


    public struct TransportInfo
    {
        double _costPerKm;
        double _speed;

        public string Type { get; set; }
        public double CostPerKm
        {
            get => _costPerKm;
            set
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
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Speed Should Be Positive!");
                }
                _speed = value;
            }
        }

        public TransportInfo() { }

        public TransportInfo(string type, double costPerKm, double speed)
        { 
            Type = type;
            CostPerKm = costPerKm;
            Speed = speed;
        }
    }
}
