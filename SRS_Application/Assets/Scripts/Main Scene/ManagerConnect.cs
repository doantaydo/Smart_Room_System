using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
public class ManagerConnect : MonoBehaviour
{
    private string broker_URI, access_token, pwd_access_token;
    private float cur_temp;
    public Text temp_field;
    protected MqttClient client;
    void Start()
    {
        broker_URI = PlayerPrefs.GetString("cur_broker_uri","io.adafruit.com");
        access_token = PlayerPrefs.GetString("cur_access_token","HanhHuynh");
        pwd_access_token = PlayerPrefs.GetString("cur_pwd_access_token","aio_aCYQ88qV5IkGvLn9IEtCNvReJkqX");

        connect();
        getTemp();
        updateTemp();
    }
    void connect() {
        Debug.Log("Connect to Server!!");
    }
    void getTemp() {
        Debug.Log("Random Temp");
        cur_temp = (float)((int)(Random.Range(-99f,99f)*10)) / 10;
    }
    void updateTemp() {
        temp_field.text = cur_temp.ToString();
    }

    void Update()
    {
        
    }
}
