using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerButtonScene : MonoBehaviour
{
    public void changeLight() {
        ManagerConnect.instance.changeState(1);
    }
    public void changeFan() {
        ManagerConnect.instance.changeState(2);
    }
    public void changeHeater() {
        ManagerConnect.instance.changeState(3);
    }
}
