namespace DTiM
{
    /// <summary>
    ///     Implements an IDigitalTwin. ProcessDT uses string-type messages in Kafka queue.
    /// </summary>
    public class ProcessDT : IDigitalTwin<string>
    {
        /// <summary>
        ///     a string-type KafkaMessageConsumer.
        /// </summary>      
        private KafkaMessageConsumer _Consumer;
        /// <summary>
        ///     a string-type KafkaMessageProducer.
        /// </summary> 
        private KafkaMessageProducer _Producer;

        /// <summary>
        ///     Implements a string-type kafkaConsumer.
        /// </summary>
        public IKafkaConsumer<string> KafkaConsumer
        {
            get => _Consumer;
            set => _Consumer = (KafkaMessageConsumer)value;
        }
        /// <summary>
        ///     Implements a string-type kafkaProducer.
        /// </summary>
        public IKafkaProducer<string> KafkaProducer
        {
            get => _Producer;
            set => _Producer = (KafkaMessageProducer)value;
        }
    }
}