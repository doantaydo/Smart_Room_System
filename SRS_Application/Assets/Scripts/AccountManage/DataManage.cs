using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;

public class DataManagement
{
    static string persistentDataPath = Application.persistentDataPath;
    static string folderName = "Data";

    public void SaveTime()
    {
        string fileName = PlayerPrefs.GetString("cur_user") + ".json";

        UserData newData = new UserData
        {
            date = System.DateTime.Now.ToString("dd/MM/yyyy"),
            time_turn_off_light = System.DateTime.Now.ToString("hh:mm"),
        };
        

        //FileStream file;
        if (!File.Exists(GetFilePath(fileName)))
        {
            File.Create(GetFilePath(fileName));
            DataSet newDataSet = new DataSet
            {
                data = new List<UserData>() { newData },
            };
            string json = JsonUtility.ToJson(newDataSet);
            var jsonObj = JObject.Parse(json);
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                   Newtonsoft.Json.Formatting.Indented);

            //string row = Js
            File.WriteAllText(GetFilePath(fileName), newJsonResult);
        }
        else
        {
            string row = JsonUtility.ToJson(newData);

            var json = File.ReadAllText(GetFilePath(fileName));
            var jsonObj = JObject.Parse(json);
            var data = jsonObj.GetValue("data") as JArray;
            var item = JObject.Parse(row);

            data.Add(item);
            if (data.Count > 30)
            {
                data.RemoveAt(0);
            }

            jsonObj["data"] = data;
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                   Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(GetFilePath(fileName), newJsonResult);
        }

        //file.Close();
    }

    public List<UserData> GetData()
    {
        string fileName = PlayerPrefs.GetString("cur_user") + ".json";
        List<UserData> dataSet = new List<UserData>();

        var json = File.ReadAllText(GetFilePath(fileName));
        var jObject = JObject.Parse(json);

        if (jObject != null)
        {
            JArray data = (JArray)jObject["data"];
            if (data != null)
            {
                foreach (var row in data)
                {
                    UserData item = new UserData
                    {
                        date = row["date"].ToString(),
                        time_turn_off_light = row["time_turn_off_light"].ToString(),
                    };
                    dataSet.Add(item);
                }
            }
        }

        return dataSet;
    }
    /*
    public int setEndDay(string time)
    {

    }
    */

    static string GetFilePath(string fileName)
    {
        return persistentDataPath + "/" + folderName + "/" + fileName;
    }

    public class UserData
    {
        public string date { get; set; }
        public string time_turn_off_light { get; set; }
    }

    public class DataSet
    {
        public List<UserData> data;
    }
}
