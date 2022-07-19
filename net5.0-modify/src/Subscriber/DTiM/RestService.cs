using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using ChoETL;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace DTiM
{

    /// <summary>
    ///     Defines a service layer for interacting with RESTful API.
    /// </summary>
    public class RestService
    {
        private static HttpClientHandler ClientHandler { get; set; }
        private static HttpClient Client { get; set; }

        /// <summary>
        ///     refer to <see cref="DTiM.DTiMConfig" />
        /// </summary>
        private DTiMConfig DtimConfig { get; set; }
        /// <summary>
        ///     refer to <see cref="DTiM.ThingWorxConfig" />
        /// </summary>
        public ThingWorxConfig ThingWorxConfig { get; set; }

        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public RestService(string confgiFilePath)
        {
            LoadConfigFile(confgiFilePath);

            ClientHandler = new HttpClientHandler();
            Client = new HttpClient(ClientHandler);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("appKey", ThingWorxConfig.AppKey);
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        ///     load the configuration from the config file './assets/config.json'. 
        /// </summary>
        private void LoadConfigFile(string filePath)
        {
            //var rootDir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string jsonString = File.ReadAllText(filePath);
            DtimConfig = JsonConvert.DeserializeObject<DTiMConfig>(jsonString);

            ThingWorxConfig = DtimConfig.ThingWorxConfig;
        }

        /// <summary>
        ///    Send a GET request to get the PropertyValue of a Thing
        /// </summary>
        /// <returns>
        ///     PropertyValue of a Thing
        /// </returns>
        /// <param name="thingName">
        ///     A Thing name you want to get its Property.
        /// </param>
        /// <param name="propertyName">
        ///     A Property name you want to get its Value.
        /// </param>
        public async Task<string> GetRestMessage(string thingName, string propertyName)
        {
            //Send request
            try
            {
                var msg = await Client.GetStringAsync($"{ThingWorxConfig.ServerURL}/Thingworx/Things/{thingName}/Properties/{propertyName}");

                //Parse message to array
                List<string> propertyList = msg.Split('\n').ToList();
                propertyList.Reverse();

                return propertyList[1];
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());

                return e.ToString();
            }
        }

        /// <summary>
        ///    Send a PUT request to set the PropertyValue of a Thing
        /// </summary>
        /// <returns>
        ///     PropertyValue of a Thing
        /// </returns>
        /// <param name="thingName">
        ///     A Thing name you want to set its Property.
        /// </param>
        /// <param name="propertyName">
        ///     A Property name you want to set its Value.
        /// </param>
        /// <param name="propertyValue">
        ///     Value of the property in JSON format: { "propertyName":"value"}
        /// </param>
        public async Task SendRestMessage(string thingName, string propertyName, string propertyValue)
        {
            StringContent data = new StringContent(propertyValue, Encoding.UTF8, "application/json");

            //Send request
            try
            {
                var msg = await Client.PutAsync($"{ThingWorxConfig.ServerURL}/Thingworx/Things/{thingName}/Properties/{propertyName}", data);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        /// <summary>
        ///    Update Thing properties based on the message received from Kafka broker
        /// </summary>
        /// <param name="msg">
        ///     Message received from Kafka broker in json format, timestamp already trimmed 
        /// </param>
        /// <param name="thingName">
        ///     A Thing name you want to set its Properties.
        /// </param>
        public async Task UpdateThingworxProperties(string msg, string thingName)
        {
            try
            {
                List<string> propertyList = msg.Trim(new char[] { '{', '}' }).Split(',').ToList();

                foreach (string property in propertyList)
                {
                    List<string> tempList = sWhitespace.Replace(property, "").Split('=').ToList();
                    string propertyName = tempList[0].ToString();
                    string propertyValue = tempList[1].ToString();

                    await SendRestMessage(thingName, propertyName, "{\"" + propertyName + "\":" + propertyValue + "}");
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        /// <summary>
        ///    Update Thing properties based on the message received from Kafka broker. 
        ///    Special usage for messages produced by CPA.
        /// </summary>
        /// <param name="msg">
        ///     Message received from Kafka broker in json format, timestamp already trimmed 
        /// </param>
        /// <param name="thingName">
        ///     A Thing name you want to set its Properties.
        /// </param>
        public async Task UpdateThingworxPropertiesfromCPA(TestMessage msg, string thingName)
        {
            try
            {
                string[] propertyNames = new string[] { "Cur_M_250T", "Cur_M_50T", "Cur_X", "Cur_Y", "Cur_Z", "Cur_B", "Vib_M", "Vib_Z" };

                foreach (string propertyName in propertyNames)
                {
                    string propertyValue = "0.0";

                    switch (propertyName)
                    {
                        case "Cur_M_250T":
                            propertyValue = msg.Msg.Cur_M_250T.ToString();
                            break;
                        case "Cur_M_50T":
                            propertyValue = msg.Msg.Cur_M_50T.ToString();
                            break;
                        case "Cur_X":
                            propertyValue = msg.Msg.Cur_X.ToString();
                            break;
                        case "Cur_Y":
                            propertyValue = msg.Msg.Cur_Y.ToString();
                            break;
                        case "Cur_Z":
                            propertyValue = msg.Msg.Cur_Z.ToString();
                            break;
                        case "Cur_B":
                            propertyValue = msg.Msg.Cur_B.ToString();
                            break;
                        case "Vib_M":
                            propertyValue = msg.Msg.Vib_M.ToString();
                            break;
                        case "Vib_Z":
                            propertyValue = msg.Msg.Vib_Z.ToString();
                            break;
                        default:
                            break;
                    }

                    await SendRestMessage(thingName, propertyName, "{\"" + propertyName + "\":" + propertyValue + "}");
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        /// <summary>
        ///    Send a GET request to get the last modified date of a Property
        /// </summary>
        /// <returns>
        ///     PropertyValue of a Thing
        /// </returns>
        /// <param name="thingName">
        ///     A Thing name you want to get its Property.
        /// </param>
        /// <param name="propertyName">
        ///     A Property name you want to get its Value.
        /// </param>
        /// <param name="verbose">
        ///     Option if you want to display the created URL for debugging.
        /// </param>
        public async Task<string> GetPropertyLastUpdate(string thingName, string propertyName, bool verbose = false)
        {
            //Send request
            try
            {
                string _url = $"{ThingWorxConfig.ServerURL}/Thingworx/Things/{thingName}/Services/GetPropertyUpdateTimeLong?method=POST&propertyName={propertyName}";
                if (verbose)
                {
                    Console.WriteLine(_url);
                }
                return await Client.GetStringAsync(_url);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());

                return e.ToString();
            }
        }

        /// <summary>
        ///    Send a GET request to get the last modified date of a Property
        /// </summary>
        /// <returns>
        ///     PropertyValue of a Thing
        /// </returns>
        /// <param name="thingName">
        ///     A Thing name you want to get its Property.
        /// </param>
        /// <param name="Cur_M">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="Cur_X">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="Cur_Z">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="Vib_M">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="Vib_D">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="verbose">
        ///     Option if you want to display the created URL for debugging.
        /// </param>
        public async Task<string> UpdatePropertiesSensorData(string thingName, string Cur_M, string Cur_X, string Cur_Z, string Vib_M, string Vib_D, bool verbose = false)
        {

            //Send request
            try
            {                
                string _url = $"{ ThingWorxConfig.ServerURL}/Thingworx/Things/{thingName}/Services/UpdateProperties?input_Cur_M={Cur_M}&input_Cur_X={Cur_X}&input_Cur_Z={Cur_Z}&input_Vib_M={Vib_M}&input_Vib_D={Vib_D}";
                if (verbose)
                {
                    Console.WriteLine(_url);
                }
                var stringTask = Client.GetStringAsync(_url);
                var msg = await stringTask;

                return (msg);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return e.ToString();
            }
            //HttpClient end
        }

        /// <summary>
        ///    Send a GET request to get the last modified date of a Property
        /// </summary>
        /// <returns>
        ///     PropertyValue of a Thing
        /// </returns>
        /// <param name="thingName">
        ///     A Thing name you want to get its Property.
        /// </param>
        /// <param name="J1">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J2">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J3">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J4">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J5">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J6">
        ///     Value of Cur_M to be stored.
        /// </param>
        public async Task<string> UpdatePropertiesRobotArm(string thingName, string Timetag_UTC, string Timetag_LOCAL, string J1, string J2, string J3, string J4, string J5, string J6, string Robot1, string Robot2, bool verbose = false)
        {

            //Send request
            try
            {
                string url_request = $"{ThingWorxConfig.ServerURL}/Thingworx/Things/{thingName}/Services/UpdateProperties?method=POST&input_Timetag_UTC={Timetag_UTC}&input_Timetag_LOCAL={Timetag_LOCAL}&input_J1={J1}&input_J2={J2}&input_J3={J3}&input_J4={J4}&input_J5={J5}&input_J6={J6}&input_Robot1={Robot1}&input_Robot2={Robot2}";
                if (verbose)
                {
                    Console.WriteLine(url_request);
                }
                var stringTask = Client.GetStringAsync(url_request);
                var msg = await stringTask;

                return (msg);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return e.ToString();
            }
            //HttpClient end
        }

        /// <summary>
        ///    Send a GET request to get the last modified date of a Property
        /// </summary>
        /// <returns>
        ///     PropertyValue of a Thing
        /// </returns>
        /// <param name="thingName">
        ///     A Thing name you want to get its Property.
        /// </param>
        /// <param name="J1">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J2">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J3">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J4">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J5">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="J6">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="LeftDoor">
        ///     Value of Cur_M to be stored.
        /// </param>
        /// <param name="RightDoor">
        ///     Value of Cur_M to be stored.
        /// </param>
        public async Task<string> UpdatePropertiesOPMachine(string thingName, string Timetag_UTC, string Timetag_LOCAL, string LastUpdateTime_LOCAL, string ToolCode, string ProgramNum, string X, string Y, string Z, string A, string B, string LeftDoor, string RightDoor, string ChangingTool, string SpindleSpinLight, string SpindleAngle, string CutterRradiusOfWear, string CutterRradiusOfGeometry, string ToolLengthOfWear, string ToolLengthOfGeometry, string SpindleSpeed, bool verbose = false)
        {

            //Send request
            try
            {
                if (X == "--" || X == "NaN")
                {
                    X = "0";
                }
                if (Y == "--" || Y == "NaN")
                {
                    Y = "0";
                }
                if (Z == "--" || Z == "NaN")
                {
                    Z = "0";
                }
                if (A == "--" || A == "NaN")
                {
                    A = "0";
                }
                if (B == "--" || B == "NaN")
                {
                    B = "0";
                }
                if (ToolCode == null || ToolCode == "")
                {
                    ToolCode = "0";
                }
                if (ProgramNum == null || ProgramNum == "")
                {
                    ProgramNum = "0";
                }
                if (ChangingTool == null || ChangingTool == "")
                {
                    ChangingTool = "null";
                }
                if (SpindleAngle == null || SpindleAngle == "")
                {
                    SpindleAngle = "0";
                }
                if (SpindleSpinLight == null || SpindleSpinLight == "")
                {
                    SpindleSpinLight = "null";
                }


                string _url = $"{ThingWorxConfig.ServerURL}/Thingworx/Things/{thingName}/Services/UpdateProperties?method=POST&input_Timetag_UTC={Timetag_UTC}&input_Timetag_LOCAL={Timetag_LOCAL}&input_LastUpdateTime_LOCAL={LastUpdateTime_LOCAL}&input_ToolCode={ToolCode}&input_ProgramNum={ProgramNum}&input_X={X}&input_Y={Y}&input_Z={Z}&input_A={A}&input_B={B}&input_LeftDoor={LeftDoor}&input_RightDoor={RightDoor}&input_ChangingTool={ChangingTool}&input_SpindleSpinLight={SpindleSpinLight}&input_SpindleAngle={SpindleAngle}&input_CutterRradiusOfWear={CutterRradiusOfWear}&input_CutterRradiusOfGeometry={CutterRradiusOfGeometry}&input_ToolLengthOfWear={ToolLengthOfWear}&input_ToolLengthOfGeometry={ToolLengthOfGeometry}&input_SpindleSpeed={SpindleSpeed}";
                if (verbose)
                {
                    Console.WriteLine(_url);
                }
                var stringTask = Client.GetStringAsync(_url);
                var msg = await stringTask;

                return (msg);
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return e.ToString();
            }
            //HttpClient end
        }



        /// <summary>
        ///    Test REST function by calling GitHub API lists
        /// </summary>
        public async Task ProcessRepositories()
        {
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            Client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var msg = await Client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

            Console.Write(msg);
        }

        /// <summary>
        ///    Parse timestamp from Thingworx to a proper DateTime format to be used by PerformanceTest class
        /// </summary>
        /// <returns>
        ///     Timestamp when a property is changed
        /// </returns>
        /// <param name="thingName">
        ///     A Thing name you want to get its Property.
        /// </param>
        /// <param name="propertyName">
        ///     A Property name you want to get its Value.
        /// </param>
        public string ParseDateTimeForPerformanceTest(string thwxTime)
        {
            try
            {
                //String manipulation
                string thwx_receive_time = thwxTime;
                thwx_receive_time = thwx_receive_time.Replace("\"", "");
                string search = "rows:";
                thwx_receive_time = thwx_receive_time.Substring(thwx_receive_time.IndexOf(search) + search.Length);
                search = "result:";
                thwx_receive_time = thwx_receive_time.Substring(thwx_receive_time.IndexOf(search) + search.Length);
                thwx_receive_time = thwx_receive_time.TrimEnd('\r', '\n').Replace("}", "").Replace("]", "");

                return thwx_receive_time;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());

                return e.ToString();
            }
        }

        /// <summary>
        ///     Upload all IPM result from the /assets/ipm-result.csv
        /// </summary>
        public async Task UploadIpmResultFromCsv()
        {
            Console.WriteLine("");

            string record = "";
            string thingName = "IPM_Result_Thing";
            FileStream fileStream = new FileStream("./assets/ipm-result-20210928.csv", FileMode.Open);

            using (StreamReader reader = new StreamReader(fileStream))
            {
                string header = reader.ReadLine();
                Console.WriteLine(header);
                List<string> headerList = header.Split(',').ToList();

                while ((record = reader.ReadLine()) != null)
                {
                    List<string> recordList = record.Split(',').ToList();
                    Console.WriteLine(record);

                    for (int i = 0; i < 10; i++)
                    {
                        string propertyName = headerList[i];
                        string propertyValue = recordList[i];
                        await SendRestMessage(thingName, propertyName, "{\"" + propertyName + "\":" + propertyValue + "}");
                    }

                    int samples = int.Parse(recordList[0]);

                    if (samples >= 2565)
                    {
                        Thread.Sleep(3000);
                    }
                    else if (samples < 2565 && samples >= 2500)
                    {
                        Thread.Sleep(333);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }
    }
}