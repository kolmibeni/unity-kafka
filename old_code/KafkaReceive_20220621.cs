using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//added for Kafka
using Confluent.Kafka;
// using System.Threading;
using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

public class KafkaReveive : MonoBehaviour
{
    public TextMesh text;

     //Parameters for Kafka thread
    bool kafkaStarted = false;
    // Thread kafkaThread;
    KafkaHandle _handle;
    
    // Start is called before the first frame update
    void Start()
    {
        //Start the Kafka thread
        StartKafka();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("Cancelling Kafka!");
            StopKafka();
        }
        //Process incoming messages from Kafka stream
        ProcessKafkaMessage();
    }

    
    //class for handling Kafka thread
    public class KafkaHandle
    {
        ConsumerConfig config;
        Message<Ignore, string> message = null; 
        // public readonly ConcurrentQueue<StreamMessage> _queue = new ConcurrentQueue<StreamMessage>();


        public void StartKafkaListener()
        {
            Debug.Log("Kafka - Starting Thread..");
            try
            {
                config = new ConsumerConfig
                {
                    GroupId = "gid-unity-test",
                    BootstrapServers = "140.116.86.241:29092,140.116.86.241:29093,140.116.86.241:29094",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                Debug.Log("Kafka - Created config");

                using (var c = new ConsumerBuilder<null, string>(config).Build())
                {
                    c.Subscribe("unity-test");
                    Debug.Log("Kafka - Subscribed");

                    // CancellationTokenSource cts = new CancellationTokenSource();
                    // Console.CancelKeyPress += (_, e) => {
                    //     e.Cancel = true; //prevent the process from terminating
                    //     cts.Cancel();
                    // };

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                //Waiting for message
                                var cr = c.Consume();
                                // ConsumeResult<Null, string> cr = c.Consume();

                                message = cr.Message;
                                Debug.Log(message.Value);
                                //Got message! Decode and put on queue
                                //StreamMessage message = ParseStreamMessage.Decode(cr.Value);
                                //_queue.Enqueue(message);
                                try
                                {
                                    c.Commit(cr);
                                }
                                catch (KafkaException e)
                                {
                                    Debug.Log("Kafka - Error occured: " + e.Error.Reason);
                                }
                            }
                            catch (ConsumeException e)
                            {
                                Debug.Log("Consume - Error occured: " + e.Error.Reason);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.Log("Kafka - Canceled..");
                        // Ensure the consumer leaves the group cleanly and final offset are committed. 
                        c.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Kafka - Received Exception: " + ex.Message + " trace: " + ex.StackTrace);
            }
        }
    }

    public void StartKafka()
    {
        if  (kafkaStarted) return;

        _handle = new KafkaHandle();
        // kafkaThread = new Thread(_handle.StartKafkaListener);
        _handle.StartKafkaListener();

        // kafkaThread.Start();
        kafkaStarted = true;
        //StartKafkaListener(config);
    }

    private void ProcessKafkaMessage()
    {
        if (kafkaStarted)
        {
            // StreamMessage message;
            // while (_handle._queue.TryDequeue(out message))
            // {
            //     ProcessPBMessages.Process(message);
            // }
        }
    }

    void StopKafka()
    {
        if (kafkaStarted)
        {
            // kafkaThread.Abort();
            // kafkaThread.Join();
            kafkaStarted = false;
        }
    }
}
