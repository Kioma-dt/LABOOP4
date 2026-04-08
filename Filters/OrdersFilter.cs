using LABOOP4.Entities;

namespace LABOOP4.Filters
{
    public interface IFilter
    {
        public bool Apply(Order order);
    }
    public class OrdersFilter
    {
        readonly List<IFilter> _filters = new();

        public OrdersFilter()
        {

        }
        
        public void AddFilter(IFilter filter) 
        {
            _filters.Add(filter);
        }

        public List<Order> ApplyFilters(List<Order> orders)
        {
            var tempOrders = new List<Order>(orders);

            foreach (var filter in _filters) 
            {
                tempOrders = tempOrders.Where(o => filter.Apply(o)).ToList();
            }

            return tempOrders;
        }
    }

    public enum ComparisonType { Equal, GreaterOrEqual, LessOrEqual, Greater, Less}
    public class DeliveryCostFilter : IFilter
    {
        readonly ComparisonType _type;
        readonly double _cost;

        public DeliveryCostFilter(double cost, ComparisonType type) 
        {
            _type = type;
            _cost = cost;
        }

        public bool Apply(Order order)
        {
            return _type switch
            {
                ComparisonType.Equal => order.Cost == _cost,
                ComparisonType.GreaterOrEqual => order.Cost >= _cost,
                ComparisonType.LessOrEqual => order.Cost <= _cost,
                ComparisonType.Greater => order.Cost > _cost,
                ComparisonType.Less => order.Cost < _cost,
                _ => order.Cost == _cost
            };
        }
    }

    public class DeliveryTimeFilter : IFilter
    {
        readonly ComparisonType _type;
        readonly double _time;

        public DeliveryTimeFilter(double time, ComparisonType type)
        {
            _time = time;
            _type = type;
        }

        public bool Apply(Order order)
        {
            return _type switch
            {
                ComparisonType.Equal => order.DeliveryTime == _time,
                ComparisonType.GreaterOrEqual => order.DeliveryTime >= _time,
                ComparisonType.LessOrEqual => order.DeliveryTime <= _time,
                ComparisonType.Greater => order.DeliveryTime > _time,
                ComparisonType.Less => order.DeliveryTime < _time,
                _ => order.DeliveryTime == _time
            };
        }
    }

    public class TransportNameFilter : IFilter 
    {
        readonly string _name;

        public TransportNameFilter(string name) 
        {
            _name = name;
        }

        public bool Apply(Order order)
        {
            return order.TrasnportName == _name;   
        }
    }
    public class TransportTypeFilter : IFilter
    {
        readonly TransportType _type;

        public TransportTypeFilter(TransportType type)
        {
            _type = type;
        }

        public bool Apply(Order order)
        {
            return order.TrasnportType == _type;
        }
    }
}
