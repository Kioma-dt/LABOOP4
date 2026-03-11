namespace LABOOP4.Entities
{
    internal class Cargo
    {
        public string Name { get;}
        public double Mass { get;}
        public int CostPerKg { get; }

        public Cargo(string name, double mass, int costPerKg)
        {
            Name = name;
            Mass = mass;
            CostPerKg = costPerKg;
        }

        public double GetCostOfOne()
        {
            return CostPerKg * Mass;
        }
    }

    internal struct CargoBatch
    {
        public Cargo Cargo { get;}
        public int Amount { get;}

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
