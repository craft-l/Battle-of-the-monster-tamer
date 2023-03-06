using UnityEngine;

public class SkynetTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 初始化
        NetCore.Init();
        NetSender.Init();
        NetReceiver.Init();
        NetCore.enabled = true;

        // 连接服务端
        NetCore.Connect("127.0.0.1", 8888, () =>
        {
            // 连接结果
            Debug.Log("connect result: " + NetCore.connected);
            if(NetCore.connected)
            {
                SendSayHello();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
         // 驱动消息分发
        NetCore.Dispatch();
    }

    void SendSayHello()
    {
        var req = new SprotoType.sayhello.request();
        req.what = "Hi, I am Unity!";
        Debug.Log("发送sayhello消息给服务端");
        NetSender.Send<Protocol.sayhello>(req, (data) =>
        {
            var rsp = data as SprotoType.sayhello.response;
            Debug.LogFormat("服务端sayhello返回, error_code: {0}, msg: {1}", rsp.error_code, rsp.msg);
        });
    }
}
