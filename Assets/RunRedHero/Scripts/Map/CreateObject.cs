using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CreateObject : NetworkBehaviour
{
    //public GameObject objectToSpawn; // 要生成的游戏对象预制体
    public GameObject[] objectToSpawns; // 要生成的游戏对象预制体数组
    public int numberOfObjects = 10; // 要生成的对象数量
    public Transform spawnParent; // 生成对象的父层级节点
    public Vector2 spawnArea = new Vector2(5f, 10f); // 生成区域的大小

    // Start is called before the first frame update
    void Start()
    {
        //只有服务器才可以生产对象
        //for (int i = 0; i < numberOfObjects; i++)
        //{
        //    Vector3 spawnPosition = new Vector3(
        //        Random.Range(0, spawnArea.x),
        //        0f, // 假定在地面上生成，可根据需要调整 Y 坐标
        //        Random.Range(0, spawnArea.y)
        //    );
        //    GameObject newGameObject = objectToSpawns[Random.Range(0, objectToSpawns.Length)];

        //    //NetworkServer.Spawn(newGameObject);
        //    GameObject instance = Instantiate(newGameObject, spawnPosition, Quaternion.identity);
        //    instance.transform.SetParent(spawnParent, false); // 将新实例设置为 spawnParent 的子对象，但不改变世界坐标位置
        //}
    }

    public override void OnStartLocalPlayer() {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(0, spawnArea.x),
                0f, // 假定在地面上生成，可根据需要调整 Y 坐标
                Random.Range(0, spawnArea.y)
            );
            GameObject newGameObject = objectToSpawns[Random.Range(0, objectToSpawns.Length)];

            //NetworkServer.Spawn(newGameObject);
            GameObject instance = Instantiate(newGameObject, spawnPosition, Quaternion.identity);
            instance.transform.SetParent(spawnParent, false); // 将新实例设置为 spawnParent 的子对象，但不改变世界坐标位置
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
