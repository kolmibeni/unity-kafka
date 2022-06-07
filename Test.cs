using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//added for Kafka
using Confluent.Kafka;
using System.Threading;

public class OP2MoveDoor : MonoBehaviour
{
    //int x_axis = 0;
    float cur_x = 0;
    float cur_y = 0;
    float cur_z = 0;
    //float speed = 50f;
    int position_way = 0; //0不動，1開門，2關門
    float movingSpeed = 500f;

    //Parameters for Kafka thread
    bool kafkaStarted = false;
    Thread kafkaThread;
    threadHandle _handle;


    // Start is called before the first frame update
    void Start()
    {
        cur_x = this.transform.position.x;
        cur_y = this.transform.position.y;
        cur_z = this.transform.position.z;
        position_way = 1;

        //Start the Kafka thread
        StartKafkaThread();
    }

    // Update is called once per frame
    void Update()
    {
        if(position_way == 1){
            if(this.transform.localPosition.x < 1000){
                this.transform.localPosition  = this.transform.localPosition  + new Vector3(1*movingSpeed, 0*movingSpeed, 0*movingSpeed) * Time.deltaTime;
            }
            else{
                position_way = 2;
            }
        } else if(position_way == 2){
            if(this.transform.localPosition.x > 0){
                this.transform.localPosition  = this.transform.localPosition  + new Vector3(-1*movingSpeed, 0*movingSpeed, 0*movingSpeed) * Time.deltaTime;
            }
            else{
                position_way = 1;
            }
        }

        GameObject.Find("ButtonOpenDoor").GetComponentInChildren<Text>().text = this.transform.localPosition.x.ToString();

       /* cur_x = this.transform.position.x;
        cur_y = this.transform.position.y;
        cur_z = this.transform.position.z;
        if(position_way == 1){ //如果是開門
            while(cur_x <= 1000){
                transform.position = new Vector3(100,0,0) * Time.deltaTime;
            }
            position_way = 2;
        }else if(position_way == 2){ //如果是開門
            while(cur_x >= 0){
                transform.position = new Vector3(-100,0,0) * Time.deltaTime;
            }
            position_way = 1;
        }*/
        
        //transform
        //tra

        //Check if Kafka is stopped using Ctrl+C
        if (Input.GetKeyUp(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.C))
        {
            Debug.Log("Cancelling Kafka!");
            StopKafkaThread();
        }
        //Process incoming messages from Kafka stream
        ProcessKafkaMessage();
    }


    //class for handling Kafka thread
    public class threadHandle
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
                    GroupID = "",
                    BootstrapServers = "",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                Debug.Log("Kafka - Created config");

                using (var c = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    c.Subscribe("networktopicdata");
                    Debug.Log("Kafka - Subscribed");

                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) => {
                        e.Cancel = true; //prevent the process from terminating
                        cts.Cancel();
                    };

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                //Waiting for message
                                var cr = c.Consume(cts.Token);
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

    public void StartKafkaThread()
    {
        if  (kafkaStarted) return;

        _handle = new threadHandle();
        kafkaThread = new Thread(_handle.StartKafkaListener);

        kafkaThread.Start();
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

    void StopKafkaThread()
    {
        if (kafkaStarted)
        {
            kafkaThread.Abort();
            kafkaThread.Join();
            kafkaStarted = false;
        }
    }
}
