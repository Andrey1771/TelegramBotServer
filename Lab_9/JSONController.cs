using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab_9
{
    static class JSONController<T> where T : new()
    {
        public static void Serialize(T data, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
        }

        public static T Deserialize(string path)
        {
            if(File.Exists(path))
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));

            return new T();
        }
    }
}
