using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RentC.WebUI.Helpers
{
    public static class Json
    {
        public static void Serialize<T>(T value, string filePath) 
            where T: class
        {
            var serializer = new JsonSerializer();

            using var sw = new StreamWriter(filePath);
            using JsonWriter writer = new JsonTextWriter(sw);

            serializer.Serialize(writer, value);
        }

        public static T Deserialize<T>(string filePath)
            where T: class
        {
            var serializer = new JsonSerializer();

            using var sr = new StreamReader(filePath);
            using var reader = new JsonTextReader(sr);

            return (T) serializer.Deserialize(reader);
        }
    }
}