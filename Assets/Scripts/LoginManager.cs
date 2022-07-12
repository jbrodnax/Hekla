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

    public void LoginClicked(){
        string username = UsernameInput.GetComponentInChildren<TMP_InputField>().text;
        string password = PasswordInput.GetComponentInChildren<TMP_InputField>().text;
        string ipaddress = AddressInput.GetComponentInChildren<TMP_InputField>().text;

        Debug.Log($"Login:\nUsername: {username}\nPassword: {password}\nIP Address: {ipaddress}");
        StartCoroutine(SendLoginRequest(username, password, ipaddress));
    }

    public void SignUpClicked(){
        string username = UsernameInput.GetComponentInChildren<TMP_InputField>().text;
        string password = PasswordInput.GetComponentInChildren<TMP_InputField>().text;
        string ipaddress = AddressInput.GetComponentInChildren<TMP_InputField>().text;

        Debug.Log($"SignUp:\nUsername: {username}\nPassword: {password}\nIP Address: {ipaddress}");
        StartCoroutine(SendSignUpRequest(username, password, ipaddress));
    }

    IEnumerator SendLoginRequest(string username, string password, string ipaddress){
        WWWForm form = new WWWForm();
        form.AddField("name", username);
        form.AddField("password", password);

        Debug.Log($"Sending Login request.");
        using (UnityWebRequest WWW = UnityWebRequest.Post($"http://{ipaddress}:5000/login", form)){
            yield return WWW.SendWebRequest();

            if (WWW.result != UnityWebRequest.Result.Success){
                Debug.Log(WWW.error);
            }else{
                Debug.Log(WWW.downloadHandler.text);
                session = SessionInfo.CreateFromJSON(WWW.downloadHandler.text);
                Debug.Log($"Token from JSON: {session.token}");
            }
        }
    }

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

}
[System.Serializable]
public class SessionInfo
{
    public bool success;
    public string token;

    public static SessionInfo CreateFromJSON(string jsonstring){
        return JsonUtility.FromJson<SessionInfo>(jsonstring);
    }
}
