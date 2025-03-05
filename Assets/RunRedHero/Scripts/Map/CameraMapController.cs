using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class CameraMapController : MonoBehaviour
{

    private Camera CameraMap;

    public float zoomSpeed = 10.0f;

    //角色的vertor
    private Transform playTransform;

    //照相机和角色偏移量
    private Vector3 relativeffset;

    //缩放滑块
    public Slider zoomSlider;

    public float minFOV = 60f; // 最小视野角度

    public float maxFOV = 200f; // 最大视野角度

    public float distanceY = 10.0f;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

        CameraMap = GetComponent<Camera>();


        zoomSlider.onValueChanged.AddListener(delegate
        {
            AdjustCameraFOV(zoomSlider.value);
        });
    }

    //更新小地图位置
    public void RefreshLocationPosition(Transform transform)
    {
        playTransform = transform;
        //Debug.Log("playTransform :" + playTransform);

        //计算小地图和角色的相对位置
        relativeffset = transform.position - playTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playTransform) return;

        CameraMap.fieldOfView = zoomSpeed;

        Vector3 desiredPosition = new Vector3(playTransform.position.x + relativeffset.x,
            relativeffset.y + distanceY,
            relativeffset.z + playTransform.position.z);
        transform.position = desiredPosition;
    }

    void AdjustCameraFOV(float value)
    {
        //Debug.Log("AdjustCameraFOV:" + value);
        distanceY = value * 800;
        //float newFOV = Mathf.Lerp(minFOV, maxFOV, value); // 插值计算新的FOV值
        //CameraMap.fieldOfView = newFOV; // 应用新的FOV值到摄像机
    }

    //调整位置
    public void justMapPosition(Vector3 vector)
    {
        if (!playTransform) return;

        transform.position = new Vector3(vector.x, vector.y + 10, vector.z);

        relativeffset = transform.position - playTransform.position;
    }

}
