using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ManagerSetTurnOff : MonoBehaviour
{
    public Text hour,minute,second;
    int hour_value, minute_value, second_value;
    public void light_set() {
        if (checkValue()) {
            // make light
        }
    }
    public void fan_set() {
        if (checkValue()) {
            // make fan
        }
    }
    public void heater_set() {
        if (checkValue()) {
            // make heater
        }
    }
    bool checkValue() {
        // hour
        hour_value = getValue(hour.text);
        if (hour_value > 24 || hour_value < 1 || hour_value == -1 ) return false;
        // minute
        minute_value = getValue(minute.text);
        if (minute_value > 60 || minute_value < -1) return false;
        else if (minute_value == -1) minute_value = 0;
        // second
        second_value = getValue(second.text);
        if (second_value > 60 || second_value < -1) return false;
        else if (second_value == -1) second_value = 0;
        
        return true;
    }
    int getValue(string input) {
        int result = 0;
        if (input.Length == 0) return -1;
        for (int i = 0; i < input.Length; i++) {            
            if (checkValue(input,i)) result = result * 10 + convert(input, i);
            else return 123456;
        }
        return result;
    }
    bool checkValue(string input, int i) {
        switch(input[i]) {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return true;
            default:
                return false;
        }
    }
    int convert(string input, int i) {
        switch (input[i]) {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
        }
        return 0;
    }
}
