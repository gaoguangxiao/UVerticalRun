using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoad : MonoBehaviour
{
    //是否到达第一段路
    bool m_ISFirst;
    //所有的路段对象
    public GameObject[] m_RoadArray;
    //障碍物数组对象
    public GameObject[] m_ObstacleArray;
    //障碍物的位置
    public List<Transform> m_ObstaclePosArray = new List<Transform>();
    //障碍物水平随机
    public Transform[] m_ObstaclePositionArray;
    // Start is called before the first frame update
    void Start()
    {
        m_ISFirst = true;
        //游戏开始自动生成3组障碍物
        for (int i = 0; i < 3; i++)
        {
            Spawn_Obstacle(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //根据传递的参数来决定切换那条路段
    public void Change_Road(int index)
    {
        //到达第一条路段不切换
        if (m_ISFirst && index == 0)
        {
            m_ISFirst = false;
            return;
        }
        else
        {
            int lastIndex = index - 1;
            if (lastIndex < 0) lastIndex = 2;

            int nextIndex = index + 1;
            if (nextIndex > 2) nextIndex = 0;

            m_RoadArray[lastIndex].transform.position = m_RoadArray[nextIndex].transform.position + new Vector3(0, 0, 80);
            Spawn_Obstacle(lastIndex);
        }
    }

    public void Spawn_Obstacle(int index)
    {
        //销毁原来的对象
        GameObject[] obsPast = GameObject.FindGameObjectsWithTag("Obstacle_" + index);
        for (int i = 0; i < obsPast.Length; i++)
        {
            Destroy(obsPast[i]);
        }
        //foreach (Transform item in m_ObstaclePosArray[index])
        //{
        //    if (item.gameObject.tag.Contains("Obstacle_"))
        //    {
        //       item.tag
        //    }
        //}
        //Debug.Log("m_ObstaclePosArray.count" + m_ObstaclePosArray.Count);
        //Debug.Log("m_ObstaclePosArray[index]" + m_ObstaclePosArray[index]);
        //Debug.Log("m_ObstacleArray.count" + m_ObstacleArray.Length);
        
        //生成障碍物
        foreach (Transform item in m_ObstaclePosArray[index])
        {
            int randomIndex = Random.Range(0, m_ObstacleArray.Length);
            //Debug.Log("randomIndex：" + randomIndex);
            GameObject prefab = m_ObstacleArray[randomIndex];
           
            //-3 0 3
            int randomPositionIndex = Random.Range(0, m_ObstaclePositionArray.Length);
            Transform position = m_ObstaclePositionArray[randomPositionIndex];
            Vector3 spawnPosition = new Vector3(
                position.position.x,
                5f, // 假定在地面上生成，可根据需要调整 Y 坐标
                0
            );
            GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);
            obj.tag = "Obstacle_" + index;
            obj.transform.SetParent(item, false); // 将新实例设置为 item 的子对象，但不改变世界坐标位置
        }
    }
}
