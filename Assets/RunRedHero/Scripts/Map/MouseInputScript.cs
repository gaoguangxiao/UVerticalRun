using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseInputScript : MonoBehaviour
{

    //玩家动态创建，并不能用编辑器拉线赋值。在初始化玩家之后，输入和玩家绑定
    public HeroNavRun heroNavRun;//玩家导航移动脚本

    public GraphicRaycaster graphicRaycaster;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!heroNavRun) return;
        //鼠标导航
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckGuiRaycastObjects())//检测到的物体有多个不执行点击事件
            {
                return;
            }
            //获取鼠标点击左边
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            //获取点击位置
            if (Physics.Raycast(ray, out hit))
            {
                //判断是否点击在UI上
                //bool isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();
                //Debug.Log("click ui bool:" + isPointerOverGameObject);

                //点击位置
                //targetVector = hit.point;
                //Debug.Log("click point:" + targetVector);
                heroNavRun.OnMouseMove(hit.point);
            }
        }
    }

    bool CheckGuiRaycastObjects()
    {
        //EventSystem.current
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;

        List<RaycastResult> list = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }
}
