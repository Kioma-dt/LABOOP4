using LABOOP4.Entities;
using System.Collections.Generic;
namespace LABOOP4.Factories
{
    public interface ICargoFactory
    {
        public Cargo CreateCargo(string name);
    }
    

    public class CargoFactory : ICargoFactory
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

    //public class ElectronicFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Electronic();
    //    }
    //}
    //public class ClothFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Cloth();
    //    }
    //}
    //public class EquipmentFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Equipment();
    //    }
    //}
    //public class ProductFactory : ICargoFactory
    //{
    //    public Cargo CreateCargo()
    //    {
    //        return new Product();
    //    }

    //}

    //public class Electronic : Cargo
    //{
    //    public Electronic()
    //        : base("Электроника", 1.5, 50)
    //    {
    //    }
    //}

    //public class Cloth : Cargo
    //{
    //    public Cloth()
    //        : base("Одежда", 0.8, 20)
    //    {
    //    }
    //}

    //public class Equipment : Cargo
    //{
    //    public Equipment()
    //        : base("Оборудование", 120, 15)
    //    {
    //    }
    //}
    //public class Product : Cargo
    //{
    //    public Product()
    //        : base("Скоропортящиеся продукты", 10, 100)
    //    {
    //    }
    //}

}
