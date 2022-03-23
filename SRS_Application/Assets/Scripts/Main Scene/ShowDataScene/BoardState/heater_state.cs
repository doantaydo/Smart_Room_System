using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heater_state : device_state
{
    void FixedUpdate() {
        if (ManagerConnect.instance.heater_state != this.state) {
            if (this.state) {
                //turn on --> off
                green_ren.color = color_dark;
            }
            else {
                // turn off
                red_ren.color = color_dark;
            }
            this.state = !this.state;
        }
        base.FixedUpdate();
    }
}
