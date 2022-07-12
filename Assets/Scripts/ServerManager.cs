using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using NativeWebSocket;

public class ServerManager : MonoBehaviour
{
    WebSocket websocket;
    public static ServerManager Instance;

    private string ServerAddress;
    private string PortNumber;
    private float ClockSpeed;

    private Queue<string> messageQueue;

    void Awake(){
        if (Instance == null){
            Instance = this;
        }

        ServerAddress = SessionManager.Instance.Address;
        PortNumber = "5000";
    }
    // Start is called before the first frame update
    async void Start()
    {
        websocket = new WebSocket($"ws://{ServerAddress}:{PortNumber}/echo");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection Open.");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log($"Error: {e}");
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log($"Connection closed. e?: {e}");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log($"On Message: {message}");
        };


        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketPing", 0.0f, ClockSpeed);

        await websocket.Connect();
    }

    public void EnqueueMessage(string msg){
        if (messageQueue == null)
            messageQueue = new Queue<string>();
        messageQueue.Enqueue(msg);
    }

    // Update is called once per frame
    void Update()
    {
        if (websocket != null)
            websocket.DispatchMessageQueue();
    }

    async void SendWebSocketPing(){
        if (websocket.State == WebSocketState.Open){
            while (messageQueue.Count > 0)
            {
                    var msg = messageQueue.Dequeue();
                    await websocket.SendText(msg);
            }
        }else{
            Debug.Log("websocket not open.");
        }
    }

    public async void echo(string message){
        await websocket.SendText(message);
    }

    private async void OnApplicationQuit(){
        await websocket.Close();
    }
}
