using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var token = SessionManager.Instance.GetToken();
        Debug.Log($"HomeScene Started: {token}");
        ServerManager.Instance.EnqueueMessage(SessionManager.Instance.GetTokenJson());
    }
}
