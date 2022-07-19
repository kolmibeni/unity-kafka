using DTiM;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;


namespace Subscriber
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Title = "Consumer";
            string configFilePath = "./assets/config.json";
            Console.WriteLine("Consumer start.");

            // This part is for connecting to WebSocket and build a WebSocket Server in middleware
            // Install WebSocketSharp in Nuget Package Manager in Visual Studio
            var port = 8888;
            var wssv = new WebSocketServer(port);
            // At least add one service into server
            wssv.AddWebSocketService<Echo>("/Echo");
            wssv.Start();
            if (wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wssv.Port);
                // Print out all added WebSocket Server Service name
                foreach (var path in wssv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }

            //Initialize DTiM API
            DtimAppAPI dtimAPI = new DtimAppAPI(KafkaRole.Consumer, configFilePath);

            try
            {
                //Test code related with Kafka
                while (true)
                {
                    //get or consume the message from the producer.
                    string msg = dtimAPI.GetMessage();

                    string timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff");       

                    //Ex5 receive OP2 data from CPA
                    bool useEx5 = true;
                    if (useEx5)
                    {
                        //Remove all backslash from original kafka message
                        string msg_clean = msg.Replace("(UTC)", "_UTC");
                        msg_clean = msg_clean.Replace("(LOCAL)", "_LOCAL");

                        //Deserialize Kafka message into Json objects
                        TestMessageRobot testMsg = JsonConvert.DeserializeObject<TestMessageRobot>(msg_clean);
                        
                        // OP_machine_data op_msg = JsonConvert.DeserializeObject<DTiM.OP_machine_data>(testMsg.Msg);
                        OP_machine_data op_msg = JsonConvert.DeserializeObject<DTiM.OP_machine_data>(msg_clean);
                        
                        //Broadcast the msg_clean string to all connected websocket client
                        wssv.WebSocketServices.Broadcast(msg_clean + "SendingTime:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                        Console.WriteLine("\nBroadcast to WebSocket at port:{0}, with path:{1}.",port,"Echo");
                        
                    }
                    //Ex5 End

                    // show the message.
                    Console.WriteLine(timestamp + " Message:" + msg);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }

            dtimAPI.CloseConsumer();
        }
    }
}