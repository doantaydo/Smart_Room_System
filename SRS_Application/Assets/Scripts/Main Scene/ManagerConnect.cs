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
        cur_temp = (float)((int)(Random.Range(-99f, 99f) * 10)) / 10;
        connect();
        getTemp();
        updateTemp();
    }
    void connect() {
        Debug.Log("Connect to Server!!");
    }
    void getTemp() {
        if (fan_state) cur_temp--;
        else if (heater_state) cur_temp++;
    }
    void updateTemp() {
        temp_field.text = cur_temp.ToString();
    }
    void FixedUpdate()
    {
        getTemp();
        updateTemp();

        if (isAuto) {
            if ((cur_temp < min_temp + (mid_temp - min_temp) / 2) && (!heater_state)) {
                changeState(3);
                if (fan_state) changeState(2);
                isAuto = true;
            }
            else if ((cur_temp > max_temp - (max_temp - mid_temp) / 2) && (!fan_state)) {
                changeState(2);
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
                // update to server
                break;
            case 2:
                fan_state = !fan_state;
                isAuto = false;
                // update to server
                break;
            case 3:
                heater_state = !heater_state;
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
        }
        isAuto = !isAuto;
    }
}
