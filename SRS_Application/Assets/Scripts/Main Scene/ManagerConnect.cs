using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
public class ManagerConnect : MonoBehaviour
{
    private float cur_temp;
    public Text temp_field;
    protected MqttClient client;
    void Start()
    {
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
    public void LogOut() {
        SceneManager.LoadScene("Login");
    }
}
