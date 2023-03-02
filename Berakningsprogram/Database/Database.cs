using System.Runtime.Serialization;
using System.Xml;

namespace purchase_list_group2
{
    internal class Database
    {
        public List<Store> storeObjects { get; set; } = new List<Store>();
        public List<User> userObjects { get; set; } = new List<User>();

        public void LoadDataBase()
        {
            storeObjects = LoadViaDataContractSerialization<List<Store>>("storeObjects.xml");
            userObjects = LoadViaDataContractSerialization<List<User>>("userObjects.xml");
            //if (activities != null)
            //{
            //    foreach (Activity activity in activities)
            //    {
            //        schedule.activities.Add(activity);
            //    }
            //}
            //else
            //    Console.WriteLine("No activities were loaded!");
        }

        public void SaveToDataBase()
        {
            //if (activities == null)
            //{
            //    Console.WriteLine("No activities were saved to Database");
            //}
            //else
            //{
            //    activities.Clear();
            //    foreach (Activity activity in schedule.activities)
            //    {
            //        activities.Add(activity);
            //    }
            //    SaveViaDataContractSerialization(activities, "activity.xml");
            //}
            SaveViaDataContractSerialization(userObjects, "user.xml");
            SaveViaDataContractSerialization(storeObjects, "store.xml");
        }

        public static string FilePath(string fileName)
        {
            char sep = Path.DirectorySeparatorChar;
            string storage = $"PurchaseList{sep}storage";
            Directory.CreateDirectory(storage);
            string fpathUser = $"{storage}{sep}{fileName}";
            return fpathUser;
        }

        public static void SaveViaDataContractSerialization<T>(T serializableObject, string fileName)
        {
            string fpathUser = FilePath(fileName);
            try
            {
                var serializer = new DataContractSerializer(typeof(T));
                var settings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t"
                };
                var writer = XmlWriter.Create(fpathUser, settings);
                serializer.WriteObject(writer, serializableObject);
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No data was saved to {fileName} {ex.Message}");
            }
        }
        public static T LoadViaDataContractSerialization<T>(string fileName)
        {
            string fpathFile = FilePath(fileName);
            try
            {
                var fileStream = new FileStream(fpathFile, FileMode.Open);
                var reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
                var serializer = new DataContractSerializer(typeof(T));
                T serializableObject = (T)serializer.ReadObject(reader, true);
                reader.Close();
                fileStream.Close();
                return serializableObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{fpathFile} {ex.Message}");
                return default;
            }
        }
    }
}
