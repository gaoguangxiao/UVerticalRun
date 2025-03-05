using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Layer_lab._3D_Casual_Character; //引入角色
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mirror;
using Cinemachine;

public enum CharaterBodyState
{
    Idle,
    Running,
    Walk,
    Jumping,
    Death,
    Angry
}

public class HeroNavRun : NetworkBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;

    //默认
    [SerializeField] private AnimationClip animActionIdle;
    //行走
    [SerializeField] private AnimationClip animActionWalk;
    //记录状态
    private CharaterBodyState previousViewState = CharaterBodyState.Idle;
    //角色状态
    private CharaterBodyState state = CharaterBodyState.Idle;

    private Animator Animator { get; set; }

    public float moveSpeed = 10.0f;

    private CameraMapController cameraMap;//小地图

    private MouseInputScript mouseInputScript;//输入事件

    //相机跟随赋值
    private CinemachineVirtualCamera vcam;
    private void Awake()
    {
        
    }

    
    public override void OnStartLocalPlayer() {
    //}
    //public override void OnStartServer() {

        Animator = transform.GetComponentInChildren<Animator>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Debug.Log("agent: " + agent);

        //更新小地图位置
        cameraMap = GameObject.FindGameObjectWithTag("CameraMap").GetComponent<CameraMapController>();
        if (cameraMap)
        {
            cameraMap.RefreshLocationPosition(transform);
        }

        //绑定输入监听事件
        mouseInputScript = GameObject.FindGameObjectWithTag("EventInput").GetComponent<MouseInputScript>();
        mouseInputScript.heroNavRun = GetComponent<HeroNavRun>();

        //虚拟相机
        vcam = GameObject.FindGameObjectWithTag("CameraVirtual").GetComponent<CinemachineVirtualCamera>();
        if (vcam)
        {
            vcam.Follow = transform;
            vcam.LookAt = transform;
        }
        //controller = GetComponent<CharacterController>();
        //Debug.Log("transform.position: " + transform.position);
    }

    public void OnMouseMove(Vector3 vector3)
    {
        //导航移动
        agent.SetDestination(vector3);//设置目标位置

        //transform.localPosition = vector3;

        //transform.transform.LookAt(vector3);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (!agent)
        {
            Debug.Log("agent is nil");
            return;
        }
        //匹配导航移动动画 检查是否到达了目标点
        if (agent.remainingDistance <= 0.1f)
        {
            // 停止动画
            UpdatState(CharaterBodyState.Idle);
        }
        else
        {
            //行走动画
            UpdatState(CharaterBodyState.Walk);
        }

    }

    void UpdatState(CharaterBodyState newState)
    {
        if (state == newState) return;
        state = newState;

        Debug.Log("UpdatState" + newState);
        //跳跃状态
        if (state == CharaterBodyState.Jumping)
        {
            return;
        }

        if (previousViewState != state)
        {
            previousViewState = state;
            PlayStateAnimation();
            //Debug.Log("UpdatState" + state);
        }
    }

    void PlayStateAnimation()
    {
        if (state == CharaterBodyState.Idle && animActionIdle != null) PlayAnimation(animActionIdle);
        else if (state == CharaterBodyState.Walk && animActionWalk != null) PlayAnimation(animActionWalk);
    }

    void PlayAnimation(AnimationClip clip)
    {
        Animator.CrossFadeInFixedTime(clip.name, 0.25f);
    }

    public void transformToPositon(Vector3 vector)
    {
        if (agent)
        {
            agent.enabled = false;
        }
        //Animator.enabled = false;
        //transform.Translate(vector);
        //transform.position
        transform.position = vector;
        if (agent)
        {
            agent.enabled = true;
        }
        //Animator.enabled = true;
        //Debug.Log("LoadVector: " + vector);
        //Debug.Log("LoadPosition: " + transform.position);
    }

}
