using System.Text.Json;
using System.Text.Json.Serialization;

namespace LABOOP4.Entities
{
    public enum TransportType { Air, Land, Water};

    public static class TransportTypeConverter
    {
        public static TransportType ParseTransportType(string value)
        {
            return value.ToLower() switch
            {
                "земля" => TransportType.Land,
                "вода" => TransportType.Water,
                "воздух" => TransportType.Air,
                _ => throw new ArgumentException($"Unknown transport type: {value}")
            };
        }
    };

    public class JsonTransportTypeConverter : JsonConverter<TransportType>
    {
        public override TransportType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            return value.ToLower() switch
            {
                "земля" => TransportType.Land,
                "вода" => TransportType.Water,
                "воздух" => TransportType.Air,
                _ => throw new JsonException($"Unknown type: {value}")
            };
        }

        public override void Write(Utf8JsonWriter writer, TransportType value, JsonSerializerOptions options)
        {
            var str = value switch
            {
                TransportType.Land => "земля",
                TransportType.Water => "вода",
                TransportType.Air => "воздух",
                _ => throw new JsonException()
            };

            writer.WriteStringValue(str);
        }
    }

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
            : this(name, info.Type, info.CostPerKm, info.Speed)
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
            if (info.Type != TransportType.Air)
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
            if (info.Type != TransportType.Land)
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
            if (info.Type != TransportType.Water)
            {
                throw new ArgumentException("Types Don't Match");
            }
        }
    }


    public struct TransportInfo
    {
        double _costPerKm;
        double _speed;

        [JsonConverter(typeof(JsonTransportTypeConverter))]
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

        public TransportInfo() { }

        public TransportInfo(TransportType type, double costPerKm, double speed)
        { 
            Type = type;
            CostPerKm = costPerKm;
            Speed = speed;
        }
    }
}
