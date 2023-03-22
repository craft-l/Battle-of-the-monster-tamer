using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理与服务端的连接以及消息处理
/// </summary>
public class ClientManager : BaseManager
{
    public override void OnInit()
    {
        base.OnInit();
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
            }
        });
        /*
        NetReceiver.AddHandler<Protocol.get_troop_target>((data) =>
        {
            var rsp = data;
            Debug.Log("收到get_troop_target"+rsp);
            return null;
        });*/

    }

    // Update is called once per frame
    public void OnUpdate()
    {
         // 驱动消息分发
        NetCore.Dispatch();
    }
}
