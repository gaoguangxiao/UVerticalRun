using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunAuto : MonoBehaviour
{
    public float moveSpeed = 10.0f;

    public float moveSpeedMax = 100.0f;

    private Animator Animator { get; set; }

    [SerializeField] private AnimationClip animActionWalk;

    [SerializeField] private AnimationClip animActionRun;

    [SerializeField] private AnimationClip animActionAngry;

    //记录状态
    private CharaterBodyState previousViewState = CharaterBodyState.Idle;
    //角色状态
    private CharaterBodyState state = CharaterBodyState.Idle;

    public float moveLeftMoothSpeed = 10.0f;
    private bool isMovingRightMiddle = false; // 标记角色是否由右向中间移动
    private bool isMovingMiddleRight = false; // 标记角色是否由中间向右移动
    private bool isMovingMiddleLeft = false;  // 标记角色是否由中间向左移动
    private bool isMovingLeftMiddle = false;  // 标记角色是否由左向中间移动


    public CreateRoad createRoad;

    //奔跑距离
    public Text runDistanceText;
    //奔跑速度
    public Text runSpeedText;

    private Vector3 startPoint;//角色起始点
    // Start is called before the first frame update
    void Start()
    {
        Animator = transform.GetComponentInChildren<Animator>();

        state = CharaterBodyState.Running;
        Animator.CrossFadeInFixedTime(animActionRun.name, 0.25f);

        ////获取起始点
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == CharaterBodyState.Angry) return;

        //鼠标导航
        if (Input.GetMouseButtonDown(0))
        {
            //if (CheckGuiRaycastObjects())//检测到的物体有多个不执行点击事件
            //{
            //    return;
            //}
            //获取鼠标点击左边
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            isMovingRightMiddle = false;
            isMovingMiddleLeft = false;
            isMovingLeftMiddle = false;
            isMovingMiddleRight = false;

            //获取点击位置
            if (Physics.Raycast(ray, out hit))
            {
                if (transform.position.x > 1.0 && hit.point.x < transform.position.x)
                {
                    //Debug.Log("右中移动");
                    isMovingRightMiddle = true;
                }
                else if (transform.position.x <= 1.0 && hit.point.x < transform.position.x)
                {
                    //Debug.Log("中左移动");
                    isMovingMiddleLeft = true;
                }
                else if (transform.position.x < -1.0 && hit.point.x > transform.position.x)
                {
                    //Debug.Log("左中移动");
                    isMovingLeftMiddle = true;
                }
                else if (transform.position.x >= -1.0 && hit.point.x > transform.position.x)
                {
                    //Debug.Log("中右移动");
                    isMovingMiddleRight = true;
                }

            }
        }

        if (isMovingLeftMiddle)
        {
            if (transform.position.x >= 0)
            {
                isMovingLeftMiddle = false;
                return;
            }
            transform.Translate(Vector3.right * moveLeftMoothSpeed * Time.deltaTime);
        }
        else if (isMovingMiddleRight)
        {
            if (transform.position.x >= 3.0)
            {
                isMovingMiddleRight = false;
                return;
            }
            transform.Translate(Vector3.right * moveLeftMoothSpeed * Time.deltaTime);
        }
        else if (isMovingRightMiddle)
        {
            if (transform.position.x <= 0)
            {
                isMovingRightMiddle = false;
                return;
            }

            transform.Translate(Vector3.left * moveLeftMoothSpeed * Time.deltaTime);
        }
        else if (isMovingMiddleLeft)
        {
            if (transform.position.x <= -3)
            {
                isMovingMiddleLeft = false;
                return;
            }
            transform.Translate(Vector3.left * moveLeftMoothSpeed * Time.deltaTime);
        }
        else
        {

        }
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        float moveDistance = transform.position.z - startPoint.z;
        runDistanceText.text = moveDistance + "米";
        runSpeedText.text = moveSpeed + "米/秒";

        //速度根据距离加快5
        if (moveSpeed <= moveSpeedMax)
        {
            if(moveDistance > 200 && moveSpeed < 15)
            {
                moveSpeed = 15;
            } else if(moveDistance > 500 && moveSpeed < 20){
                moveSpeed = 20;
            } else if (moveDistance > 1000 && moveSpeed < 25)
            {
                moveSpeed = 25;
            }
            else if (moveDistance > 2000 && moveSpeed < 35)
            {
                moveSpeed = 35;
            }
            else if (moveDistance > 4000 && moveSpeed < 40)
            {
                moveSpeed = 40;
            }
            else if (moveDistance > 8000 && moveSpeed < 50)
            {
                moveSpeed = 50;
            }
            else if (moveDistance > 10000 && moveSpeed < 60)
            {
                moveSpeed = 60;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //障碍物

        //食物
        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Obstacles")
        {
            //障碍物，置为原点
            //transform.position = startPoint;

            //将人物
            Destroy(collision.gameObject);

            state = CharaterBodyState.Angry;
            //人物-生气-
            Animator.CrossFadeInFixedTime(animActionAngry.name, 0.25f);
            //1秒之后奔跑
            Invoke("ResetRun", 1.0f);
        }
        else if (collision.gameObject.tag.Contains("Obstacle_"))
        {
            //障碍物，向前移动
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 15);

            //移除当前障碍物
            Destroy(collision.gameObject);

            //角色闪烁
        }
        else if (collision.gameObject.name == "finish_000")
        {
            //
            createRoad.Change_Road(0);
        }
        else if (collision.gameObject.name == "finish_001")
        {
            createRoad.Change_Road(1);
        }
        else if (collision.gameObject.name == "finish_002")
        {
            createRoad.Change_Road(2);
        }
        //Debug.Log("enter:" + collision.gameObject);
        //Debug.Log("enter.tag:" + collision.gameObject.tag);
    }

    void ResetRun()
    {
        state = CharaterBodyState.Running;
        //人物-奔跑
        Animator.CrossFadeInFixedTime(animActionRun.name, 0.25f);
    }
}
