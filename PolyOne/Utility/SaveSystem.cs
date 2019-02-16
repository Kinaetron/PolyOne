using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace PolyOne.Utility
{
    public class SaveSystem<T>
    {
        public void Save(string filename, T save)
        {
            MemoryStream stream = new MemoryStream();

            using (BsonWriter writer = new BsonWriter(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, save);
            }

            string data = Convert.ToBase64String(stream.ToArray());
            File.WriteAllText(filename, data);
        }

        public T Load(string filename)
        {
            byte[] data = Convert.FromBase64String(File.ReadAllText(filename));

            MemoryStream stream = new MemoryStream(data);
            using (BsonReader reader = new BsonReader(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                T file = serializer.Deserialize<T>(reader);
                return file;
            }
        }

        public bool Exists(string filename)
        {
            return File.Exists(filename);
        }

        public void Delete(string filename)
        {
            if (Exists(filename) == true) {
                File.Delete(filename);
            }
        }
    }
}
