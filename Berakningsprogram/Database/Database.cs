using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.ExceptionServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gym_Booking_Manager
{
    internal class Database
    {
        public List<Equipment> equipmentObjects { get; set; } = new List<Equipment>();
        public List<Space> spaceObjects { get; set; } = new List<Space>();
        public List<Trainer> trainerObjects { get; set; } = new List<Trainer>();
        public List<Activity> activities { get; set; } = new List<Activity>();
        public List<ReservingEntity> userObjects { get; set; } = new List<ReservingEntity>();
        public List<RestrictedObjects> restrictedObjects { get; set; } = new List<RestrictedObjects>();
        public List<RestrictedObjects> restrictedList { get; set; } = new List<RestrictedObjects>();

        public GroupSchedule schedule { get; set; } = new GroupSchedule();
        public RestrictedObjects restricted { get; set; } = new RestrictedObjects();
        public ReservingEntity user { get; set; } = new ReservingEntity();
        public List<Activity> templateActivityObjects { get; set; } = new List<Activity>();

        public void LoadDataBase()
        {
            equipmentObjects = LoadViaDataContractSerialization<List<Equipment>>("equipmentObjects.xml");
            spaceObjects = LoadViaDataContractSerialization<List<Space>>("spaceObjects.xml");
            trainerObjects = LoadViaDataContractSerialization<List<Trainer>>("trainerObjects.xml");
            userObjects = LoadViaDataContractSerialization<List<ReservingEntity>>("user.xml");
            activities = LoadViaDataContractSerialization<List<Activity>>("activity.xml");
            restrictedList = LoadViaDataContractSerialization<List<RestrictedObjects>>("restrictedList.xml");
            templateActivityObjects = LoadViaDataContractSerialization<List<Activity>>("templateActivities.xml");
            if (activities != null)
            {
                foreach (Activity activity in activities)
                {
                    schedule.activities.Add(activity);
                }
            }
            else
                Console.WriteLine("No activities were loaded!");
        }

        public void SaveToDataBase()
        {
            if (activities == null)
            {
                Console.WriteLine("No activities were saved to Database");
            }
            else
            {
                activities.Clear();
                foreach (Activity activity in schedule.activities)
                {
                    activities.Add(activity);
                }
                SaveViaDataContractSerialization(activities, "activity.xml");
            }
            SaveViaDataContractSerialization(userObjects, "user.xml");
            SaveViaDataContractSerialization(restrictedList, "restrictedList.xml");
            SaveViaDataContractSerialization(spaceObjects, "spaceObjects.xml");
            SaveViaDataContractSerialization(trainerObjects, "trainerObjects.xml");
            SaveViaDataContractSerialization(equipmentObjects, "equipmentObjects.xml");
            SaveViaDataContractSerialization(templateActivityObjects, "templateActivities.xml");
        }

        public static string FilePath(string fileName)
        {
            char sep = Path.DirectorySeparatorChar;
            string storage = $"GymDB{sep}storage";
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
