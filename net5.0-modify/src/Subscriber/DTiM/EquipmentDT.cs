namespace DTiM
{
    /// <summary>
    ///     Implements an IDigitalTwin. EquipmentDT uses string-type messages in Kafka queue.
    /// </summary>
    public class EquipmentDT : IDigitalTwin<string>
    {
        /// <summary>
        ///     a string-type KafkaMessageConsumer.
        /// </summary>        
        private KafkaMessageConsumer _Consumer { get; set; }
        /// <summary>
        ///     a string-type KafkaMessageProducer.
        /// </summary>  
        private KafkaMessageProducer _Producer { get; set; }

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