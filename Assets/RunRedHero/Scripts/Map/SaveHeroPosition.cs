using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json; // 引入Json.NET库来序列化和反序列化Json数据（需要先安装Json.NET库）
using System.IO; // 引入System.IO命名空间以使用File类


[System.Serializable]
public class PositionRecord
{
    public float time; // 时间戳，可以用Time.time获取
    public float x, y, z; // 位置
}

public class SaveHeroPosition : MonoBehaviour
{

    private string pathPositioin;

    private HeroNavRun heroNavRun;//角色脚本

    private CameraMapController cameraMapController;

    // Start is called before the first frame update
    void Start()
    {
        pathPositioin = Application.persistentDataPath + "/positionHistory.json";

        heroNavRun = GetComponent<HeroNavRun>();

        cameraMapController = GameObject.FindGameObjectWithTag("CameraMap").GetComponent<CameraMapController>();
        //读取位置
        LoadPosition();

        //LoadPlayerPositionFromJson();
    }

    // Update is called once per frame
    void Update()
    {
        PositionRecord positionRecord = new PositionRecord
        {
            time = Time.time,
            x = transform.position.x,
            y = transform.position.y,
            z = transform.position.z
        };
        //Debug.Log(positionRecord.position);
        //SavePlayerPositionToJson(positionRecord);

        SavePositionHistoryToPlayerPrefs(positionRecord);
    }

    void SavePlayerPositionToJson(PositionRecord positionRecord)
    {
        string json = JsonConvert.SerializeObject(positionRecord); // 将PositionRecord序列化为Json字符串
        Debug.Log("position: " + json);
        File.WriteAllText(pathPositioin, json); // 将Json字符串写入文件系统中的文件
    }

    // 从JSON文件加载玩家位置
    void LoadPlayerPositionFromJson()
    {
        string positionStr = File.ReadAllText(pathPositioin);
        PositionRecord position = JsonConvert.DeserializeObject<PositionRecord>(positionStr);
        //PositionRecord position = JsonConvert.FromJson<PositionRecord>(positionStr);
        Debug.Log("LoadPosition position: " + position);
        transformToPositon(new Vector3(
            position.x,
            position.y,
            position.z));
    }

    private void SavePositionHistoryToPlayerPrefs(PositionRecord position)
    {
        string positionStr = position.x + "," + position.y + "," + position.z;
        PlayerPrefs.SetString("PlayerPosition", positionStr);
        PlayerPrefs.Save(); // 确保数据被写入磁盘
    }

    private void LoadPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPosition"))
        {
            string positionStr = PlayerPrefs.GetString("PlayerPosition");
            Debug.Log("LoadPosition position: " + positionStr);
            string[] positionArr = positionStr.Split(',');
            float x = float.Parse(positionArr[0]);
            float y = float.Parse(positionArr[1]);
            float z = float.Parse(positionArr[2]);
            transformToPositon(new Vector3(x, y, z));
        }
    }

    void transformToPositon(Vector3 vector)
    {
        //transform.position = vector;
        heroNavRun.transformToPositon(vector);

        cameraMapController.justMapPosition(vector);
    }
}
