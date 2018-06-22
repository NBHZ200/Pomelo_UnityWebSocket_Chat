using UnityEngine;
using SimpleJson;
using Pomelo.WebglPomelo;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WebglPomeloLogin : MonoBehaviour {
    
    /// <summary>
    /// pomelo webgl 客户端 
    /// </summary>
    public WebglPomeloClient wpClient = null;
    /// <summary>
    /// 用来存储你刚进入聊天室时候 channel 里的用户
    /// </summary>
    public JsonObject users = null;
    /// <summary>
    /// 单例
    /// </summary>
    public static WebglPomeloLogin Instance;
    /// <summary>
    /// 是否跳到下一个场景（下一个场景是指聊天收发场景）
    /// </summary>
    bool isLoad = false;
    /// <summary>
    /// 登录时以及之后聊天的  用户名
    /// </summary>
    public static string userName;
    /// <summary>
    /// 房间号
    /// </summary>
    public static string channel;
    /// <summary>
    /// 输入用户名的 UGUI 控件
    /// </summary>
    public InputField inputName;
    /// <summary>
    /// 输入房间号的 UGUI 控件
    /// </summary>
    public InputField inputChannel;


    public void Start()
    {
        //创建 WebSocket 收发器，这句必须写
        if (WebSocketJS.WebSocketReceiver.instance == null)
        {
            WebSocketJS.WebSocketReceiver.AutoCreateInstance();
        }
        //写入单例
        Instance = this.GetComponent<WebglPomeloLogin>();
        //此脚本以及 Delayer 脚本切换场景时不销毁
        //Delayer 脚本也挂在此物体上
        DontDestroyOnLoad(this.gameObject);
    }


    public void Login()
    {
        userName = inputName.text;
        channel = inputChannel.text;
        if (wpClient == null)
        {
            wpClient = new WebglPomeloClient();
            //监听网络状态变化事件
            wpClient.NetWorkStateChangedEvent += (state) =>
            {
                Debug.Log("CurrentState is:" + state);
            };
            
            wpClient.initClient("ws://127.0.0.1:3014/", () =>
            {
                JsonObject msg = new JsonObject();
                wpClient.connect(msg, (JsonObject json) =>
                {
                    JsonObject user = new JsonObject();
                    user["uid"] = userName;
                    wpClient.request("gate.gateHandler.queryEntry", user, OnQuery);
                });
             });

        }
    }

    void OnQuery(JsonObject result)
    {
        if (Convert.ToInt32(result["code"]) == 200)
        {
            wpClient.disconnect();

            string host = (string)result["host"];
            int port = Convert.ToInt32(result["port"]);

            wpClient = new WebglPomeloClient();
            wpClient.initClient("ws://" + host + ":" + port.ToString() + "/", () =>
            {
                JsonObject msg = new JsonObject();
                wpClient.connect(msg, (JsonObject json) =>
                {
                    JsonObject userMessage = new JsonObject();
                    userMessage["username"] = userName;
                    userMessage["rid"] = channel;
                    if (wpClient != null)
                    {
                        wpClient.request("connector.entryHandler.enter", userMessage, OnEntry);
                    }
                });
            });
        }
    }

    void OnEntry(JsonObject data)
    {
        users = data;
        isLoad = true;
    }




    private void Update()
    {
        if (isLoad)
        {
            SceneManager.LoadScene("Chat");
            isLoad = false;
        }
    }

    void OnApplicationQuit()
    {
        if (wpClient != null)
            wpClient.disconnect();

    }




}
