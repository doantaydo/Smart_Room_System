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
    // option setting
    float min_temp, max_temp, mid_temp;
    public bool isAuto;
    void Start() {
        if (instance == null) instance = this;
        isAuto = false;
        light_state = false;
        fan_state = false;
        heater_state = false;
        cur_temp = Random.Range(-99f, 99f);
        connect();
        getTemp();
        updateTemp();
    }
    void connect() {
        SystemLog.instance.EnQueue("Connecting to server!!!");
        // wait connect
        SystemLog.instance.EnQueue("Connected to server!!!");
    }
    void getTemp() {
        if (fan_state) cur_temp -= 0.5f;
        else if (heater_state) cur_temp += 0.5f;
    }
    void updateTemp() {
        temp_field.text = cur_temp.ToString("0.0");
    }
    void FixedUpdate()
    {
        getTemp();
        updateTemp();

        if (isAuto) {
            if (cur_temp < min_temp + (mid_temp - min_temp) / 2) {
                if (!heater_state) changeState(3);
                if (fan_state) changeState(2);
                isAuto = true;
            }
            else if (cur_temp > max_temp - (max_temp - mid_temp) / 2) {
                if (!fan_state) changeState(2);
                if (heater_state) changeState(3);
                isAuto = true;
            }
            else {
                if (fan_state) changeState(2);
                if (heater_state) changeState(3);
                isAuto = true;
            }
        }
    }
    public void LogOut() {
        SceneManager.LoadScene("Login");
    }
    // support instance
    public void changeState(int device) {
        switch (device) {
            case 1:
                light_state = !light_state;
                if (light_state) SystemLog.instance.EnQueue("Lights: ON");
                else SystemLog.instance.EnQueue("Lights: OFF");
                // update to server
                break;
            case 2:
                fan_state = !fan_state;
                if (fan_state) SystemLog.instance.EnQueue("Fans: ON");
                else SystemLog.instance.EnQueue("Fans: OFF");
                isAuto = false;
                // update to server
                break;
            case 3:
                heater_state = !heater_state;
                if (heater_state) SystemLog.instance.EnQueue("Heaters: ON");
                else SystemLog.instance.EnQueue("Heaters: OFF");
                isAuto = false;
                // update to server
                break;
        }
    }
    public void updateAutoMode(float min_temp, float max_temp) {
        if (!isAuto) {
            this.max_temp = max_temp;
            this.min_temp = min_temp;
            this.mid_temp = (min_temp + max_temp) / 2;
            SystemLog.instance.EnQueue("Auto Mode: ON");
            SystemLog.instance.EnQueue("Min Temperature: " + min_temp.ToString("0.0"));
            SystemLog.instance.EnQueue("Max Temperature: " + max_temp.ToString("0.0"));
        }
        else SystemLog.instance.EnQueue("Auto Mode: OFF");
        isAuto = !isAuto;
    }
}
