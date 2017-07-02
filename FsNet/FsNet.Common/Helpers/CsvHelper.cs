using System.Collections.Generic;
using ServiceStack.Text;

namespace FsNet.Common.Helpers
{
    public class CsvHelper
    {
        public static string SerializeToFile<T>(IEnumerable<T> data)
        {
            return CsvSerializer.SerializeToCsv(data);
        }

        public static string Serialize<T>(T data)
        {
            return CsvSerializer.SerializeToString(data);
        }

        public static T Deserialize<T>(string data)
        {
            return CsvSerializer.DeserializeFromString<T>(data);
        }

    }
}
