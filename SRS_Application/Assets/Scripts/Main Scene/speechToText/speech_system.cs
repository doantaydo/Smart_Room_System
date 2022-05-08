using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceRecognition {
    public class speech_system : MonoBehaviour {
        SpeechRecognitionEngine recognizer;

        Choices cmdlist, numbers, customChoice;
        GrammarBuilder customTime, builder;
        void SetUp() {
            recognizer = new SpeechRecognitionEngine();

            cmdlist = new Choices("turn on the light", "turn off the light", "turn on the fan", "turn off the fan", 
            "open the door", "close the door", "turn on auto mode", "turn off auto mode", "turn on auto light mode", 
            "turn off auto light mode", "quit", "exit", "log out", "yes i'm here");
            numbers = new Choices(); for (int i = 0; i < 10; i++) numbers.Add(new SemanticResultValue(i.ToString(), i));

            customChoice = new Choices("light", "fan");
            customTime = new GrammarBuilder("turn off the ");
            customTime.Append(customChoice);
            customTime.Append("at");
            customTime.Append(new SemanticResultKey("h1", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("h2", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("m1", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("m2", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("s1", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("s2", numbers));
            cmdlist.Add(customTime);

            builder = new GrammarBuilder(cmdlist);
            recognizer.LoadGrammar(new Grammar(builder));
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechHandler);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            recognizer.EndSilenceTimeout = TimeSpan.FromSeconds(2);
            recognizer.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(2.5);            
        }
        bool isSetUp = false;
        // Update is called once per frame
        void Update() {
            if (!isSetUp) {
                SetUp();
                isSetUp = true;
            }
            Console.ReadLine();
        }
        void SpeechHandler(object sender, SpeechRecognizedEventArgs e) {
            Debug.Log(e.Result.Text);        
            string str = "non";
            if (e.Result.Text == "turn on the light") {
                callFunction.turnDevice(0, true);
                str = "Turn on the light";
            }
            else if (e.Result.Text == "turn off the light") {
                callFunction.turnDevice(0, false);
                str = "Turn off the light";
            }
            else if (e.Result.Text == "turn on the fan") {
                callFunction.turnDevice(1, true);
                str = "Turn on the fan";
            }
            else if (e.Result.Text == "turn off the fan") {
                callFunction.turnDevice(1, false);
                str = "Turn off the fan";
            }
            else if (e.Result.Text == "open the door") {
                callFunction.turnDevice(2, true);
                str = "Open the door";
            }
            else if (e.Result.Text == "close the door") {
                callFunction.turnDevice(2, false);
                str = "Close the door";
            }
            else if (e.Result.Text == "turn on auto mode") {
                callFunction.turnAutoMode(true);
                str = "Turn on auto mode";
            }
            else if (e.Result.Text == "turn off auto mode") {
                callFunction.turnAutoMode(false);
                str = "Turn off auto mode";
            }
            else if (e.Result.Text == "turn on auto light mode") {
                callFunction.turnAutoLightMode(true);
                str = "Turn on auto light mode";
            }
            else if (e.Result.Text == "turn off auto light mode") {
                callFunction.turnAutoLightMode(false);
                str = "Turn off auto light mode";
            }
            else if (e.Result.Text == "quit" || e.Result.Text == "exit") {
                str = "Exit";
                callFunction.quitSystem();
            }
            else if (e.Result.Text == "log out") {
                str = "Log out";
                callFunction.logOut();
            }
            else if (e.Result.Text == "yes i'm here") {
                str = "Yes i'm here";
                callFunction.iHere();
            }
            else {
                hh = (int)e.Result.Semantics["h1"].Value * 10 + (int)e.Result.Semantics["h2"].Value;
                mm = (int)e.Result.Semantics["m1"].Value * 10 + (int)e.Result.Semantics["m2"].Value;
                ss = (int)e.Result.Semantics["s1"].Value * 10 + (int)e.Result.Semantics["s2"].Value;
                if (e.Result.Text.Contains("light")) callFunction.turnOffDeviceAt(0, hh, mm, ss);
                else callFunction.turnOffDeviceAt(1, hh, mm, ss);
            }
            SystemLog.instance.EnQueue(str);
        }
    }
}
