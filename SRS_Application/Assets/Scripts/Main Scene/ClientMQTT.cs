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
        public string Topic = "";
        public string Machine_Id;
        //public string Topic_to_Subcribe = "";
        public string msg_received_from_topic = "";
        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;
        private string topic_temp = "", topic_light = "", topic_fan = "", topic_heater = "";
        protected override void Awake() {
            if (instance == null) instance = this;
            // brokerAddress = PlayerPrefs.GetString("cur_broker_uri", "io.adafruit.com");
            // mqttUserName = PlayerPrefs.GetString("cur_access_token", "doantaydo");
            // mqttPassword = PlayerPrefs.GetString("cur_pwd_access_token", "aio_sKoR22KKkwLAMPdW05LYf7hAyHOG");
            brokerPort = 1883;

            brokerAddress = "io.adafruit.com";
            mqttUserName = "HanhHuynh";
            mqttPassword = "aio_oUMU18kutAlwzxs5ehkzvbphSCkN";


            topic_temp = mqttUserName + "/feeds/microbit-temp";
            topic_light = mqttUserName + "/feeds/microbit-led";
            topic_fan = mqttUserName + "/feeds/microbit-fan";
            // topic_heater = mqttUserName + "/feeds/microbit-heater";
            autoConnect = true;
            base.Awake();
        }

        public void TestPublish() {
            //client.Publish(Topic_to_Subcribe, System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
            AddUiMessage("Test message published.");
        }

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        //public void SetUiMessage(string msg)
        //{
        //    if (consoleInputField != null)
        //    {
        //        consoleInputField.text = msg;
        //        updateUI = true;
        //    }
        //}

        public void AddUiMessage(string msg)
        {
            //if (consoleInputField != null)
            //{
            //    consoleInputField.text += msg + "\n";
            //    updateUI = true;
            //}
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            //SetUiMessage("Connected to broker on " + brokerAddress + "\n");

            //if (autoTest)
            //{
            //    TestPublish();
            //}
            SubscribeTopics();
        }
        bool isPub = false;
        protected override void SubscribeTopics()
        {
            if (!isPub) {
                Debug.Log("Pub");
                client.Publish(topic_temp, System.Text.Encoding.UTF8.GetBytes("-10000"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                // publishFan(false);
                //publishLight(false);
                // publishHeater(false);
                isPub = true;
            }
            
            // if (Topic_to_Subcribe != "")
            // { 
            //     client.Subscribe(new string[] { Topic_to_Subcribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            // }
            if (topic_temp != "") {
                client.Subscribe(new string[] { topic_temp }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
            if (topic_fan != "") {
                client.Subscribe(new string[] { topic_fan }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
            if (topic_heater != "") {
                client.Subscribe(new string[] { topic_heater }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
            if (topic_light != "") {
                client.Subscribe(new string[] { topic_light }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
        }

        protected override void UnsubscribeTopics()
        {
            //client.Unsubscribe(new string[] { Topic_to_Subcribe });
            client.Unsubscribe(new string[] { topic_temp });
            client.Unsubscribe(new string[] { topic_fan });
            client.Unsubscribe(new string[] { topic_heater });
            client.Unsubscribe(new string[] { topic_light });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            AddUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            AddUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            AddUiMessage("CONNECTION LOST!");
        }

        private void UpdateUI()
        {
            //if (client == null)
            //{
            //    if (connectButton != null)
            //    {
            //        connectButton.interactable = true;
            //        disconnectButton.interactable = false;
            //        testPublishButton.interactable = false;
            //    }
            //}
            //else
            //{
            //    if (testPublishButton != null)
            //    {
            //        testPublishButton.interactable = client.IsConnected;
            //    }
            //    if (disconnectButton != null)
            //    {
            //        disconnectButton.interactable = client.IsConnected;
            //    }
            //    if (connectButton != null)
            //    {
            //        connectButton.interactable = !client.IsConnected;
            //    }
            //}
            //if (addressInputField != null && connectButton != null)
            //{
            //    addressInputField.interactable = connectButton.interactable;
            //    addressInputField.text = brokerAddress;
            //}
            //if (portInputField != null && connectButton != null)
            //{
            //    portInputField.interactable = connectButton.interactable;
            //    portInputField.text = brokerPort.ToString();
            //}
            //if (encryptedToggle != null && connectButton != null)
            //{
            //    encryptedToggle.interactable = connectButton.interactable;
            //    encryptedToggle.isOn = isEncrypted;
            //}
            //if (clearButton != null && connectButton != null)
            //{
            //    clearButton.interactable = connectButton.interactable;
            //}
            //updateUI = false;
        }

        protected override void Start()
        {
            //SetUiMessage("Ready.");
            // brokerAddress = PlayerPrefs.GetString("cur_broker_uri", "io.adafruit.com");
            // mqttUserName = PlayerPrefs.GetString("cur_access_token", "doantaydo");
            // mqttPassword = PlayerPrefs.GetString("cur_pwd_access_token", "aio_Bqas77KIlnXA1wJAKhqmE4DY4ufh");

            // Topic = "doantaydo/feeds/temp-device";
            // Topic_to_Subcribe = "doantaydo/feeds/temp-device";
            //Topic_to_Subcribe = Topic + Machine_Id;
            SystemLog.instance.EnQueue("Connecting to server!!!");
            updateUI = true;
            base.Start();
            SystemLog.instance.EnQueue("Connected to server!!!");
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            msg_received_from_topic = msg;
            //text_display.text = msg;
            Debug.Log("Received: " + msg);
            StoreMessage(msg);
            if (topic == topic_light)
                ManagerConnect.instance.light_state = (msg == "1");
            else if (topic == topic_fan)
                ManagerConnect.instance.fan_state = (msg == "3");
            else if (topic == topic_temp) {
                ManagerConnect.instance.cur_temp = float.Parse(msg);
            }
            // else {
            //     if (msg == "1") {
            //         if (topic == topic_light) ManagerConnect.instance.light_state = true;
            //         if (topic == topic_fan) ManagerConnect.instance.fan_state = true;
            //         if (topic == topic_heater) ManagerConnect.instance.heater_state = true;
            //     }
            //     else {
            //         if (topic == topic_light) ManagerConnect.instance.light_state = false;
            //         if (topic == topic_fan) ManagerConnect.instance.fan_state = false;
            //         if (topic == topic_heater) ManagerConnect.instance.heater_state = false;
            //     }
            // }
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            AddUiMessage("Received: " + msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            if (updateUI)
            {
                UpdateUI();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            //if (autoTest)
            //{
            //    autoConnect = true;
            //}
        }
        public void publishLight(bool type) {
            if (topic_light == "") return;
            if (type) client.Publish(topic_light, System.Text.Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            else client.Publish(topic_light, System.Text.Encoding.UTF8.GetBytes("0"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishFan(bool type) {
            if (topic_fan == "") return;
            if (type) client.Publish(topic_fan, System.Text.Encoding.UTF8.GetBytes("3"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            else client.Publish(topic_fan, System.Text.Encoding.UTF8.GetBytes("2"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishHeater(bool type) {
            if (topic_heater == "") return;
            if (type) client.Publish(topic_heater, System.Text.Encoding.UTF8.GetBytes("true"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            else client.Publish(topic_heater, System.Text.Encoding.UTF8.GetBytes("false"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}


