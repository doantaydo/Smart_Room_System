using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateAccount : MonoBehaviour
{
    // Start is called before the first frame update
    public Button signUp;
    public List<InputField> input;
    public List<Text> warning;

    void Start()
    {
        signUp.onClick.AddListener(SignUp);
    }

    // Update is called once per frame

    void SignUp()
    {
        foreach (Text text in warning)
        {
            text.gameObject.SetActive(false);
        }

        string userName = input[0].text;
        string brokerID = input[1].text;
        string accessToken = input[2].text;
        string password = input[4].text;
        string repassword = input[5].text;
        string passwordToken = input[3].text;

        foreach (InputField field in input)
        {
            if (field.text == "")
            {
                warning[2].gameObject.SetActive(true);
                return;
            }
        }

        if (BinarySerializer.HasSaved(userName))
        {
            warning[0].gameObject.SetActive(true);
            return;
        }

        if (password != repassword)
        {
            warning[1].gameObject.SetActive(true);
            return;
        }

        Account account = new Account(userName, brokerID, accessToken, password, passwordToken);
        BinarySerializer.Save<Account>(account, userName);
        //Debug.Log(userName);
        SceneManager.LoadScene("Login");
    }
}

[System.Serializable]
public class Account
{
    public string userName;
    public string broker;
    public string accessToken;
    public string password;
    public string passwordToken;

    public Account()
    {

    }

    public Account(string userName, string brokerID, string accessToken, string password, string passwordToken)
    {
        this.userName = userName;
        this.broker = brokerID;
        this.accessToken = accessToken;
        this.password = password;
        this.passwordToken = passwordToken;
    }
}




