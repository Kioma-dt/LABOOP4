using System.Text.Json.Serialization;

namespace LABOOP4.Entities
{
    public class Cargo
    {
        string _name;
        double _mass;
        double _costPerKg;
        public string Name 
        { 
            get => _name;
            set => _name = value;
        }
        public double Mass {
            get => _mass;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Mass Should Be Positive!");
                }
                _mass = value;
            }
        }
        public double CostPerKg 
        {
            get => _costPerKg;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Cost Should Be Positive!");
                }

                _costPerKg = value;
            }
        }
        public Cargo(string name,  double mass, double costPerKg)
        {
            Name = name;
            Mass = mass;
            CostPerKg = costPerKg;
        }
        public Cargo(string name, CargoInfo info)
            :this(name, info.Mass, info.CostPerKg)
        {

        }

        public double GetCostOfOne()
        {
            return CostPerKg * Mass;
        }
    }

    public struct CargoInfo
    {
        double _mass;
        double _costPerKg;
        public double Mass
        {
            get => _mass;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Mass Should Be Positive!");
                }
                _mass = value;
            }
        }
        public double CostPerKg
        {
            get => _costPerKg;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Cost Should Be Positive!");
                }

                _costPerKg = value;
            }
        }

        public CargoInfo() { }

        public CargoInfo(double mass, int costPerKg)
        {
            Mass = mass;
            CostPerKg = costPerKg;
        }
    }

    public struct CargoBatch
    {
        int _amount;
        public Cargo Cargo { get;}
        public int Amount 
        { 
            get => _amount;
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("Amount Should Be Positive!");
                }
                _amount = value;
            }
        }

        public CargoBatch(Cargo cargo, int amount) 
        {
            Cargo = cargo;
            Amount = amount;
        }

        public double GetCost()
        {
            return Cargo.GetCostOfOne() * Amount;
        }

    }
}
