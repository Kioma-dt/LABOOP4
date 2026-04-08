using LABOOP4.Entities;
using System.Globalization;
using System.Text.Json;

namespace LABOOP4.FileService.Save
{
    public abstract class SaveService
    {
        public abstract void Save(IEnumerable<SaveDTO> orders, string fileName);
    }

    public class SaveDTO
    {
        public SaveDTO() { }
        public SaveDTO(Order order)
        {
            DeliveryCost = order.Cost.ToString(CultureInfo.InvariantCulture);
            DeliveryTime = order.DeliveryTime.ToString(CultureInfo.InvariantCulture);
        }
        public string DeliveryCost {  get; set; }
        public string DeliveryTime { get; set; }
    }
    public class JsonSaveService : SaveService
    {
        public override void Save(IEnumerable<SaveDTO> orders, string fileName)
        {
            try
            {

                var options = new JsonSerializerOptions() { WriteIndented = true };
                var json = JsonSerializer.Serialize(orders, options);

                using (var writer = new StreamWriter(fileName))
                {
                    writer.Write(json);
                    writer.Flush();
                }
            }
            catch (IOException ex) 
            {
                throw new Exception($"Can't write to File {fileName}: {ex.Message}");
            }
        }
    }

    public class CsvSaveService : SaveService
    {
        public override void Save(IEnumerable<SaveDTO> orders, string fileName)
        {
            try
            {

                using (var writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("DeliveryCost,DeliveryTime");

                    foreach (var order in orders)
                    {
                        writer.WriteLine($"{order.DeliveryCost},{order.DeliveryTime}");
                    }
                }
            }
            catch (IOException ex)
            {
                throw new Exception($"Can't write to File {fileName}: {ex.Message}");
            }
        }
    }
}
