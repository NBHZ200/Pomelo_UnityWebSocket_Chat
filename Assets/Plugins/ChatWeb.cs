using UnityEngine;
using SimpleJson;

public class ChatWeb : MonoBehaviour
{
    void Awake()
    {
        WebglPomeloLogin.Instance.wpClient.on("onAdd", (data) => {
        });
        WebglPomeloLogin.Instance.wpClient.on("onLeave", (data) =>{
        });
        WebglPomeloLogin.Instance.wpClient.on("onChat", (data) => {
            Debug.Log(data);
        });
        InvokeRepeating("Send", 0, 0.1f);
    }



    public void SendChatInner(string msgInner)
    {
        JsonObject message = new JsonObject();
        message.Add("rid", WebglPomeloLogin.channel);//channel
        message.Add("content", msgInner);
        message.Add("from", WebglPomeloLogin.userName);//name
        message.Add("target", "*");
        WebglPomeloLogin.Instance.wpClient.request("chat.chatHandler.send", message, (data) =>
        {
        });
    }

    public void Send()
    {
        SendChatInner("这是一句话");
    }



    void OnApplicationQuit()
    {
        if (WebglPomeloLogin.Instance.wpClient != null)
            WebglPomeloLogin.Instance.wpClient.disconnect();
    }

}


