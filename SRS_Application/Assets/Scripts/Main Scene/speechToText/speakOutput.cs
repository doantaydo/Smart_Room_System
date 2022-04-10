using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using SampleSynthesis {
    public class speakOutput : MonoBehaviour {
    // Start is called before the first frame update
        public static speakOutput instance;
        void Awake() {
            if (instance == null) instance = this;
        }
        void Start()
        {

        }

        void speak(msg) {
            //speak
        }
        // speakOutput.instance.speak("...")

        // public void speak(string msg) {
        //     //synthesizer
        // }
    }
//}


