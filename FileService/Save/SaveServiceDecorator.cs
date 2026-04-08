using System.IO.Compression;
using System.Security.Cryptography;
namespace LABOOP4.FileService.Save
{
    public abstract class SaveServiceDecorator : SaveService
    {
        protected SaveService saveService;

        public SaveServiceDecorator(SaveService saveService)
            : base()
        {
            this.saveService = saveService;
        }
    }

    public class EncryptionDecorator : SaveServiceDecorator
    {
        byte[] _key;
        byte[] _iv;
        public EncryptionDecorator(SaveService saveService, byte[] key, byte[] iv) : base(saveService)
        {
            _key = key;
            _iv = iv;
        }

        public override void Save(IEnumerable<SaveDTO> orders, string fileName)
        {
            var encrypted_orders = new List<SaveDTO>();

            foreach (var order in orders) 
            {
                encrypted_orders.Add(new SaveDTO { DeliveryCost = Encrypt(order.DeliveryCost), DeliveryTime = Encrypt(order.DeliveryTime) });
            }

            saveService.Save(encrypted_orders, fileName);
        }

        string Encrypt(string plainText)
        {
            using var aes = Aes.Create();

            aes.Key = _key;
            aes.IV = _iv;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);

            sw.Write(plainText);
            sw.Close();

            return Convert.ToBase64String(ms.ToArray());
        }
    }

    public class ZipDecorator : SaveServiceDecorator
    {
        string _zipFileName = "zip";
        public ZipDecorator(SaveService saveService, string zipFileName) : base(saveService)
        {
            _zipFileName = zipFileName;
        }

        public override void Save(IEnumerable<SaveDTO> orders, string fileName)
        {
            var tempFile = Path.GetTempFileName();

            saveService.Save(orders, tempFile);

            if (File.Exists(_zipFileName))
            {
                File.Delete(_zipFileName);
            }

            using (var archive = ZipFile.Open(_zipFileName, ZipArchiveMode.Create)){
                archive.CreateEntryFromFile(tempFile, fileName);
            }

            File.Delete(tempFile);
        }
    }
}
