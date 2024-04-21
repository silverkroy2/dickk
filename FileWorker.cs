using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace dick
{
    public static class FileWorker
    {
        // Сериализация на Newtonsoft и запись в файл
        public static void Serialize<T>(T objects, string path)
        {
            try
            {
                var json = JsonConvert.SerializeObject(objects);
                File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        // извлечение из файла и Десериализация на Newtonsoft
        public static T Deserialize<T>(string path) where T : new()
        {
            try
            {
                var obj = File.ReadAllText(path);

                if (string.IsNullOrEmpty(obj) || obj.Length < 3)
                {
                    return new T();
                }

                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new T();
            }
        }




    }
}
