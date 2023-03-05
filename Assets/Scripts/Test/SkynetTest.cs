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
        });
    }

    // Update is called once per frame
    void Update()
    {
         // 驱动消息分发
        NetCore.Dispatch();
    }
}
