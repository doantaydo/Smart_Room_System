using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Examples {
    public class ClientMQTT : M2MqttUnityClient {
        public static ClientMQTT instance;
        private string Topic_to_Subcribe = "doantaydo/feeds/temp-device";
        public string msg_received_from_topic = "";
        public Text text_display;
        private List<string> eventMessages = new List<string>();
        protected override void Start() {
            if (instance == null) instance = this;
            brokerAddress = PlayerPrefs.GetString("cur_broker_uri", "io.adafruit.com");
            mqttUserName = PlayerPrefs.GetString("cur_access_token", "doantaydo");
            mqttPassword = PlayerPrefs.GetString("cur_pwd_access_token", "aio_Bqas77KIlnXA1wJAKhqmE4DY4ufh");
            Debug.Log("Start connect!!");
            Connect();
            Debug.Log("Connected!!");
        }

        public void TestPublish() {
            client.Publish(Topic_to_Subcribe, System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            Debug.Log("Test message published.");
        }
        public void SetEncrypted(bool isEncrypted) {
            this.isEncrypted = isEncrypted;
        }
        protected override void OnConnecting() {
            base.OnConnecting();
        }

        protected override void OnConnected() {
            base.OnConnected();
            //SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            //if (autoTest)
            //{
            //    TestPublish();
            //}
            SubscribeTopics();
        }
        protected override void SubscribeTopics() {
            if (Topic_to_Subcribe != "") {
                client.Subscribe(new string[] { Topic_to_Subcribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
        }

        protected override void UnsubscribeTopics() {
            client.Unsubscribe(new string[] { Topic_to_Subcribe });
        }

        protected override void OnConnectionFailed(string errorMessage) {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected() {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost() {
            Debug.Log("CONNECTION LOST!");
        }

        protected override void DecodeMessage(string topic, byte[] message) {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            msg_received_from_topic = msg;
            text_display.text = msg;
            Debug.Log("Received: " + msg);
            StoreMessage(msg);
            if (topic == Topic_to_Subcribe)
            {
                //if (autoTest)
                //{
                //    autoTest = false;
                //    Disconnect();
                //}
            }
        }

        private void StoreMessage(string eventMsg) {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg) {
            Debug.Log("Received: " + msg);
        }

        protected override void Update() {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0) {
                foreach (string msg in eventMessages) {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            // if (updateUI) {
            //     UpdateUI();
            // }
        }

        private void OnDestroy() {
            Disconnect();
        }
    }
}


