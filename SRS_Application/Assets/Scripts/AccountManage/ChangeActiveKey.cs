using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeActiveKey : MonoBehaviour
{
    public Button save;

    public InputField userName;
    public InputField password;

    public GameObject warningText;

    public static Account user = new Account();

    //bool login = false;
    public InputField activeKey;

    void Start()
    {
        save.onClick.AddListener(Save);
    }

    // Update is called once per frame
    void Save()
    {
        string username = userName.text;
        string pwd = password.text;

        if (!BinarySerializer.HasSaved(username))
        {
            warningText.gameObject.SetActive(true);
            return;
        }
        else user = BinarySerializer.Load<Account>(username);

        if (pwd != user.password)
        {
            warningText.gameObject.SetActive(true);
            user = null;
            return;
        }

        user.passwordToken = activeKey.text;
        BinarySerializer.Save<Account>(user, username);

        //login = true;

        /*
        PlayerPrefs.SetString("cur_broker_uri", user.broker);
        PlayerPrefs.SetString("cur_access_token", user.accessToken);
        PlayerPrefs.SetString("cur_pwd_access_token", user.passwordToken);
        */
        //SceneManager.LoadScene("main_system");
    }
}
