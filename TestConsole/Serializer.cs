//using System.IO;
//using System.Text;

//namespace TestConsole
//{
//    /// <summary>
//    /// Static class for serializing objects to JSON or XML strings and deserializing Json or XML to objects of the specified type. 
//    /// </summary>
//    public static class Serializer
//    {
//        /// <summary>
//        /// Deserializes a JSON string to the type of object specified.
//        /// </summary>
//        /// <typeparam name="T">The type of object to deserialize the string to.</typeparam>
//        /// <param name="json">The string to deserialize.</param>
//        /// <returns>An instance of the object deserialized from the string.</returns>
//        public static T DeserializeJson<T>(string json)
//        {
//            if (string.IsNullOrEmpty(json))
//            {
//                return default(T);
//            }

//            var jsonSerializer = new DataContractJsonSerializer(typeof(T));
//            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
//            {
//                return (T)jsonSerializer.ReadObject(memoryStream);
//            }
//        }

//        /// <summary>
//        /// Serializes an object to JSON.
//        /// </summary>
//        /// <typeparam name="T">The type of the object to serialize.</typeparam>
//        /// <param name="obj">The object to serialize.</param>
//        /// <returns>The serialized object in JSON format.</returns>
//        public static string SerializeToJson<T>(T obj)
//        {
//            if (obj == null)
//            {
//                return null;
//            }

//            var jsonSerializer = new DataContractJsonSerializer(obj.GetType());
//            using (var memoryStream = new MemoryStream())
//            {
//                jsonSerializer.WriteObject(memoryStream, obj);
//                return Encoding.UTF8.GetString(memoryStream.ToArray());
//            }
//        }
//    }
//}
