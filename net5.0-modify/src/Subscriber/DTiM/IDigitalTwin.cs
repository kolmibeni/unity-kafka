
namespace DTiM
{
    /// <summary>
    ///     Defines a DigitalTwin. Each IDigitalTwin contains one Consumer and one Producer. You need to define the type of classes that implement IDigitalTwin. It will define the type of message you consumed or produced.
    /// </summary>
    interface IDigitalTwin<T>
    {
        /// <summary>
        ///     Defines KafkaConsumer
        /// </summary>
        IKafkaConsumer<T> KafkaConsumer { get; set; }
        /// <summary>
        ///     Defines kafkaProducer
        /// </summary>
        IKafkaProducer<T> KafkaProducer { get; set; }
    }
}