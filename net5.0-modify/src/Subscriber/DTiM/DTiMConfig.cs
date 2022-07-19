using Confluent.Kafka;
using System;

namespace DTiM
{
    /// <summary>
    ///     Defines a data model for ./DTiM/assets/config.json.
    /// </summary>
    public class DTiMConfig
    {
        /// <summary>
        ///     Defines the version of DTiMConfig
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Refer to <see cref="DTiMProducerConfig" />.
        /// </summary>
        public DTiMProducerConfig DtimProducerConfig { get; set; }

        /// <summary>
        ///     Refer to <see cref="DTiMConsumerConfig" />.
        /// </summary>
        public DTiMConsumerConfig DtimConsumerConfig { get; set; }

        /// <summary>
        ///     The parameters for PTC ThingWorx.
        ///     Refer to <see cref="ThingWorxConfig" />.
        /// </summary>
        public ThingWorxConfig ThingWorxConfig { get; set; }
    }

    /// <summary>
    ///     Defines a data model for ClientConfig.
    /// </summary>
    public class DTiMBaseConfig
    {
        //Parameters for ClientConfig.

        /// <summary>
        ///     Initial list of brokers as a CSV list of broker host or host:port. The application may also use `rd_kafka_brokers_add()` to add brokers during runtime.
        ///
        ///     default: ''
        ///     importance: high
        ///     alias: bootstrap.servers
        /// </summary>
        public string BootstrapServers { get; set; }

        /// <summary>
        ///     Client identifier.
        ///
        ///     default: rdkafka
        ///     importance: low
        ///     alias: client.id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     This field indicates the number of acknowledgements the leader broker must receive from ISR brokers
        ///     before responding to the request: Zero=Broker does not send any response/ack to client, One=The
        ///     leader will write the record to its local log but will respond without awaiting full acknowledgement
        ///     from all followers. All=Broker will block until message is committed by all in sync replicas (ISRs).
        ///     If there are less than min.insync.replicas (broker configuration) in the ISR set the produce request
        ///     will fail.
        /// </summary>
        public Acks? Acks { get; set; }

        /// <summary>
        ///     Maximum Kafka protocol request message size. Due to differing framing overhead between protocol versions the producer is unable to reliably enforce a strict max message limit at produce time and may exceed the maximum size by one message in protocol ProduceRequests, the broker will enforce the the topic's `max.message.bytes` limit (see Apache Kafka documentation).
        ///
        ///     default: 1000000
        ///     importance: medium
        ///     alias: message.max.bytes
        /// </summary>
        public int? MessageMaxBytes { get; set; }

        /// <summary>
        ///     Maximum size for message to be copied to buffer. Messages larger than this will be passed by reference (zero-copy) at the expense of larger iovecs.
        ///
        ///     default: 65535
        ///     importance: low
        ///     alias: message.copy.max.bytes
        /// </summary>
        public int? MessageCopyMaxBytes { get; set; }

        /// <summary>
        ///     Maximum Kafka protocol response message size. This serves as a safety precaution to avoid memory exhaustion in case of protocol hickups. This value must be at least `fetch.max.bytes`  + 512 to allow for protocol overhead; the value is adjusted automatically unless the configuration property is explicitly set.
        ///
        ///     default: 100000000
        ///     importance: medium
        ///     alias: receive.message.max.bytes
        /// </summary>
        public int? ReceiveMessageMaxBytes { get; set; }

        /// <summary>
        ///     Maximum number of in-flight requests per broker connection. This is a generic property applied to all broker communication, however it is primarily relevant to produce requests. In particular, note that other mechanisms limit the number of outstanding consumer fetch request per broker to one.
        ///
        ///     default: 1000000
        ///     importance: low
        ///     alias: max.in.flight
        /// </summary>
        public int? MaxInFlight { get; set; }

        /// <summary>
        ///     Period of time in milliseconds at which topic and broker metadata is refreshed in order to proactively discover any new brokers, topics, partitions or partition leader changes. Use -1 to disable the intervalled refresh (not recommended). If there are no locally referenced topics (no topic objects created, no messages produced, no subscription or no assignment) then only the broker list will be refreshed every interval but no more often than every 10s.
        ///
        ///     default: 300000
        ///     importance: low
        ///     alias: topic.metadata.refresh.interval.ms
        /// </summary>
        public int? TopicMetadataRefreshIntervalMs { get; set; }

        /// <summary>
        ///     Metadata cache max age. Defaults to topic.metadata.refresh.interval.ms * 3
        ///
        ///     default: 900000
        ///     importance: low
        ///     alias: metadata.max.age.ms
        /// </summary>
        public int? MetadataMaxAgeMs { get; set; }

        /// <summary>
        ///     When a topic loses its leader a new metadata request will be enqueued with this initial interval, exponentially increasing until the topic metadata has been refreshed. This is used to recover quickly from transitioning leader brokers.
        ///
        ///     default: 250
        ///     importance: low
        ///     alias: topic.metadata.refresh.fast.interval.ms
        /// </summary>
        public int? TopicMetadataRefreshFastIntervalMs { get; set; }

        /// <summary>
        ///     Sparse metadata requests (consumes less network bandwidth)
        ///
        ///     default: true
        ///     importance: low
        ///     alias: topic.metadata.refresh.sparse
        /// </summary>
        public bool? TopicMetadataRefreshSparse { get; set; }

        /// <summary>
        ///     Apache Kafka topic creation is asynchronous and it takes some time for a new topic to propagate throughout the cluster to all brokers. If a client requests topic metadata after manual topic creation but before the topic has been fully propagated to the broker the client is requesting metadata from, the topic will seem to be non-existent and the client will mark the topic as such, failing queued produced messages with `ERR__UNKNOWN_TOPIC`. This setting delays marking a topic as non-existent until the configured propagation max time has passed. The maximum propagation time is calculated from the time the topic is first referenced in the client, e.g., on produce().
        ///
        ///     default: 30000
        ///     importance: low
        ///     alias: topic.metadata.propagation.max.ms
        /// </summary>
        public int? TopicMetadataPropagationMaxMs { get; set; }

        /// <summary>
        ///     A comma-separated list of debug contexts to enable. Detailed Producer debugging: broker,topic,msg. Consumer: consumer,cgrp,topic,fetch
        ///
        ///     default: null
        ///     importance: medium
        ///     alias: debug
        /// </summary>
        public string Debug { get; set; }

        /// <summary>
        ///     Default timeout for network requests. Producer: ProduceRequests will use the lesser value of `socket.timeout.ms` and remaining `message.timeout.ms` for the first message in the batch. Consumer: FetchRequests will use `fetch.wait.max.ms` + `socket.timeout.ms`. Admin: Admin requests will use `socket.timeout.ms` or explicitly set `rd_kafka_AdminOptions_set_operation_timeout()` value.
        ///
        ///     default: 60000
        ///     importance: low
        ///     alias: socket.timeout.ms
        /// </summary>
        public int? SocketTimeoutMs { get; set; }

        /// <summary>
        ///     Broker socket send buffer size. System default is used if 0.
        ///
        ///     default: 0
        ///     importance: low
        ///     alias: socket.send.buffer.bytes
        /// </summary>
        public int? SocketSendBufferBytes { get; set; }

        /// <summary>
        ///     Broker socket receive buffer size. System default is used if 0.
        ///
        ///     default: 0
        ///     importance: low
        ///     alias: socket.receive.buffer.bytes
        /// </summary>
        public int? SocketReceiveBufferBytes { get; set; }

        /// <summary>
        ///     Enable TCP keep-alives (SO_KEEPALIVE) on broker sockets
        ///
        ///     default: false
        ///     importance: low
        ///     alias: socket.keepalive.enable
        /// </summary>
        public bool? SocketKeepaliveEnable { get; set; }

        /// <summary>
        ///     Disconnect from broker when this number of send failures (e.g., timed out requests) is reached. Disable with 0. WARNING: It is highly recommended to leave this setting at its default value of 1 to avoid the client and broker to become desynchronized in case of request timeouts. NOTE: The connection is automatically re-established.
        ///
        ///     default: 1
        ///     importance: low
        ///     alias: socket.max.fails
        /// </summary>
        public int? SocketMaxFails { get; set; }

        /// <summary>
        ///     How long to cache the broker address resolving results (milliseconds).
        ///
        ///     default: 1000
        ///     importance: low
        ///     alias: broker.address.ttl
        /// </summary>
        public int? BrokerAddressTtl { get; set; }

        /// <summary>
        ///     The initial time to wait before reconnecting to a broker after the connection has been closed. The time is increased exponentially until `reconnect.backoff.max.ms` is reached. -25% to +50% jitter is applied to each reconnect backoff. A value of 0 disables the backoff and reconnects immediately.
        ///
        ///     default: 100
        ///     importance: medium
        ///     alias: econnect.backoff.ms
        /// </summary>
        public int? ReconnectBackoffMs { get; set; }

        /// <summary>
        ///     Signal that librdkafka will use to quickly terminate on rd_kafka_destroy(). If this signal is not set then there will be a delay before rd_kafka_wait_destroyed() returns true as internal threads are timing out their system calls. If this signal is set however the delay will be minimal. The application should mask this signal as an internal signal handler is installed.
        ///
        ///     default: 0
        ///     importance: low
        ///     alias: internal.termination.signal
        /// </summary>
        public int? InternalTerminationSignal { get; set; }

        /// <summary>
        ///     Protocol used to communicate with brokers.
        ///
        ///     default: plaintext
        ///     importance: high
        ///     alias: security.protocol
        /// </summary>
        public SecurityProtocol? SecurityProtocol { get; set; }

        /// <summary>
        ///     Path to client's private key (PEM) used for authentication.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.key.location
        /// </summary>
        public string SslKeyLocation { get; set; }

        /// <summary>
        ///     Private key passphrase (for use with `ssl.key.location` and `set_ssl_cert()`)
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.key.password
        /// </summary>
        public string SslKeyPassword { get; set; }

        /// <summary>
        ///     Client's private key string (PEM format) used for authentication.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.key.pem
        /// </summary>
        public string SslKeyPem { get; set; }

        /// <summary>
        ///     Path to client's public key (PEM) used for authentication.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.certificate.location
        /// </summary>
        public string SslCertificateLocation { get; set; }

        /// <summary>
        ///     Client's public key string (PEM format) used for authentication.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.certificate.pem
        /// </summary>
        public string SslCertificatePem { get; set; }

        /// <summary>
        ///     File or directory path to CA certificate(s) for verifying the broker's key. Defaults: On Windows the system's CA certificates are automatically looked up in the Windows Root certificate store. On Mac OSX this configuration defaults to `probe`. It is recommended to install openssl using Homebrew, to provide CA certificates. On Linux install the distribution's ca-certificates package. If OpenSSL is statically linked or `ssl.ca.location` is set to `probe` a list of standard paths will be probed and the first one found will be used as the default CA certificate location path. If OpenSSL is dynamically linked the OpenSSL library's default path will be used (see `OPENSSLDIR` in `openssl version -a`).
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.ca.location
        /// </summary>
        public string SslCaLocation { get; set; }

        /// <summary>
        ///     Path to CRL for verifying broker's certificate validity.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.crl.location
        /// </summary>
        public string SslCrlLocation { get; set; }

        /// <summary>
        ///     Path to client's keystore (PKCS#12) used for authentication.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.keystore.location
        /// </summary>
        public string SslKeystoreLocation { get; set; }

        /// <summary>
        ///     Client's keystore (PKCS#12) password.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.keystore.password
        /// </summary>
        public string SslKeystorePassword { get; set; }

        /// <summary>
        ///     Path to OpenSSL engine library. OpenSSL >= 1.1.0 required.
        ///
        ///     default: ''
        ///     importance: low
        ///     alias: ssl.engine.location
        /// </summary>
        public string SslEngineLocation { get; set; }

        /// <summary>
        ///     Enable OpenSSL's builtin broker (server) certificate verification. This verification can be extended by the application by implementing a certificate_verify_cb.
        ///
        ///     default: true
        ///     importance: low
        ///     alias: enable.ssl.certificate.verification
        /// </summary>
        public bool? EnableSslCertificateVerification { get; set; }

        /// <summary>
        ///     Endpoint identification algorithm to validate broker hostname using broker certificate. https - Server (broker) hostname verification as specified in RFC2818. none - No endpoint verification. OpenSSL >= 1.0.2 required.
        ///
        ///     default: none
        ///     importance: low
        ///     alias: ssl.endpoint.identification.algorithm
        /// </summary>
        public SslEndpointIdentificationAlgorithm? SslEndpointIdentificationAlgorithm { get; set; }
    }

    /// <summary>
    ///     Defines a data model for <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ProducerConfig.html">Confluent.Kafka.ProducerConfig</a>.
    /// </summary>
    public class DTiMProducerConfig : DTiMBaseConfig
    {
        //Parameters for ProducerConfig.

        /// <summary>
        ///     The ack timeout of the producer request in milliseconds. This value is only enforced by the broker and relies on `request.required.acks` being != 0.
        ///
        ///     default: 30000
        ///     importance: medium
        ///     alias: request.timeout.ms
        /// </summary>
        public int? RequestTimeoutMs { get; set; }

        /// <summary>
        ///     Local message timeout. This value is only enforced locally and limits the time a produced message waits for successful delivery. A time of 0 is infinite. This is the maximum time librdkafka may use to deliver a message (including retries). Delivery error occurs when either the retry count or the message timeout are exceeded. The message timeout is automatically adjusted to `transaction.timeout.ms` if `transactional.id` is configured.
        ///
        ///     default: 300000
        ///     importance: high
        ///     alias: message.timeout.ms
        /// </summary>
        public int? MessageTimeoutMs { get; set; }

        /// <summary>
        ///     Compression level parameter for algorithm selected by configuration property `compression.codec`. Higher values will result in better compression at the cost of more CPU usage. Usable range is algorithm-dependent: [0-9] for gzip; [0-12] for lz4; only 0 for snappy; -1 = codec-dependent default compression level.
        ///
        ///     default: -1
        ///     importance: medium
        ///     alias: compression.level
        /// </summary>
        public int? CompressionLevel { get; set; }

        /// <summary>
        ///     The maximum amount of time in milliseconds that the transaction coordinator will wait for a transaction status update from the producer before proactively aborting the ongoing transaction. If this value is larger than the `transaction.max.timeout.ms` setting in the broker, the init_transactions() call will fail with ERR_INVALID_TRANSACTION_TIMEOUT. The transaction timeout automatically adjusts `message.timeout.ms` and `socket.timeout.ms`, unless explicitly configured in which case they must not exceed the transaction timeout (`socket.timeout.ms` must be at least 100ms lower than `transaction.timeout.ms`). This is also the default timeout value if no timeout (-1) is supplied to the transactional API methods.
        ///
        ///     default: 60000
        ///     importance: medium
        ///     alias: transaction.timeout.ms
        /// </summary>
        public int? TransactionTimeoutMs { get; set; }

        /// <summary>
        ///     When set to `true`, the producer will ensure that messages are successfully produced exactly once and in the original produce order. The following configuration properties are adjusted automatically (if not modified by the user) when idempotence is enabled: `max.in.flight.requests.per.connection=5` (must be less than or equal to 5), `retries=INT32_MAX` (must be greater than 0), `acks=all`, `queuing.strategy=fifo`. Producer instantation will fail if user-supplied configuration is incompatible.
        ///
        ///     default: false
        ///     importance: high
        ///     alias: enable.idempotence
        /// </summary>
        public bool? EnableIdempotence { get; set; }

        /// <summary>
        ///     Maximum number of messages allowed on the producer queue. This queue is shared by all topics and partitions.
        ///
        ///     default: 100000
        ///     importance: high
        ///     alias: queue.buffering.max.messages
        /// </summary>
        public int? QueueBufferingMaxMessages { get; set; }

        /// <summary>
        ///     Maximum total message size sum allowed on the producer queue. This queue is shared by all topics and partitions. This property has higher priority than queue.buffering.max.messages.
        ///
        ///     default: 1048576
        ///     importance: high
        ///     alias: queue.buffering.max.kbytes
        /// </summary>
        public int? QueueBufferingMaxKbytes { get; set; }

        /// <summary>
        ///     Delay in milliseconds to wait for messages in the producer queue to accumulate before constructing message batches (MessageSets) to transmit to brokers. A higher value allows larger and more effective (less overhead, improved compression) batches of messages to accumulate at the expense of increased message delivery latency.
        ///
        ///     default: 5
        ///     importance: high
        ///     alias: linger.ms
        /// </summary>
        public double? LingerMs { get; set; }

        /// <summary>
        ///     How many times to retry sending a failing Message. **Note:** retrying may cause reordering unless `enable.idempotence` is set to true.
        ///
        ///     default: 2147483647
        ///     importance: high
        ///     alias: message.send.max.retries
        /// </summary>
        public int? MessageSendMaxRetries { get; set; }

        /// <summary>
        ///     The backoff time in milliseconds before retrying a protocol request.
        ///
        ///     default: 100
        ///     importance: medium
        ///     alias: retry.backoff.ms"
        /// </summary>
        public int? RetryBackoffMs { get; set; }

        /// <summary>
        ///     The threshold of outstanding not yet transmitted broker requests needed to backpressure the producer's message accumulator. If the number of not yet transmitted requests equals or exceeds this number, produce request creation that would have otherwise been triggered (for example, in accordance with linger.ms) will be delayed. A lower number yields larger and more effective batches. A higher value can improve latency when using compression on slow machines.
        ///
        ///     default: 1
        ///     importance: low
        ///     alias: queue.buffering.backpressure.threshold
        /// </summary>
        public int? QueueBufferingBackpressureThreshold { get; set; }

        /// <summary>
        ///     compression codec to use for compressing message sets. This is the default value for all topics, may be overridden by the topic configuration property `compression.codec`.
        ///
        ///     default: none
        ///     importance: medium
        ///     alias: compression.type
        /// </summary>
        public CompressionType? CompressionType { get; set; }

        /// <summary>
        ///     Maximum number of messages batched in one MessageSet. The total MessageSet size is also limited by batch.size and message.max.bytes.
        ///
        ///     default: 10000
        ///     importance: medium
        ///     alias: batch.num.messages
        /// </summary>
        public int? BatchNumMessages { get; set; }

        /// <summary>
        ///     Maximum size (in bytes) of all messages batched in one MessageSet, including protocol framing overhead. This limit is applied after the first message has been added to the batch, regardless of the first message's size, this is to ensure that messages that exceed batch.size are produced. The total MessageSet size is also limited by batch.num.messages and message.max.bytes.
        ///
        ///     default: 1000000
        ///     importance: medium
        ///     alias: batch.size
        /// </summary>
        public int? BatchSize { get; set; }

        /// <summary>
        ///     Delay in milliseconds to wait to assign new sticky partitions for each topic. By default, set to double the time of linger.ms. To disable sticky behavior, set to 0. This behavior affects messages with the key NULL in all cases, and messages with key lengths of zero when the consistent_random partitioner is in use. These messages would otherwise be assigned randomly. A higher value allows for more effective batching of these messages.
        ///
        ///     default: 10
        ///     importance: low
        ///     alias: sticky.partitioning.linger.ms
        /// </summary>
        public int? StickyPartitioningLingerMs { get; set; }

        //Custom Config

        /// <summary>
        ///     The topics for this producer
        /// </summary>        
        public string[] Topics { get; set; }

        public ProducerConfig ToKafkaProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = this.BootstrapServers,
                ClientId = this.ClientId,
                Acks = this.Acks,
                MessageMaxBytes = this.MessageMaxBytes,
                MessageCopyMaxBytes = this.MessageCopyMaxBytes,
                ReceiveMessageMaxBytes = this.ReceiveMessageMaxBytes,
                MaxInFlight = this.MaxInFlight,
                TopicMetadataRefreshIntervalMs = this.TopicMetadataRefreshIntervalMs,
                MetadataMaxAgeMs = this.MetadataMaxAgeMs,
                TopicMetadataRefreshFastIntervalMs = this.TopicMetadataRefreshFastIntervalMs,
                TopicMetadataRefreshSparse = this.TopicMetadataRefreshSparse,
                TopicMetadataPropagationMaxMs = this.TopicMetadataPropagationMaxMs,
                Debug = this.Debug,
                SocketTimeoutMs = this.SocketTimeoutMs,
                SocketSendBufferBytes = this.SocketSendBufferBytes,
                SocketReceiveBufferBytes = this.SocketReceiveBufferBytes,
                SocketKeepaliveEnable = this.SocketKeepaliveEnable,
                SocketMaxFails = this.SocketMaxFails,
                BrokerAddressTtl = this.BrokerAddressTtl,
                ReconnectBackoffMs = this.ReconnectBackoffMs,
                InternalTerminationSignal = this.InternalTerminationSignal,
                SecurityProtocol = this.SecurityProtocol,
                SslKeyLocation = this.SslKeyLocation,
                SslKeyPassword = this.SslKeyPassword,
                SslKeyPem = this.SslKeyPem,
                SslCertificateLocation = this.SslCertificateLocation,
                SslCertificatePem = this.SslCertificatePem,
                SslCaLocation = this.SslCaLocation,
                SslCrlLocation = this.SslCrlLocation,
                SslKeystoreLocation = this.SslKeystoreLocation,
                SslKeystorePassword = this.SslKeystorePassword,
                SslEngineLocation = this.SslEngineLocation,
                EnableSslCertificateVerification = this.EnableSslCertificateVerification,
                SslEndpointIdentificationAlgorithm = this.SslEndpointIdentificationAlgorithm,

                RequestTimeoutMs = this.RequestTimeoutMs,
                MessageTimeoutMs = this.MessageTimeoutMs,
                CompressionLevel = this.CompressionLevel,
                TransactionTimeoutMs = this.TransactionTimeoutMs,
                EnableIdempotence = this.EnableIdempotence,
                QueueBufferingMaxMessages = this.QueueBufferingMaxMessages,
                QueueBufferingMaxKbytes = this.QueueBufferingMaxKbytes,
                LingerMs = this.LingerMs,
                MessageSendMaxRetries = this.MessageSendMaxRetries,
                RetryBackoffMs = this.RetryBackoffMs,
                QueueBufferingBackpressureThreshold = this.QueueBufferingBackpressureThreshold,
                CompressionType = this.CompressionType,
                BatchNumMessages = this.BatchNumMessages,
                BatchSize = this.BatchSize,
                StickyPartitioningLingerMs = this.StickyPartitioningLingerMs,
            };
        }
    }

    /// <summary>
    ///     Defines a data model for  <a href="https://kafka.apache.org/26/javadoc/org/apache/kafka/clients/producer/ConsumerConfig.html">Confluent.Kafka.ConsumerConfig</a>
    /// </summary>
    public class DTiMConsumerConfig : DTiMBaseConfig
    {
        //Parameters for ConsumerConfig.

        /// <summary>
        ///     Client group id string. All clients sharing the same group.id belong to the same group.
        ///
        ///     default: ''
        ///     importance: high
        ///     alias: group.id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        ///     Action to take when there is no initial offset in offset store or the desired offset is out of range: 'smallest','earliest' - automatically reset the offset to the smallest offset, 'largest','latest' - automatically reset the offset to the largest offset, 'error' - trigger an error (ERR__AUTO_OFFSET_RESET) which is retrieved by consuming messages and checking 'message->err'.
        ///
        ///     default: largest
        ///     importance: high
        ///     alias: auto.offset.reset"
        /// </summary>
        public string AutoOffsetReset { get; set; }

        /// <summary>
        ///     Client group session and failure detection timeout. The consumer sends periodic heartbeats (heartbeat.interval.ms) to indicate its liveness to the broker. If no hearts are received by the broker for a group member within the session timeout, the broker will remove the consumer from the group and trigger a rebalance. The allowed range is configured with the **broker** configuration properties `group.min.session.timeout.ms` and `group.max.session.timeout.ms`. Also see `max.poll.interval.ms`.
        ///
        ///     default: 45000
        ///     importance: high
        ///     alias: session.timeout.ms
        /// </summary>
        public int? SessionTimeoutMs { get; set; }

        /// <summary>
        ///     Group session keepalive heartbeat interval.
        ///
        ///     default: 3000
        ///     importance: low
        ///     alias: heartbeat.interval.ms
        /// </summary>
        public int? HeartbeatIntervalMs { get; set; }

        /// <summary>
        ///     How often to query for the current client group coordinator. If the currently assigned coordinator is down the configured query interval will be divided by ten to more quickly recover in case of coordinator reassignment.
        ///
        ///     default: 600000
        ///     importance: low
        ///     alias: coordinator.query.interval.ms
        /// </summary>
        public int? CoordinatorQueryIntervalMs { get; set; }

        /// <summary>
        ///     Maximum allowed time between calls to consume messages (e.g., rd_kafka_consumer_poll()) for high-level consumers. If this interval is exceeded the consumer is considered failed and the group will rebalance in order to reassign the partitions to another consumer group member. Warning: Offset commits may be not possible at this point. Note: It is recommended to set `enable.auto.offset.store=false` for long-time processing applications and then explicitly store offsets (using offsets_store()) *after* message processing, to make sure offsets are not auto-committed prior to processing has finished. The interval is checked two times per second. See KIP-62 for more information.
        ///
        ///     default: 300000
        ///     importance: high
        ///     alias: max.poll.interval.ms
        /// </summary>
        public int? MaxPollIntervalMs { get; set; }

        /// <summary>
        ///     The frequency in milliseconds that the consumer offsets are committed (written) to offset storage. (0 = disable). This setting is used by the high-level consumer.
        ///
        ///     default: 5000
        ///     importance: medium
        ///     alias: auto.commit.interval.ms
        /// </summary>
        public int? AutoCommitIntervalMs { get; set; }

        /// <summary>
        ///     Minimum number of messages per topic+partition librdkafka tries to maintain in the local consumer queue.
        ///
        ///     default: 100000
        ///     importance: medium
        ///     alias: queued.min.messages
        /// </summary>
        public int? QueuedMinMessages { get; set; }

        /// <summary>
        ///     Maximum number of kilobytes of queued pre-fetched messages in the local consumer queue. If using the high-level consumer this setting applies to the single consumer queue, regardless of the number of partitions. When using the legacy simple consumer or when separate partition queues are used this setting applies per partition. This value may be overshot by fetch.message.max.bytes. This property has higher priority than queued.min.messages.
        ///
        ///     default: 65536
        ///     importance: medium
        ///     alias: queued.max.messages.kbytes
        /// </summary>
        public int? QueuedMaxMessagesKbytes { get; set; }

        /// <summary>
        ///     Maximum time the broker may wait to fill the Fetch response with fetch.min.bytes of messages.
        ///
        ///     default: 500
        ///     importance: low
        ///     alias: fetch.wait.max.ms
        /// </summary>
        public int? FetchWaitMaxMs { get; set; }

        /// <summary>
        ///     Initial maximum number of bytes per topic+partition to request when fetching messages from the broker. If the client encounters a message larger than this value it will gradually try to increase it until the entire message can be fetched.
        ///
        ///     default: 1048576
        ///     importance: medium
        ///     alias: max.partition.fetch.bytes or fetch.message.max.bytes
        /// </summary>
        public int? MaxPartitionFetchBytes { get; set; }

        /// <summary>
        ///     Maximum amount of data the broker shall return for a Fetch request. Messages are fetched in batches by the consumer and if the first message batch in the first non-empty partition of the Fetch request is larger than this value, then the message batch will still be returned to ensure the consumer can make progress. The maximum message batch size accepted by the broker is defined via `message.max.bytes` (broker config) or `max.message.bytes` (broker topic config). `fetch.max.bytes` is automatically adjusted upwards to be at least `message.max.bytes` (consumer config).
        ///
        ///     default: 52428800
        ///     importance: medium
        ///     alias: fetch.max.bytes
        /// </summary>
        public int? FetchMaxBytes { get; set; }

        /// <summary>
        ///     Minimum number of bytes the broker responds with. If fetch.wait.max.ms expires the accumulated data will be sent to the client regardless of this setting.
        ///
        ///     default: 1
        ///     importance: low
        ///     alias: fetch.min.bytes
        /// </summary>
        public int? FetchMinBytes { get; set; }

        /// <summary>
        ///     How long to postpone the next fetch request for a topic+partition in case of a fetch error.
        ///
        ///     default: 500
        ///     importance: medium
        ///     alias: fetch.error.backoff.ms
        /// </summary>
        public int? FetchErrorBackoffMs { get; set; }

        /// <summary>
        ///     Controls how to read messages written transactionally: `read_committed` - only return transactional messages which have been committed. `read_uncommitted` - return all messages, even transactional messages which have been aborted.
        ///
        ///     default: read_committed
        ///     importance: high
        ///     alias: isolation.level
        /// </summary>
        public string IsolationLevel { get; set; }

        //Custom Config

        /// <summary>
        ///     The Topics you want the consumer to subscribe to.
        /// </summary>        
        public string[] Topics { get; set; }

        public ConsumerConfig ToKafkaConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = this.BootstrapServers,
                ClientId = this.ClientId,
                Acks = this.Acks,
                MessageMaxBytes = this.MessageMaxBytes,
                MessageCopyMaxBytes = this.MessageCopyMaxBytes,
                ReceiveMessageMaxBytes = this.ReceiveMessageMaxBytes,
                MaxInFlight = this.MaxInFlight,
                TopicMetadataRefreshIntervalMs = this.TopicMetadataRefreshIntervalMs,
                MetadataMaxAgeMs = this.MetadataMaxAgeMs,
                TopicMetadataRefreshFastIntervalMs = this.TopicMetadataRefreshFastIntervalMs,
                TopicMetadataRefreshSparse = this.TopicMetadataRefreshSparse,
                TopicMetadataPropagationMaxMs = this.TopicMetadataPropagationMaxMs,
                Debug = this.Debug,
                SocketTimeoutMs = this.SocketTimeoutMs,
                SocketSendBufferBytes = this.SocketSendBufferBytes,
                SocketReceiveBufferBytes = this.SocketReceiveBufferBytes,
                SocketKeepaliveEnable = this.SocketKeepaliveEnable,
                SocketMaxFails = this.SocketMaxFails,
                BrokerAddressTtl = this.BrokerAddressTtl,
                ReconnectBackoffMs = this.ReconnectBackoffMs,
                InternalTerminationSignal = this.InternalTerminationSignal,
                SecurityProtocol = this.SecurityProtocol,
                SslKeyLocation = this.SslKeyLocation,
                SslKeyPassword = this.SslKeyPassword,
                SslKeyPem = this.SslKeyPem,
                SslCertificateLocation = this.SslCertificateLocation,
                SslCertificatePem = this.SslCertificatePem,
                SslCaLocation = this.SslCaLocation,
                SslCrlLocation = this.SslCrlLocation,
                SslKeystoreLocation = this.SslKeystoreLocation,
                SslKeystorePassword = this.SslKeystorePassword,
                SslEngineLocation = this.SslEngineLocation,
                EnableSslCertificateVerification = this.EnableSslCertificateVerification,
                SslEndpointIdentificationAlgorithm = this.SslEndpointIdentificationAlgorithm,

                GroupId = this.GroupId,
                AutoOffsetReset = !string.IsNullOrEmpty(this.AutoOffsetReset) ?
                                  (AutoOffsetReset?)Enum.Parse(typeof(AutoOffsetReset), this.AutoOffsetReset) : null,
                SessionTimeoutMs = this.SessionTimeoutMs,
                HeartbeatIntervalMs = this.HeartbeatIntervalMs,
                CoordinatorQueryIntervalMs = this.CoordinatorQueryIntervalMs,
                MaxPollIntervalMs = this.MaxPollIntervalMs,
                AutoCommitIntervalMs = this.AutoCommitIntervalMs,
                QueuedMinMessages = this.QueuedMinMessages,
                QueuedMaxMessagesKbytes = this.QueuedMaxMessagesKbytes,
                FetchWaitMaxMs = this.FetchWaitMaxMs,
                MaxPartitionFetchBytes = this.MaxPartitionFetchBytes,
                FetchMaxBytes = this.FetchMaxBytes,
                FetchMinBytes = this.FetchMinBytes,
                FetchErrorBackoffMs = this.FetchErrorBackoffMs,
                IsolationLevel = !string.IsNullOrEmpty(this.IsolationLevel) ?
                                 (IsolationLevel?)Enum.Parse(typeof(IsolationLevel), this.IsolationLevel) : null
            };
        }
    }

    /// <summary>
    ///     Parameters of ThingWorx.
    /// </summary>
    public class ThingWorxConfig
    {
        /// <summary>
        ///     A app key for ThingWorx.
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        ///     The ThingWorx Server URL.
        /// </summary>
        public string ServerURL { get; set; }
    }
}

