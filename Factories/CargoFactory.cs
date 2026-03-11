using LABOOP4.Entities;
using System.Collections.Generic;
namespace LABOOP4.Factories
{
    internal class CargoProvider
    {
        Dictionary<string, ICargoFactory> _catalog;

        public CargoProvider(Dictionary<string, ICargoFactory> catalog)
        {
            _catalog = catalog;
        }
        public Cargo GetCargo(string name)
        {
            if (!_catalog.ContainsKey(name))
            {
                throw new ArgumentException("No Such Cargo!");
            }

            return _catalog[name].CreateCargo();
        }
    }

    internal interface ICargoFactory
    {
        public Cargo CreateCargo();
    }

    internal class ElectronicFactory : ICargoFactory
    {
        public Cargo CreateCargo()
        {
            return new Cargo("Electronic", 1.5, 50);
        }
    }
    internal class ClothFactory : ICargoFactory
    {
        public Cargo CreateCargo()
        {
            return new Cargo("Cloth", 0.8, 20);
        }
    }
    internal class EquipmentFactory : ICargoFactory
    {
        public Cargo CreateCargo()
        {
            return new Cargo("Equipment", 120, 15);
        }
    }
    internal class ProductFactory : ICargoFactory
    {
        public Cargo CreateCargo()
        {
            return new Cargo("Product", 10, 100);
        }

    }

}
