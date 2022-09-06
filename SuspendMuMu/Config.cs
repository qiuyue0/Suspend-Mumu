using System;
using System.IO;
using System.Xml;

namespace SuspendMuMu
{
    internal class Config
    {
        // %AppData%/SuspendMuMu
        private static readonly string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SuspendMuMu");
        private static readonly string configPath = Path.Combine(directory, "config.xml");
        private static readonly XmlReaderSettings readerSettings = new XmlReaderSettings() { Async = false };
        private static readonly XmlWriterSettings writerSettings = new XmlWriterSettings() { Async = true };
        private static bool flag = false;
        static Config()
        {
            Directory.CreateDirectory(directory);
        }

        // load config
        public static void Load()
        {
            if (CheckExistence(configPath))
            {
                using (Stream stream = File.OpenRead(configPath))
                {

                    using (XmlReader reader = XmlReader.Create(stream, readerSettings))
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element && reader.Name == "VkCode")
                                {
                                    Common.VkCode = int.Parse(reader.ReadElementContentAsString());
                                    Common.KeyName = reader.ReadElementContentAsString();
                                }

                            }
                        }
                        catch (InvalidOperationException ex)
                        {

                        }
                        catch
                        {
                            flag = true;
                        }
                    }
                }
            }
            else
            {
                flag = true;
            }

            if (flag)
            {
                flag = false;
                Common.VkCode = 113;
                Common.KeyName = "F2";
                Save();
            }
        }
        // save config
        public static async void Save()
        {
            using (Stream stream = File.OpenWrite(configPath))
            {
                using (XmlWriter writer = XmlWriter.Create(stream, writerSettings))
                {
                    await writer.WriteStartDocumentAsync();
                    await writer.WriteStartElementAsync(null, "configuation", null);

                    // 写入节点VkCode
                    await writer.WriteStartElementAsync(null, "VkCode", null);
                    await writer.WriteStringAsync(Common.VkCode.ToString());
                    await writer.WriteEndElementAsync();

                    // 写入节点KeyName
                    await writer.WriteStartElementAsync(null, "KeyName", null);
                    await writer.WriteStringAsync(Common.KeyName);
                    await writer.WriteEndElementAsync();

                    // 结束xml
                    await writer.WriteEndElementAsync();
                    await writer.FlushAsync();
                }
            }


        }
        // check config exsitence
        private static bool CheckExistence(string path)
        {
            return File.Exists(path);

        }

        private class JsonData
        {
            public Int32 VkCode { get; set; }
            public string KeyName { get; set; }
        }
    }


}
