using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance;
    [SerializeField] public GameObject UsernameInput, PasswordInput, AddressInput;
    public SessionInfo session;

    void Awake(){
        Instance = this;
    }

    /// <summary>
    /// Called when the Login button is clicked from the Login scene
    /// </summary>
    public void LoginClicked(){
        string username = UsernameInput.GetComponentInChildren<TMP_InputField>().text;
        string password = PasswordInput.GetComponentInChildren<TMP_InputField>().text;
        string ipaddress = AddressInput.GetComponentInChildren<TMP_InputField>().text;

        Debug.Log($"Login:\nUsername: {username}\nPassword: {password}\nIP Address: {ipaddress}");
        StartCoroutine(SendLoginRequest(username, password, ipaddress));
    }

    /// <summary>
    /// Called when the Signup button is clicked from the Login scene
    /// </summary>
    public void SignUpClicked(){
        string username = UsernameInput.GetComponentInChildren<TMP_InputField>().text;
        string password = PasswordInput.GetComponentInChildren<TMP_InputField>().text;
        string ipaddress = AddressInput.GetComponentInChildren<TMP_InputField>().text;

        Debug.Log($"SignUp:\nUsername: {username}\nPassword: {password}\nIP Address: {ipaddress}");
        StartCoroutine(SendSignUpRequest(username, password, ipaddress));
    }

    /// <summary>
    /// Send a login request to the server.
    /// </summary>
    /// <param name="username">Account username</param>
    /// <param name="password">Account password</param>
    /// <param name="ipaddress">IP address of the server</param>
    /// <returns></returns>
    IEnumerator SendLoginRequest(string username, string password, string ipaddress){
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("password", password);

        Debug.Log($"Sending Login request.");
        using (UnityWebRequest WWW = UnityWebRequest.Post($"http://{ipaddress}:5000/login", form)){
            yield return WWW.SendWebRequest();

            if (WWW.result != UnityWebRequest.Result.Success){
                Debug.Log(WWW.error);
                // TKTK - Display error message to UI.
            }else{
                Debug.Log(WWW.downloadHandler.text);
                if (SessionManager.Instance.SaveSessionInfo(WWW.downloadHandler.text) == true){
                    SceneLoaderHandoff(ipaddress);
                }
            }
        }
    }

    /// <summary>
    /// Send a signup request to the server.
    /// </summary>
    /// <param name="username">Username credential for new account</param>
    /// <param name="password">Password credential for new account</param>
    /// <param name="ipaddress">IP address of the server</param>
    /// <returns></returns>
    IEnumerator SendSignUpRequest(string username, string password, string ipaddress){
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("password", password);

        Debug.Log($"Sending SignUp request.");
        using (UnityWebRequest WWW = UnityWebRequest.Post($"http://{ipaddress}:5000/signup", form)){
            yield return WWW.SendWebRequest();

            if (WWW.result != UnityWebRequest.Result.Success){
                Debug.Log(WWW.error);
            }else{
                Debug.Log(WWW.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Pass the server address to Session Manager and load the next scene.
    /// </summary>
    /// <param name="session">SessionInfo class populated with auth token.</param>
    /// <param name="address">Server IP address</param>
    public void SceneLoaderHandoff(string address){
        SessionManager.Instance.ServerAddress = address;
        SceneLoader.Instance.Load("Home");
    }
}
