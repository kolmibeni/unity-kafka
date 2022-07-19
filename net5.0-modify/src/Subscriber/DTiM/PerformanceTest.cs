using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DTiM
{
    /// <summary>
    ///     The message format for performance test.
    /// </summary>
    public class TestMessage
    {
        public string Timestamp { get; set; }
        public WHL55 Msg { get; set; }
    }

    public class WHL55
    {
        public string Cur_M_250T { get; set; }
        public string Cur_M_50T { get; set; }
        public string Cur_X { get; set; }
        public string Cur_Y { get; set; }
        public string Cur_Z { get; set; }
        public string Cur_B { get; set; }
        public string Vib_M { get; set; }
        public string Vib_Z { get; set; }
    }

    public class TestMessageRobot
    {
        public string Timestamp { get; set; }
        public string Msg { get; set; }
    }

    public class Robot_data
    {
        public string Timetag_UTC { get; set; }
        public string Timetag { get; set; }
        public string LastUpdateTime { get; set; }
        public string J1 { get; set; }
        public string J2 { get; set; }
        public string J3 { get; set; }
        public string J4 { get; set; }
        public string J5 { get; set; }
        public string J6 { get; set; }
        public string J7 { get; set; }
        public string J8 { get; set; }
        public string J9 { get; set; }
        public string UT { get; set; }
        public string ValidJ { get; set; }
        public string Robot1 { get; set; }
        public string Robot2 { get; set; }
        public string PositionType { get; set; }
        public string WheelIdOfOP1 { get; set; }
        public string WheelIdOfOP2 { get; set; }
        public string WheelIdOfOP3 { get; set; }
        public string WheelIdOfOP4 { get; set; }
        public string WheelTypeOfOP1 { get; set; }
        public string WheelTypeOfOP2 { get; set; }
        public string WheelTypeOfOP3 { get; set; }
        public string WheelTypeOfOP4 { get; set; }
    }

    public class OP_machine_data
    {
        public string Timetag_UTC { get; set; }
        public string Timetag { get; set; }
        public string LastUpdateTime_LOCAL { get; set; }
        public string ToolCode { get; set; }
        public string ProgramNum { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string LeftDoor { get; set; }
        public string RightDoor { get; set; }
        public string ChangingTool { get; set; }
        public string SpindleSpinLight { get; set; }
        public string SpindleAngle { get; set; }
        public string CutterRradiusOfWear { get; set; }
        public string CutterRradiusOfGeometry { get; set; }
        public string ToolLengthOfWear { get; set; }
        public string ToolLengthOfGeometry { get; set; }
        public string SpindleSpeed { get; set; }
    }


    /// <summary>
    ///     The csv record format for performance test.
    /// </summary>
    public class LogRecord
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string TimeSpan { get; set; }
        public string Interval { get; set; }
        public string Size { get; set; }

        public LogRecord(string start, string end, string timeSpan, string interval, string size)
        {
            Start = start;
            End = end;
            TimeSpan = timeSpan;
            Interval = interval;
            Size = size;
        }
    }

    /// <summary>
    ///     A class for Performance tests. You can use this method after a consumer received the message, Ex:
    ///     ```
    ///         var msg = dtimAPI.getMessage();
    ///         string timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff");
    ///         await DTiM.PerformanceTest.calcPerformance(msg, timestamp, false);
    ///     ```
    ///     NOTICE: Make sure to add the keyword 'await' to let it can run asynchronously, the log file will be stored at ./assets/log/
    /// </summary>
    public class PerformanceTest
    {
        /// <summary>
        ///     Measure the speed of a message transmission
        /// </summary>
        /// <param name="msg">
        ///     A received message.
        /// </param>
        /// <param name="current">
        ///     A timestamp when the consumer receiving the message.
        /// </param>
        /// <param name="showlog">
        ///     Show the log on Console windle if this value is true.
        /// </param>
        public static void CalcPerformance(string msg, string current, bool showlog)
        {
            string datetimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff";
            LogRecord record;

            try
            {
                TestMessage testMsg = JsonConvert.DeserializeObject<TestMessage>(msg);

                var start = DateTime.ParseExact(testMsg.Timestamp, datetimeFormat, null);
                var end = DateTime.ParseExact(current, datetimeFormat, null);
                TimeSpan ts = end - start;
                int size = Encoding.Unicode.GetByteCount(testMsg.Msg.ToString());
                record = new LogRecord
                (
                    start.ToString(datetimeFormat),
                    end.ToString(datetimeFormat),
                    ts.ToString(),
                    ts.Milliseconds.ToString(),
                    size.ToString()
                );

                if (showlog)
                {
                    Console.WriteLine("start:" + record.Start);
                    Console.WriteLine("end:" + record.End);
                    Console.WriteLine("timespan:" + record.TimeSpan);
                    Console.WriteLine("Interval:" + record.Interval + " ms");
                    Console.WriteLine("Msg Size:" + record.Size + " bytes");
                }

                UpdateLogFIle(record);
            }
            catch (Exception)
            {
                Console.WriteLine($"the message can't be used by PerformanceTest. msg: {msg}");
            }
        }

        /// <summary>
        ///     Measure the speed of a message transmission
        /// </summary>
        /// <param name="msg">
        ///     A received message.
        /// </param>
        /// <param name="current">
        ///     A timestamp when the consumer receiving the message.
        /// </param>
        /// <param name="showlog">
        ///     Show the log on Console windle if this value is true.
        /// </param>
        public static async Task CalcPerformanceAsyc(string msg, string current, bool showlog)
        {
            string datetimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff";

            try
            {
                TestMessageRobot testMsg = JsonConvert.DeserializeObject<TestMessageRobot>(msg);

                var start = DateTime.ParseExact(testMsg.Timestamp, datetimeFormat, null);
                var end = DateTime.ParseExact(current, datetimeFormat, null);
                TimeSpan ts = end - start;
                int size = Encoding.Unicode.GetByteCount(testMsg.Msg.ToString());
                LogRecord record = new LogRecord
                (
                    start.ToString(datetimeFormat),
                    end.ToString(datetimeFormat),
                    ts.ToString(),
                    ts.Milliseconds.ToString(),
                    size.ToString()
                );

                if (showlog)
                {
                    Console.WriteLine("start:" + record.Start);
                    Console.WriteLine("end:" + record.End);
                    Console.WriteLine("timespan:" + record.TimeSpan);
                    Console.WriteLine("Interval:" + record.Interval + " ms");
                    Console.WriteLine("Msg Size:" + record.Size + " bytes");
                }

                await UpdateLogFIleAsync(record);
            }
            catch (Exception)
            {
                Console.WriteLine("the message can't be used by PerformanceTest. msg:" + msg);
            }
        }

        /// <summary>
        ///     Measure the speed of a message transmission from Consumer to Thingworx.
        /// </summary>
        /// <param name="msg">
        ///     A received message.
        /// </param>
        /// <param name="consumeTime">
        ///     A timestamp when the consumer receive the message.
        /// </param>
        /// <param name="thwxTime">
        ///     A timestamp when the Thingworx receive the message. To be used in conjunction with RestService.getPropertyLastUpdate()
        /// </param>
        /// <param name="showlog">
        ///     Show the log on Console window if this value is true.
        /// </param>
        public static async Task CalcPerformanceThingworx(string msg, string consumeTime, string thwxTime, Boolean showlog)
        {
            string datetimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff";

            try
            {
                var temp = thwxTime.Replace("\"", "");
                var start = DateTime.ParseExact(consumeTime, datetimeFormat, null);
                //Console.WriteLine("consumer: " + consumeTime);
                var end = DateTime.ParseExact(temp, datetimeFormat, null);
                //Console.WriteLine("thwx: " + temp);
                TimeSpan ts = end - start;
                int size = Encoding.Unicode.GetByteCount(msg);
                LogRecord record = new LogRecord
                (
                    start.ToString(datetimeFormat),
                    end.ToString(datetimeFormat),
                    ts.ToString(),
                    ts.Milliseconds.ToString(),
                    size.ToString()
                );

                if (showlog)
                {
                    Console.WriteLine("start:" + record.Start);
                    Console.WriteLine("end:" + record.End);
                    Console.WriteLine("timespan:" + record.TimeSpan);
                    Console.WriteLine("Interval:" + record.Interval + " ms");
                    Console.WriteLine("Msg Size:" + record.Size + " bytes");
                }

                await UpdateLogFileAsyncThingworx(record);
            }
            catch
            {
                Console.WriteLine("the message can't be used by Thingworx PerformanceTest. msg:" + msg);
            }
        }

        /// <summary>
        ///     Measure the speed of a message transmission from Consumer to Thingworx.
        ///     Special usage for messages produced by CPA.
        /// </summary>
        /// <param name="msg">
        ///     A received message.
        /// </param>
        /// <param name="consumeTime">
        ///     A timestamp when the consumer receive the message.
        /// </param>
        /// <param name="thwxTime">
        ///     A timestamp when the Thingworx receive the message. To be used in conjunction with RestService.getPropertyLastUpdate()
        /// </param>
        /// <param name="showlog">
        ///     Show the log on Console window if this value is true.
        /// </param>
        public static async Task CalcPerformanceThingworxfromCPA(string msg, string consumeTime, string thwxTime, Boolean showlog)
        {
            string datetimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff";

            try
            {
                var temp = thwxTime.Replace("\"", "");
                var start = DateTime.ParseExact(consumeTime, datetimeFormat, null);
                //Console.WriteLine("consumer: " + consumeTime);
                var end = DateTime.ParseExact(temp, datetimeFormat, null);
                //Console.WriteLine("thwx: " + temp);
                TimeSpan ts = end - start;
                int size = 108;
                LogRecord record = new LogRecord
                (
                    start.ToString(datetimeFormat),
                    end.ToString(datetimeFormat),
                    ts.ToString(),
                    ts.Milliseconds.ToString(),
                    size.ToString()
                );

                if (showlog)
                {
                    Console.WriteLine("start:" + record.Start);
                    Console.WriteLine("end:" + record.End);
                    Console.WriteLine("timespan:" + record.TimeSpan);
                    Console.WriteLine("Interval:" + record.Interval + " ms");
                    Console.WriteLine("Msg Size:" + record.Size + " bytes");
                }

                await UpdateLogFileAsyncThingworx(record);
            }
            catch
            {
                Console.WriteLine("the message can't be used by Thingworx PerformanceTest. msg:" + msg);
            }
        }

        /// <summary>
        ///     Update the log file of performance tests.
        /// </summary>
        /// <param name="record">
        ///     A logRecord for stores a result of a message performance test.
        /// </param>
        public static void UpdateLogFIle(LogRecord record)
        {
            string datetimeFormat = "yyyy-MM-dd";
            string filePath = $"./assets/logs/{DateTime.Today.ToString(datetimeFormat)}-test.csv";
            string bufferText;

            if (File.Exists(filePath))
            {
                bufferText = string.Format("{0},{1},{2},{3},{4}",
                                           record.Start,
                                           record.End,
                                           record.TimeSpan,
                                           record.Interval,
                                           record.Size);
            }
            else
            {
                bufferText = "start, end, timespan, interval, size";
            }

            WriteFile(filePath, bufferText);
        }

        /// <summary>
        ///     Update the log file of performance tests.
        /// </summary>
        /// <param name="record">
        ///     A logRecord for stores a result of a message performance test.
        /// </param>
        public static async Task UpdateLogFIleAsync(LogRecord record)
        {
            string datetimeFormat = "yyyy-MM-dd";
            string filePath = $"./assets/logs/{DateTime.Today.ToString(datetimeFormat)}-test.csv";
            string bufferText;

            if (File.Exists(filePath))
            {
                bufferText = string.Format("{0},{1},{2},{3},{4}",
                                           record.Start,
                                           record.End,
                                           record.TimeSpan,
                                           record.Interval,
                                           record.Size);
            }
            else
            {
                bufferText = "start, end, timespan, interval, size";
            }

            await WriteFileAsync(filePath, bufferText);
        }

        /// <summary>
        ///     Update the log file of performance tests for Thingworx.
        /// </summary>
        /// <param name="record">
        ///     A logRecord for stores a result of a message performance test.
        /// </param>
        public static async Task UpdateLogFileAsyncThingworx(LogRecord record)
        {
            string datetimeFormat = "yyyy-MM-dd";
            string filePath = $"./assets/logs/{DateTime.Today.ToString(datetimeFormat)}-thingworx-test.csv";
            string bufferText;

            if (File.Exists(filePath))
            {
                bufferText = string.Format("{0},{1},{2},{3},{4}",
                                           record.Start,
                                           record.End,
                                           record.TimeSpan,
                                           record.Interval,
                                           record.Size);
            }
            else
            {
                bufferText = "start, end, timespan, interval, size";
            }

            await WriteFileAsync(filePath, bufferText);
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

