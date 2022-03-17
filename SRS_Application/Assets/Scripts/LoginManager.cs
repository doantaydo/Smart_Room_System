using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button signUp;
    public Button logIn;

    public InputField userName;
    public InputField password;

    public GameObject warningText;

    public static Account user = new Account();

    void Start()
    {
        signUp.onClick.AddListener(SignUp);
        logIn.onClick.AddListener(LogIn);
    }

    // Update is called once per frame

    void SignUp()
    {
        SceneManager.LoadScene("Signup");
    }

    void LogIn()
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
            return;
        }

        SceneManager.LoadScene("Demo");
    }
}
