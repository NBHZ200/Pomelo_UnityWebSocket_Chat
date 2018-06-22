using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using WebSocketJS;

/// <summary>
/// ���Խű�  ����ɾ
/// </summary>
public class TestWebSocket : MonoBehaviour
{
    public Text receive;
    public InputField address;
    public InputField message;
    public Dropdown enter_Dropdown;
    private WebSocket m_scoket;
    private string m_content = "Receive:\n";

    public void Connect()
    {
        if (m_scoket != null && m_scoket.state != WebSocket.State.Closed)
            return;

        m_scoket = new WebSocket(address.text);
        m_scoket.onOpen += OnOpen;
        m_scoket.onClose += OnClose;
        m_scoket.onReceive += OnReceive;
        
        m_scoket.Connect();
    }

    public void Close()
    {
        if (m_scoket.state == WebSocket.State.Connecting
            || m_scoket.state == WebSocket.State.Connected)
        {
            m_scoket.Close();
        }
    }

    public void Send()
    {
        if (m_scoket.state == WebSocket.State.Connected)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(message.text);
            m_scoket.Send(data);
        }
    }

    public void OnOpen()
    {

    }

    public void OnClose()
    {
        m_content += "Closed\n";
        m_scoket.Alert(" socket closed : " + m_scoket.address);
        m_scoket.onOpen -= OnOpen;
        m_scoket.onClose -= OnClose;
        m_scoket.onReceive -= OnReceive;
    }

    public void OnReceive(byte[] data)
    {
        if(data != null)
            m_content += System.Text.Encoding.UTF8.GetString(data) + "\n";
    }

    public void OnDorpdownCheck(int i)
    {
        message.text = enter_Dropdown.options[i].text;
    }
    private void Start()
    {

        enter_Dropdown.onValueChanged.AddListener(OnDorpdownCheck);
    }

    void Update()
    {
        receive.text = m_content;
    }
}
