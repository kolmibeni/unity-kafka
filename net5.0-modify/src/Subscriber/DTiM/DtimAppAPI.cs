using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DTiM
{
    /// <summary>
    ///     Defines a DTiM app API layer.
    /// </summary>
    public class DtimAppAPI
    {
        private KafkaMessageProducer Producer { get; set; }
        private KafkaMessageConsumer Consumer { get; set; }

        /// <summary>
        ///     The path of the configuration file.
        /// </summary>
        private string ConfigFilePath { get; set; }
        /// <summary>
        ///     Refer to <see cref="DTiMConfig" />.
        /// </summary>
        private DTiMConfig DtimConfig { get; set; }
        /// <summary>
        ///     Refer to <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ProducerConfig.html">Confluent.Kafka.ProducerConfig</a>.
        /// </summary>
        private ProducerConfig ProducerConfig { get; set; }
        /// <summary>
        ///     Refer to <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ConsumerConfig.html">Confluent.Kafka.ConsumerConfig</a>.
        /// </summary>
        private ConsumerConfig ConsumerConfig { get; set; }
        /// <summary>
        ///     Refer to <see cref="MockDataGenerator" />.
        /// </summary>
        private MockDataGenerator MockDataGen { get; set; }

        public DtimAppAPI()
        {
        }
        /// <summary>
        ///     Initialize DtimAppAPI with the specific <see cref="KafkaRole" /> and specific configuration file path.
        /// </summary>
        /// <param name="kafkaRole">
        ///     Refer to <see cref="KafkaRole" />.
        /// </param>
        /// <param name="configFilePath">
        ///     The path of the configuration file you specified.
        /// </param>
        public DtimAppAPI(KafkaRole kafkaRole, string configFilePath)
        {
            ConfigFilePath = configFilePath;

            LoadConfigFile(ConfigFilePath);

            switch (kafkaRole)
            {
                case KafkaRole.Producer:
                    Producer = new KafkaMessageProducer(ProducerConfig);
                    break;
                case KafkaRole.Consumer:
                    Consumer = new KafkaMessageConsumer(ConsumerConfig);
                    Subscribe();
                    break;
                case KafkaRole.Both:
                default:
                    Producer = new KafkaMessageProducer(ProducerConfig);
                    Consumer = new KafkaMessageConsumer(ConsumerConfig);
                    break;
            }

            MockDataGen = new MockDataGenerator(this, "./test-data/Total_CT350.csv");
        }
        /// <summary>
        ///     Initialize DtimAppAPI with the specific <see cref="KafkaRole" /> and the <see cref="DTiMConfig" />.
        /// </summary>
        /// <param name="kafkaRole">
        ///     refer to <see cref="KafkaRole" />.
        /// </param>
        /// <param name="config">
        ///     refer to <see cref="DTiMConfig" />.
        /// </param>
        public DtimAppAPI(KafkaRole kafkaRole, DTiMConfig config)
        {
            DtimConfig = config;
            ProducerConfig = config.DtimProducerConfig.ToKafkaProducerConfig();
            ConsumerConfig = config.DtimConsumerConfig.ToKafkaConsumerConfig();

            switch (kafkaRole)
            {
                case KafkaRole.Producer:
                    Producer = new KafkaMessageProducer(ProducerConfig);
                    break;
                case KafkaRole.Consumer:
                    Consumer = new KafkaMessageConsumer(ConsumerConfig);
                    Subscribe();
                    break;
                case KafkaRole.Both:
                default:
                    Producer = new KafkaMessageProducer(ProducerConfig);
                    Consumer = new KafkaMessageConsumer(ConsumerConfig);
                    break;
            }

            MockDataGen = new MockDataGenerator(this, "./test-data/Total_CT350.csv");
        }
        /// <summary>
        ///     Initialize DtimAppAPI with the specific <see cref="KafkaRole" /> and using the configuration that defines in './assets/config.json'.
        /// </summary>
        /// <param name="kafkaRole">
        ///     Refer to <see cref="KafkaRole" />.
        /// </param>
        public DtimAppAPI(KafkaRole kafkaRole)
        {
            ConfigFilePath = "./assets/config.json";

            LoadConfigFile(ConfigFilePath);

            switch (kafkaRole)
            {
                case KafkaRole.Producer:
                    Producer = new KafkaMessageProducer(ProducerConfig);
                    break;
                case KafkaRole.Consumer:
                    Consumer = new KafkaMessageConsumer(ConsumerConfig);
                    Subscribe();
                    break;
                case KafkaRole.Both:
                default:
                    Producer = new KafkaMessageProducer(ProducerConfig);
                    Consumer = new KafkaMessageConsumer(ConsumerConfig);
                    break;
            }

            MockDataGen = new MockDataGenerator(this, "./test-data/Total_CT350.csv");
        }

        /// <summary>
        ///     Load the settings from the json configuration file './config.json'.
        /// </summary>
        private void LoadConfigFile(string filePath)
        {
            //string rootDir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string jsonString = File.ReadAllText(filePath);
            DtimConfig = JsonConvert.DeserializeObject<DTiMConfig>(jsonString);
            ProducerConfig = DtimConfig.DtimProducerConfig.ToKafkaProducerConfig();
            ConsumerConfig = DtimConfig.DtimConsumerConfig.ToKafkaConsumerConfig();
        }

        /// <summary>
        ///     Send a string-type message to all consumers who subscribe to this topic.
        /// </summary>
        /// <typeparam name="T">
        ///     Specific type.
        /// </typeparam>
        /// <param name="obj">
        ///     The message you want to send with specific type.
        /// </param>
        /// <param name="topic">
        ///     The topic you want to send.
        /// </param>
        /// <returns></returns>
        public async Task SendMessage<T>(T obj, string topic)
        {
            string msg = typeof(T).Equals(typeof(string)) ? obj as string : JsonConvert.SerializeObject(obj);

            await Producer.SendMessage(msg, topic);
        }

        /// <summary>
        ///     Send a string-type message to all consumers who subscribe to the topic you defined in the configuration file './config.json'.
        /// </summary>
        /// <typeparam name="T">
        ///     Specific type.
        /// </typeparam>
        /// <param name="obj">
        ///     The message you want to send with specific type.
        /// </param>
        /// <returns></returns>
        public async Task SendMessage<T>(T obj)
        {
            string msg = typeof(T).Equals(typeof(string)) ? obj as string : JsonConvert.SerializeObject(obj);

            await Producer.SendMessage(msg, DtimConfig.DtimProducerConfig.Topics);
        }

        /// <summary>
        ///     Subscribe to a new topic and the new topic will overwrite all the old topics.
        /// </summary>
        /// <param name="topic">
        ///     The topic you want to subscribe to.
        /// </param>
        public void Subscribe(string topic)
        {
            Consumer.Subscribe(topic);
        }

        /// <summary>
        ///     Subscribe to the topics you define in the file './assets/config.json'. It will subscribe to all topics in the list DtimConfig.DtimConsumerConfig.Topics.
        /// </summary>
        public void Subscribe()
        {
            Consumer.Subscribe(DtimConfig.DtimConsumerConfig.Topics);
        }

        /// <summary>
        ///     Get or consume a specific type message from the message queue.
        /// </summary>
        /// <returns>
        ///     Get a message from the producers that produce messages for these topics. 
        /// </returns>
        public T GetMessage<T>()
        {
            string result = Consumer.GetMessage();

            return typeof(T).Equals(typeof(string)) ? (T)Convert.ChangeType(result, typeof(T)) : JsonConvert.DeserializeObject<T>(result);
        }
        /// <summary>
        ///     Get or consume a string-type message from the message queue.
        /// </summary>
        /// <returns>
        ///     Get a message from the producers that produce messages for these topics.
        /// </returns>
        public string GetMessage()
        {
            return Consumer.GetMessage();
        }

        /// <summary>
        ///     Close the consumer immediately. Always close() the consumer before exiting. This will close the network connections and sockets. It will also trigger a rebalance immediately rather than wait for the group coordinator to discover that the consumer stopped sending heartbeats and is likely dead, which will take longer and therefore result in a longer period of time in which consumers can't consume messages from a subset of the partitions.
        /// </summary>
        public void CloseConsumer()
        {
            Consumer.Close();
        }

        /// <summary>
        ///     Produce mock data from a CSV file for testing.
        /// </summary>
        public async Task ProduceMockDataFromCSVFile()
        {
            //Measure time cost.
            Stopwatch watch = new Stopwatch();
            //Start measure.
            watch.Start();

            //Send a start signal message to consumers.
            var startMsg = new
            {
                Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff"),
                Msg = "Start"
            };
            await Producer.SendMessage(JsonConvert.SerializeObject(startMsg), "vibration");

            //MockDataGenerator can generate mock data from a CSV file.
            //Produce mock data from a CSV.
            int interval_by_msec = 1000; //interval in msec
            await MockDataGen.ProduceDataFromCSV(interval_by_msec);

            //End measure.
            watch.Stop();

            //Show interval
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            //Send a end signal message to consumers.
            var endMsg = new
            {
                Timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss.ffff"),
                Msg = "End"
            };
            await Producer.SendMessage(JsonConvert.SerializeObject(endMsg), "vibration");
        }

        /// <summary>
        ///     Produce mock data in OP2 format.
        /// </summary>
        public async Task GetWHL55_SensorData()
        {
            await MockDataGen.ProduceWHL55_SensorData();
        }
    }

    public enum KafkaRole
    {
        Both,
        Producer,
        Consumer
    }
}