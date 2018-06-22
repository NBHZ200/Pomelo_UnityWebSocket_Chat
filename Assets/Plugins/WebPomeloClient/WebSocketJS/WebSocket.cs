using System.Runtime.InteropServices;
using System;

namespace WebSocketJS
{
    /// <summary>
    /// <para>WebSocket 表示一个网络连接，</para>
    /// <para>它可以是 connecting connected closing closed 状态，</para>
    /// <para>可以发送和接收消息，</para>
    /// <para>注册 receive 回调函数来处理收到的消息</para>
    /// </summary>
    public class WebSocket
    {
        public string address { get; private set; }
        public State state { get; private set; }
        public Action onOpen { get; set; }
        public Action onClose { get; set; }
        public Action onSend { get; set; }
        public Action<byte[]> onReceive { get; set; }
        private WebSocket() { }

        public WebSocket(string address)
        {
            this.address = address;
            this.state = State.Closed;
        }

        /*------------- call jslib method --------*/
        [DllImport("__Internal")]
        private static extern void ConnectJS(string str);
        [DllImport("__Internal")]
        private static extern void SendJS(byte[] data, int length);
        [DllImport("__Internal")]
        private static extern void CloseJS();
        /// <summary>
        /// 弹出消息对话框
        /// </summary>
        [DllImport("__Internal")]
        private static extern void AlertJS(string str);


        public void Connect()
        {
            this.state = State.Connecting;
            WebSocketReceiver.instance.AddListener(address, OnOpen, OnClose);
            ConnectJS(address);
        }

        public void Send(byte[] data)
        {
            SendJS(data, data.Length);
        }

        public void Close()
        {
            this.state = State.Closing;
            CloseJS();
        }

        public void Alert(string str)
        {
            AlertJS(str);
        }

        private void OnOpen()
        {
            if (onOpen != null)
                onOpen.Invoke();
            this.state = State.Connected;
        }

        private void OnReceive(byte[] msg)
        {
            if (onReceive != null)
                onReceive.Invoke(msg);
        }

        private void OnClose()
        {
            if (onClose != null)
                onClose.Invoke();
            this.state = State.Closed;
            WebSocketReceiver.instance.RemoveListener(address);
        }


        public enum State
        {
            Closed,
            Connecting,
            Connected,
            Closing,
        }
    }
}