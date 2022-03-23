using UnityEngine;
using UnityEngine.UI;

public class ManagerAutoMode : MonoBehaviour
{
    public Text minTemp, maxTemp;
    float min_temp, max_temp;
    public void clicked() {
        if (checkValue())
        ManagerConnect.instance.updateAutoMode(min_temp, max_temp);
        Debug.Log(min_temp.ToString());
        Debug.Log(max_temp.ToString());
    }
    bool checkValue() {
        string min_temp_value = minTemp.text;
        string max_temp_value = maxTemp.text;

        min_temp = getValue(min_temp_value);
        if (min_temp == 10000000) return false;

        max_temp = getValue(max_temp_value);
        if (max_temp == 10000000) return false;
        
        return true;
    }
    float getValue(string input) {
        float result = 0;
        if (input.Length == 0) return 10000000;
        for (int i = 0; i < input.Length; i++) {
            if (input[i] == '.') {
                float value = getTail(input, i + 1);
                if (value == -1) return 10000000;
                result += value;
                return result;
            }
            
            if (checkValue(input,i)) result = result * 10 + convert(input, i);
            else return 10000000;
        }
        return result;
    }
    float getTail(string input, int i) {
        float result = 0;
        int count = 0;
        for (; i < input.Length; i++) {
            if (checkValue(input,i)) {
                if (count == 0) result += convert(input, i) / 10;
                else if (count == 1) result += convert(input, i) /100;
                count++;
            }
            else return -1;
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
    float convert(string input, int i) {
        switch (input[i])
        {
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
