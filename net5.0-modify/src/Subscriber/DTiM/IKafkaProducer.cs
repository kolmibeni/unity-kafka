using System.Threading.Tasks;

namespace DTiM
{
    /// <summary>
    ///     Defines a IKafkaProducer.
    /// </summary>
    public interface IKafkaProducer<T>
    {
        /// <summary>
        ///     Defines a method that will send a message to all consumers that subscribed to this topic.
        /// </summary>
        /// <param name="msg">
        ///     A message you want to send.
        /// </param>
        /// <param name="topic">
        ///     A topic you want to subscribe to.
        /// </param>
        public Task SendMessage(string msg, string topic);
    }
}