using LABOOP4.Entities;
using System.Collections.Generic;
namespace LABOOP4.Factories
{
    internal interface ICargoFactory
    {
        public Cargo CreateCargo(string name);
    }
    

    internal class CargoFactory : ICargoFactory
    {
        Dictionary<string, CargoInfo> _catalog;
        public CargoFactory(Dictionary<string, CargoInfo> catalog)
        { 
            _catalog = catalog;
        }

        public Cargo CreateCargo(string name)
        {
            if (!_catalog.ContainsKey(name))
            {
                throw new Exception($"No Such Cargo: {name} in Catalog!");
            }

            return new Cargo(name, _catalog[name]);
        }
    }

    //internal class ElectronicFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Electronic();
    //    }
    //}
    //internal class ClothFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Cloth();
    //    }
    //}
    //internal class EquipmentFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Equipment();
    //    }
    //}
    //internal class ProductFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Product();
    //    }

    //}

    //internal class Electronic : Cargo
    //{
    //    public Electronic()
    //        : base("Электроника", 1.5, 50)
    //    {
    //    }
    //}

    //internal class Cloth : Cargo
    //{
    //    public Cloth()
    //        : base("Одежда", 0.8, 20)
    //    {
    //    }
    //}

    //internal class Equipment : Cargo
    //{
    //    public Equipment()
    //        : base("Оборудование", 120, 15)
    //    {
    //    }
    //}
    //internal class Product : Cargo
    //{
    //    public Product()
    //        : base("Скоропортящиеся продукты", 10, 100)
    //    {
    //    }
    //}

}
