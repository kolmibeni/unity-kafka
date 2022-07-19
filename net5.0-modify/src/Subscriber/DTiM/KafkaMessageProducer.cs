using Confluent.Kafka;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DTiM
{
    /// <summary>
    ///     Implements an IKafkaProducer. KafkaMessageProducer uses string-type messages in Kafka queue.
    /// </summary>
    public class KafkaMessageProducer: IKafkaProducer<string>
    {
        private IProducer<Null, string> Producer { get; set; }

        /// <summary>
        ///     Refer to <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ProducerConfig.html">Confluent.Kafka.ProducerConfig</a>.
        /// </summary>
        private ProducerConfig Config { get; set; }

        /// <summary>
        ///     Initialize KafkaMessageConsumer with a <see cref="DTiMProducerConfig" />.
        /// </summary>
        /// <param name="config">
        ///     refer to <see cref="DTiMProducerConfig" />
        /// </param>
        public KafkaMessageProducer(DTiMProducerConfig config)
        {
            Config = config.ToKafkaProducerConfig();
            Producer = new ProducerBuilder<Null, string>(Config).Build();
        }
        /// <summary>
        ///     Initialize KafkaMessageConsumer with a <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ProducerConfig.html">ProducerConfig</a>.
        /// </summary>
        /// <param name="config">
        ///     refer to <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ProducerConfig.html">Confluent.Kafka.ProducerConfig</a>.
        /// </param>
        public KafkaMessageProducer(ProducerConfig config)
        {
            Config = config;
            Producer = new ProducerBuilder<Null, string>(Config).Build();
        }

        /// <summary>
        ///     Send a string-type message to all consumers who subscribe to this topic.
        /// </summary>
        /// <param name="msg">
        ///     The message you want to send.
        /// </param>
        /// <param name="topic">
        ///     The topic you want to send.
        /// </param>
        public async Task SendMessage(string msg, string topic)
        {
            await Producer.ProduceAsync(topic, new Message<Null, string> { Value = msg });
            //producer.Flush(TimeSpan.FromSeconds(10));
            Producer.Flush();
        }
        /// <summary>
        ///     Send a string-type message to all consumers who subscribe to the topic.
        /// </summary>
        /// <param name="msg">
        ///     The message you want to send.
        /// </param>
        /// <param name="topics">
        ///     The topics you want to send.
        /// </param>
        /// <returns></returns>
        public async Task SendMessage(string msg, IEnumerable<string> topics)
        {
            foreach (string topic in topics)
            {
                await Producer.ProduceAsync(topic, new Message<Null, string> { Value = msg });
            }

            //producer.Flush(TimeSpan.FromSeconds(10));
            Producer.Flush();
        }
    }
}