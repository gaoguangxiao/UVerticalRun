using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class OnLinePlayer : NetworkBehaviour
{
    //On

    public override void OnStartLocalPlayer()
    {
        //摄像机与角色绑定
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = Vector3.zero;

        //player初始化
        //player 名称的位置,大小
        //name.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        //name.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        //随机生成颜色和名字
        //ChangedColorAndName();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;


        var movex = Input.GetAxis("Horizontal") * Time.deltaTime * 110f;
        var movez = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, movex, 0);
        transform.Translate(0, 0, movez);

    }
}
