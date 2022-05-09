using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangePassword : MonoBehaviour
{
    public Button save;
    public Button returnLogin;

    public InputField userName;
    public InputField password;

    public Text warningText;

    public static Account user = new Account();

    //bool login = false;
    public InputField nPassword;
    public InputField rnPassword;

    string username;

    void Start()
    {
        save.onClick.AddListener(Save);
        returnLogin.onClick.AddListener(ReturnMain);
    }

    // Update is called once per frame
    void Save()
    {
        string username = userName.text;
        string pwd = password.text;

        foreach (InputField field in new List<InputField>() {password, nPassword, rnPassword})
        {
            if (field.text == "")
            {
                warningText.text = "Please fill in all your information";
                warningText.gameObject.SetActive(true);
                return;
            }
        }

        if (pwd != user.password)
        {
            warningText.gameObject.SetActive(true);
            return;
        }

        if (nPassword.text != rnPassword.text)
        {
            warningText.text = "Password doesn't match";
            warningText.gameObject.SetActive(true);
            return;
        }

        user.password = nPassword.text;

        BinarySerializer.Save<Account>(user, username);

        //login = true;
        SceneManager.LoadScene("main_system");
    }

    void ReturnMain()
    {
        SceneManager.LoadScene("Login");
    }
}
