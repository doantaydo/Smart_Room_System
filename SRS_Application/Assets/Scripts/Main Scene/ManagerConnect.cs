using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
public class ManagerConnect : MonoBehaviour
{
    public static ManagerConnect instance;
    // DATA OF USER
    private float cur_temp;
    public Text temp_field;
    public bool light_state, fan_state, heater_state;
    void Start()
    {
        if (instance == null) instance = this;
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
        light_state = false;
        fan_state = false;
        heater_state = false;
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
    public void changeState(int device) {
        switch (device) {
            case 1:
                light_state = !light_state;
                Debug.Log("1");
                // update to server
                break;
            case 2:
                fan_state = !fan_state;
                Debug.Log("2");
                // update to server
                break;
            case 3:
                heater_state = !heater_state;
                Debug.Log("3");
                // update to server
                break;
        }
    }
}
