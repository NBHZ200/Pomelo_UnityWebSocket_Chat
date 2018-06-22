# Pomelo_UnityWebSocket_Chat
自己修改的 Pomelo Chat 客户端，Unity Webgl 可用
修改者邮箱：931461808@qq.com，发现问题可及时联系;
必须先发布成 Webgl 才能正常运行，不能用 Editor 正常运行;
它的服务器是 chatofpomelo-websocket-master, 下载地址：https://github.com/NetEase/chatofpomelo-websocket;
此修改版，每个客户端只能使用一个 PomeloClient，不支持同时连接多个;
如需同时连接多个 PomeloClient，可修改 WebSocketReceiver.cs 中的 OnReceive 函数, 重新制作它的响应方式;
登录脚本为 WebglPomeloLogin.cs, 收发脚本为 ChatWeb.cs;
修改者使用的 Unity 版本为 2017.3.1f1 64bit 个人版, 请使用相同版本;

