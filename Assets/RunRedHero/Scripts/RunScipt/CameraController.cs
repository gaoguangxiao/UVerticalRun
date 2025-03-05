using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvent
{
    public static string mouseScrollWheel = "Mouse ScrollWheel";

}

public class CameraController : MonoBehaviour
{

    public float zoomSpeed = 10f; // 缩放速度

    //角色的vertor
    private Transform playTransform;

    //照相机和角色偏移量
    private Vector3 relativeffset;

    // 相机与目标的竖直高度参数
    public float distanceUp = 5f;
    // 相机与目标的水平距离参数
    public float distanceAway = 5f;
    // 跟随的平滑度参数
    public float smooth = 0.5f;

    ////俯仰和偏转的灵敏度
    //public float pitchSensitivity; //Y方向 俯仰（绕X轴）
    //public float yawSensitivity; //X方向 偏转（绕Y轴）

    ////最大俯仰角度
    //public float pitchLimit = 75;

    //private Quaternion playerCamCenter;

    // Start is called before the first frame update
    void Start()
    {
        //playerCamCenter = transform.localRotation;

        //获取当前角色的变换
        playTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //获取相对位置
        relativeffset = transform.position - playTransform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //角色移动时候
        //transform.position = playTransform.position + relativeffset;

        //获取滚轮滚动大小
        //float scroll = Input.GetAxis(MouseEvent.mouseScrollWheel);

        //Debug.Log("滚轮" + scroll);

        //Camera.main.fieldOfView += scroll * zoomSpeed;

        //限制视野
        //Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30, 80);

        //Pitch();

        //固定相机跟随
        Vector3 desiredPosition = new Vector3(relativeffset.x,
            playTransform.position.y + relativeffset.y,
            playTransform.position.z + relativeffset.z);

        // 第三人称摄像机跟随
        // 计算相机期望的位置，由目标位置加上竖直方向的偏移（Vector3.up * distanceUp）再减去目标正方向的偏移（target.forward * distanceAway）
        //Vector3 desiredPosition = playTransform.position + Vector3.up * distanceUp - playTransform.forward * distanceAway;
        // 使用线性插值（Lerp）方法将当前相机位置平滑过渡到期望位置，插值速度由时间差（Time.deltaTime）和自定义的平滑度参数（smooth）决定
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smooth);
        // 让相机始终看向目标物体
        //Camera.main.transform.LookAt(playTransform);

        //var cameraTransform = Camera.main.transform;
        //cameraTransform.eulerAngles = new Vector3(cameraTransform.eulerAngles.x, playTransform.eulerAngles.y, cameraTransform.eulerAngles.z);
    }

   
}
