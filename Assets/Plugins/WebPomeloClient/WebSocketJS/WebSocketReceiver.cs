using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace WebSocketJS
{
    public class WebSocketReceiver : MonoBehaviour
    {
        public static WebSocketReceiver instance { get; private set; }

        private Dictionary<string, Action> m_openActions = new Dictionary<string, Action>();
        private Dictionary<string, Action> m_closeActions = new Dictionary<string, Action>();
        //private Dictionary<string, List<Action<byte[]>>> m_receiveActions = new Dictionary<string, List<Action<byte[]>>>();
        private Dictionary<string, List<Pomelo.WebglPomelo.Transporter>> m_recActions = 
            new Dictionary<string, List<Pomelo.WebglPomelo.Transporter>>();
        private WebSocketReceiver()
        { }

        void Awake()
        {
            
            if(instance == null)
                instance = this.GetComponent<WebSocketReceiver>();
            DontDestroyOnLoad(this.gameObject);
        }


        
        /// <summary>
        /// 如果没有Receiver物体，用这个函数创建
        /// </summary>
        public static void AutoCreateInstance()
        {
            GameObject go = GameObject.Find("/WebSocketReceiver");
            if (go == null)
            {
                go = new GameObject("WebSocketReceiver");
            }

            if (go.GetComponent<WebSocketReceiver>() == null)
            {
                go.AddComponent<WebSocketReceiver>();
            }

            instance = go.GetComponent<WebSocketReceiver>();
        }


        /// <summary>
        /// 添加收到消息函数
        /// </summary>
        public void AddRecListener(string address, Pomelo.WebglPomelo.Transporter transporter)
        {
            if(!m_recActions.ContainsKey(address))
                m_recActions[address] = new List<Pomelo.WebglPomelo.Transporter>();
            m_recActions[address].Add(transporter);
        }


        /// <summary>
        /// 移除收到消息函数
        /// </summary>
        public void RemoveRecListener(string address, Pomelo.WebglPomelo.Transporter transporter)
        { 
            m_recActions[address].Remove(transporter);  
        }


        public void AddListener(string address, Action onOpen, Action onClose)
        {
            if (!m_openActions.ContainsKey(address))
                m_openActions.Add(address, null);
            m_openActions[address] = onOpen;

            if (!m_closeActions.ContainsKey(address))
                m_closeActions.Add(address, null);
            m_closeActions[address] = onClose;
        }


        public void RemoveListener(string address)
        {
            if (m_openActions.ContainsKey(address))
                m_openActions.Remove(address);

            if (m_closeActions.ContainsKey(address))
                m_closeActions.Remove(address);
        }


        /// <summary>
        /// 内置函数：jslib 会在 Receive Message 后调用此方法，其实就相当于 OnSend
        /// </summary>
        private void OnReceive(string strData)
        {
            
            byte[] data;
            string address;
            GetMsgByte(strData, out address, out data);
            for (int i = 0; i < m_recActions[address].Count; ++i)
            {
                m_recActions[address][i].onReceive(data);
            }
        }

        /// <summary>
        /// 内置函数：jslib 会在 open 连接后调用此方法
        /// </summary>
        private void OnOpen(string address)
        {
            if (m_openActions.ContainsKey(address) && m_openActions[address] != null)
            {
                m_openActions[address].Invoke();
            }
        }
        
        /// <summary>
        /// 内置函数：jslib 会在关闭连接后调用此方法
        /// </summary>
        private void OnClose(string address)
        {
            if (m_closeActions.ContainsKey(address) && m_closeActions[address] != null)
            {
                m_closeActions[address].Invoke();
            }
        }

        ///<summary>
        ///<para>翻译 message, 只在收消息的时候调用</para>
        ///</summary>
        private void GetMsgByte(string address_data, out string address, out byte[] data)
        {
            int splitIndex = address_data.LastIndexOf("_");
            address = address_data.Substring(0, splitIndex);
            string hexData = address_data.Substring(splitIndex + 1);
            data = new byte[hexData.Length / 2];
            for (int i = 0; i < data.Length; i++)
            {
                string hex = hexData.Substring(i * 2, 2);
                data[i] = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            }
        }
    }
}
