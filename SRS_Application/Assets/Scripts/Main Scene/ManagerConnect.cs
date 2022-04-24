using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
public class ManagerConnect : MonoBehaviour
{
    public static ManagerConnect instance;
    public float LIGHT_POINT, GAS_POINT;
    // DATA OF USER
    public float cur_temp, cur_light, cur_gas;
    public Text temp_field;
    public bool light_state, fan_state, heater_state;
    public bool isAuto, isAutoLight;
    public GameObject canvasMain, isSleep, warningGas;
    public int hourSleep = 0, minuteSleep = 0;
    // option setting
    float min_temp, max_temp, mid_temp;
    // GAS WARNING TIME
    int warn_hour, warn_minute, warn_day, warn_month, warn_year;
    void Awake() {
        if (instance == null) instance = this;
    }
    void Start() {
        isAuto = false;
        isAutoLight = false;
        light_state = false;
        fan_state = false;
        heater_state = false;
        cur_temp = -10000;
        temp_field.text = "Wait";
    }
    void updateTemp() {
        if (cur_temp != -10000) temp_field.text = ((int)cur_temp).ToString();
    }
    void FixedUpdate()
    {
        updateTemp();
        // auto mod
        // auto turn on/off FAN follow the temperature
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

        // auto turn on/off light follow the light sensitive
        if (isAutoLight) {
            if ((cur_light < LIGHT_POINT) && (light_state == false)) changeState(1);
            if ((cur_light >= LIGHT_POINT) && (light_state == true)) changeState(1);
        }
        // warning if the gas is out of
        if (cur_gas >= GAS_POINT) warningGasIsOut();
        // warning if the user is sleeping

        sleepControl.instance.StartRunning();
        canvasMain.SetActive(false);
        isSleep.SetActive(true);

        // int cur_h = GetTime.getHour(), cur_m = GetTime.getMinute();
        // if ((cur_h >= 20 && cur_h <= 4) || (cur_h == 5 && cur_m == 0) && light_state == true) {
        //     if ((cur_h > hourSleep) || (cur_h == hourSleep && cur_m >= minuteSleep)) {
        //         sleepControl.instance.StartRunning();
        //         canvasMain.SetActive(false);
        //         isSleep.SetActive(true);
        //     }
        // }
        // update sleeping time
        updateSleepTime();
    }
    bool hadUpdatedToDay = false;
    void updateSleepTime() {
        int cur_h = GetTime.getHour();
        if (cur_h >= 6 && cur_h < 20) {
            if (hadUpdatedToDay == false) {
                //call method update time
                hadUpdatedToDay = true;
            }
        }
        else hadUpdatedToDay = false;

    }
    public void notSleep() {
        sleepControl.instance.stopCount();
        canvasMain.SetActive(true);
        isSleep.SetActive(false);
        if (minuteSleep < 30) minuteSleep += 30;
        else {
            hourSleep += 1;
            minuteSleep -= 30;
            if (hourSleep == 24) hourSleep = 0;
        }
    }
    void warningGasIsOut() {
        int cur_warn_hour = GetTime.getHour();
        int cur_warn_minute = GetTime.getMinute();
        int cur_warn_day = GetTime.getDay();
        int cur_warn_month = GetTime.getMonth();
        int cur_warn_year = GetTime.getYear();
        if ((cur_warn_year == warn_year) && (cur_warn_month == warn_month) && (cur_warn_day == warn_day)) {
            if ((warn_minute > 50) && (cur_warn_hour - warn_hour < 2) && (cur_warn_minute + 60 - warn_minute < 10)) return;
            else if ((cur_warn_hour == warn_hour) && (cur_warn_minute - warn_minute < 10)) return;
        }
        warn_hour   = cur_warn_hour;
        warn_minute = cur_warn_minute;
        warn_day    = cur_warn_day;
        warn_month  = cur_warn_month;
        warn_year   = cur_warn_year;
        canvasMain.SetActive(false);
        warningGas.SetActive(true);
    }
    public void LogOut() {
        M2MqttUnity.Examples.ClientMQTT.instance.OnDestroy();
        SceneManager.LoadScene("Login");
    }
    public void changeState(int device) {
        bool previous;
        switch (device) {
            case 1:
                M2MqttUnity.Examples.ClientMQTT.instance.publishLed(!light_state);
                if (light_state) SystemLog.instance.EnQueue("Lights: ON");
                else {
                    SystemLog.instance.EnQueue("Lights: OFF");
                    int cur_h = GetTime.getHour();
                    int cur_m = GetTime.getMinute();
                    if (cur_h >= 20 && cur_h <= 4) DataManage.instance.SaveTime(); // from 20h00 to 4h59
                    else if (cur_h == 5 && cur_m == 0) DataManage.instance.SaveTime(); // at 5h
                }
                break;
            case 2:
                M2MqttUnity.Examples.ClientMQTT.instance.publishFan(!fan_state);
                if (fan_state) SystemLog.instance.EnQueue("Fans: ON");
                else SystemLog.instance.EnQueue("Fans: OFF");
                isAuto = false;
                break;
            case 3:
                M2MqttUnity.Examples.ClientMQTT.instance.publishHeater(!heater_state);
                if (heater_state) SystemLog.instance.EnQueue("Heaters: ON");
                else SystemLog.instance.EnQueue("Heaters: OFF");
                isAuto = false;
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
