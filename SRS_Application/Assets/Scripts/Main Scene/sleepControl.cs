using UnityEngine;
using UnityEngine.UI;

public class sleepControl : MonoBehaviour
{
    public Text field;
    public GameObject mainCanvas, isSleeping;
    public static sleepControl instance;
    int time;
    void Awake() {
        if (instance == null) instance = this;
    }
    public void StartRunning()
    {
        time = 10;
        field.text = time.ToString();
    }

    // Update is called once per frame
    int count = 0;
    void FixedUpdate()
    {
        count++;
        if (count == 10) {
            time--;
            field.text = time.ToString();
            count = 0;
        }

        if (time == 0 && count == 5) turnOff();
    }
    void turnOff() {
        mainCanvas.SetActive(true);
        isSleeping.SetActive(false);
        ManagerConnect.instance.changeState(1);
    }
    public void stopCount() {
        time = -1;
    }
}
