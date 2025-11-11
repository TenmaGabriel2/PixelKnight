using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 序列化、反序列化方案
/// </summary>
public enum JsonType
{
    JsonUtlity,
    LitJson
}

/// <summary>
/// Json管理器 用于序列化存储和反序列化读取json
/// </summary>
public class JsonMgr 
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;

    private JsonMgr() { }
   

    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        //确定存储路径
        string path =  Application.persistentDataPath + "/" + fileName + ".json";
        //序列化
        string jsonStr = "";
        switch (type)
        {
            case JsonType.JsonUtlity:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
               jsonStr = LitJson.JsonMapper.ToJson(data);
                break;
        }
        //将序列化的json字符串保存到文件中
        File.WriteAllText(path, jsonStr);
    }

    //用于读取json文件 （反序列化
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        //确定存储路径
        //应先判断默认数据文件夹中是否存在该文件，若存在则读取，若不存在则换路径
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        //读取json字符串
        if (!File.Exists(path)) //若不存在 取个反 返回true进入该函数 更换路径
        {
           path =  Application.persistentDataPath + "/" + fileName + ".json";
        }
        //如果读写文件夹还没有 则返回一个默认对象
        if (!File.Exists(path))
            return new T();
        //反序列化
        string jsonStr = File.ReadAllText(path);

        T data = default(T);
        switch (type)
        {
            case JsonType.JsonUtlity:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = LitJson.JsonMapper.ToObject<T>(jsonStr);
                break;
        }
        //把对象返回出去
        return data;
    }
}
