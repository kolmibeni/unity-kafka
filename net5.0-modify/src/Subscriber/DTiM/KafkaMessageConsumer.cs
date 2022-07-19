using Confluent.Kafka;
using System.Collections.Generic;

namespace DTiM
{
    /// <summary>
    ///     Implements an IKafkaConsumer. KafkaMessageConsumer uses string-type messages in Kafka queue.
    /// </summary>
    public class KafkaMessageConsumer: IKafkaConsumer<string>
    {
        private IConsumer<Null, string> Consumer { get; set; }

        /// <summary>
        ///     Refer to <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ConsumerConfig.html">Confluent.Kafka.ConsumerConfig</a>.
        /// </summary>
        private ConsumerConfig Config { get; set; }

        /// <summary>
        ///     Initialize KafkaMessageConsumer with a <see cref="DTiMProducerConfig" />.
        /// </summary>
        /// <param name="config">
        ///     refer to <see cref="DTiMConsumerConfig" />
        /// </param>
        public KafkaMessageConsumer(DTiMConsumerConfig config)
        {
            Config = config.ToKafkaConsumerConfig();
            Consumer = new ConsumerBuilder<Null, string>(Config).Build();
        }
        /// <summary>
        ///     Initialize KafkaMessageConsumer with a <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ConsumerConfig.html">ConsumerConfig</a>.
        /// </summary>
        /// <param name="config">
        ///     refer to <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ConsumerConfig.html">Confluent.Kafka.ConsumerConfig</a>.
        /// </param>
        public KafkaMessageConsumer(ConsumerConfig config)
        {
            Config = config;
            Consumer = new ConsumerBuilder<Null, string>(Config).Build();
        }

        /// <summary>
        ///     Sets the subscription set to a single topic. Any previous subscription will be unassigned and unsubscribed first.
        /// </summary>
        /// <param name="topic">
        ///     The topic to subscribe to. A regex can be specified to subscribe to the set of all matching topics (which is updated as topics are added / removed from the cluster). A regex must be front anchored to be recognized as a regex. e.g. ^myregex
        /// </param>
        public void Subscribe(string topic)
        {
            Consumer.Subscribe(topic);
        }

        /// <summary>
        ///     Update the topic subscription. Any previous subscription will be unassigned and unsubscribed first.
        /// </summary>
        /// <param name="topics">
        ///     The topics to subscribe to. A regex can be specified to subscribe to the set of all matching topics (which is updated as topics are added / removed from the cluster). A regex must be front anchored to be recognized as a regex. e.g. ^myregex
        /// </param>
        public void Subscribe(IEnumerable<string> topics)
        {
            Consumer.Subscribe(topics);
        }

        /// <summary>
        ///     Get or consume a string-type message from the message queue.
        /// </summary>
        /// <returns>
        ///     Get a message from the producers that produce messages for these topics. 
        /// </returns>
        public string GetMessage()
        {
            ConsumeResult<Null, string> cr = Consumer.Consume();

            return cr.Message.Value;
        }

        /// <summary>
        ///     Close the consumer immediately. Always close() the consumer before exiting. This will close the network connections and sockets. It will also trigger a rebalance immediately rather than wait for the group coordinator to discover that the consumer stopped sending heartbeats and is likely dead, which will take longer and therefore result in a longer period of time in which consumers can't consume messages from a subset of the partitions.
        /// </summary>        
        public void Close()
        {
            Consumer.Close();
        }
    }
}