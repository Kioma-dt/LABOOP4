using LABOOP4.Entities;

namespace LABOOP4.Sorts
{
    public interface IOrderSort
    {
        List<Order> Sort(List<Order> ordres, bool descending =  false);
    }

    public class DeliveryCostSort : IOrderSort
    {
        public List<Order> Sort(List<Order> ordres, bool descending = false)
        {
            if (descending) {
                return ordres.Select(o => o)
                       .OrderByDescending(o => o.Cost)
                       .ToList();
            }
            else
            {
                return ordres.Select(o => o)
                       .OrderBy(o => o.Cost)
                       .ToList();
            }
        }
    }

    public class DeliveryTimeSort : IOrderSort
    {
        public List<Order> Sort(List<Order> ordres, bool descending = false)
        {
            if (descending)
            {
                return ordres.Select(o => o)
                       .OrderByDescending(o => o.DeliveryTime)
                       .ToList();
            }
            else
            {
                return ordres.Select(o => o)
                       .OrderBy(o => o.DeliveryTime)
                       .ToList();
            }
        }
    }

    public class TransportNameSort : IOrderSort
    {
        public List<Order> Sort(List<Order> ordres, bool descending = false)
        {
            if (descending)
            {
                return ordres.Select(o => o)
                       .OrderByDescending(o => o.TrasnportName)
                       .ToList();
            }
            else
            {
                return ordres.Select(o => o)
                       .OrderBy(o => o.TrasnportName)
                       .ToList();
            }
        }
    }
    public class TransportTypeSort : IOrderSort
    {
        public List<Order> Sort(List<Order> ordres, bool descending = false)
        {
            if (descending)
            {
                return ordres.Select(o => o)
                       .OrderByDescending(o => o.TrasnportType)
                       .ToList();
            }
            else
            {
                return ordres.Select(o => o)
                       .OrderBy(o => o.TrasnportType)
                       .ToList();
            }
        }
    }
}
