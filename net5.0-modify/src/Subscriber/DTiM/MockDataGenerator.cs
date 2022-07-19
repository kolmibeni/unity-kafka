using ChoETL;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DTiM
{
    /// <summary>
    ///     Produce mock data for testing.
    /// </summary>
    public class MockDataGenerator
    {
        private DtimAppAPI DtimApi { get; set; }

        /// <summary>
        ///     The CSV file you want to use to produce messages.
        /// </summary>
        public string CsvFilePath { get; set; }

        /// <summary>
        ///     Initialize MockDataGenerator with Specify a CSV file.
        /// </summary>
        public MockDataGenerator(DtimAppAPI dtimApi, string filePath)
        {
            DtimApi = dtimApi;
            CsvFilePath = filePath;
        }

        /// <summary>
        ///     Get all the content of the CSV File.
        /// </summary>
        public string GetAllCSVText()
        {
            return File.ReadAllText(CsvFilePath);
        }

        /// <summary>
        ///     Produce mock data from a string variable.
        /// </summary>
        public string GetMockData()
        {
            string csv = @"Id, Name, City
            1, Tom, NY
            2, Mark, NJ
            3, Lou, FL
            4, Smith, PA
            5, Raj, DC
            ";

            return csv;
        }

        /// <summary>
        ///     Produce OP2 format data.
        /// </summary>    
        public async Task ProduceWHL55_SensorData()
        {
            string topic = "WHL55_SensorData";
            var sensorData = new
            {
                Cur_M = 1.2,
                Cur_X = 1.3,
                Cur_Z = 1.4,
                Vib_M = 3.4,
                Vib_ = 5.6
            };

            await DtimApi.SendMessage(sensorData, topic);
        }

        /// <summary>
        ///     Specific a producer to produce mock data by a period.
        /// </summary>
        /// <param name="producer">
        ///     A producer you specify.
        /// </param>
        /// <param name="interval">
        ///     the interval expressed in milliseconds, at which to produce the message.
        /// </param>        
        public async Task ProduceDataFromCSV(int interval)
        {
            string logFilePath = $"./assets/logs/{DateTime.Today:yyyy-MM-dd}-data.log";

            using (StreamReader reader = new StreamReader(CsvFilePath))
            {
                string header = reader.ReadLine();
                string record = string.Empty;
                int i = 0;
                while ((record = reader.ReadLine()) != null)
                {
                    string csv = $"{header}{Environment.NewLine}{record}";
                    //Console.WriteLine("record:"+csv);

                    StringBuilder sb = new StringBuilder();
                    using (var p = ChoCSVReader.LoadText(csv).WithFirstLineHeader())
                    {
                        using (var w = new ChoJSONWriter(sb))
                        {
                            w.Write(p);
                        }
                    }
                    //string rawData = sb.ToString();
                    //var rawData = new
                    //{
                    //    Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff"),
                    //    Msg = record
                    //};
                    //await DtimApi.SendMessage(rawData, "vibration");
                    await DtimApi.SendMessage(record, "vibration");
                    await UpdateLogFileAsync(logFilePath, $"{i}:{sb}");
                    i++;

                    Thread.Sleep(interval);
                }
            }
        }

        public static async Task UpdateLogFileAsync(string filePath, string msg)
        {
            await WriteFileAsync(filePath, msg);
        }

        private static void WriteFile(string filePath, string content)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                sw.WriteLine(content);
            }
        }

        private static async Task WriteFileAsync(string filePath, string content)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.Default))
            {
                await sw.WriteLineAsync(content);
            }
        }
    }
}