
namespace DTiM
{
    /// <summary>
    ///     Defines a Kafka consumer.
    /// </summary>
    public interface IKafkaConsumer<T>
    {
        /// <summary>
        ///     defines a method that can subscribe to a new topic.
        /// </summary>
        /// <param name="topic">
        ///     The topic you want to subscribe to.
        /// </param>
        /// <returns>
        ///     void
        /// </returns>
        public void Subscribe(string topic);
        /// <summary>
        ///     define a method that can get or consume a message from the message queue.
        /// </summary>
        /// <returns>
        ///     You need to define the type of the message 
        /// </returns>
        public T GetMessage();
    }
}