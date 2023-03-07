using System.IO;
using UnityEngine;

public class SaveManager : BaseManager
 {
    //private static readonly string SVAE_PATH = Application.persistentDataPath + "/Data/";
    private static readonly string SVAE_PATH = Application.dataPath + "/Data/";

    public void SaveByJson(object data,string saveFileName)
    {
        string json = JsonUtility.ToJson(data);
        string path = Path.Combine(SVAE_PATH,saveFileName);

        try{
            File.WriteAllText(path,json);

            #if UNITY_EDITOR
            Debug.Log($"Save data to {path} sucessfully");
            #endif
        }catch(System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to save data to {path},{exception}");
            #endif
        }
    }

    public T LoadFromJson<T>(string saveFileName)
    {
        string path = Path.Combine(SVAE_PATH,saveFileName);

        try{
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }catch(System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to load data to {path},{exception}");
            #endif

            File.WriteAllText(path,JsonUtility.ToJson(default(T)));
            return default;
        }
    }


    public void DeleteSaveFile(string saveFileName)
    {
        string path = Path.Combine(SVAE_PATH,saveFileName);

        try{
            File.Delete(path);
        }catch(System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"Failed to delete {path},{exception}");
            #endif
        }

    }
/*
    public void WriteData(object data, string dataPath)
    {
        string path = SVAE_PATH + dataPath;
        string str = JsonUtility.ToJson(data);
        Debug.Log("SaveManager:" + str);
        File.WriteAllText(path,str);
    }

    public TData ReadData<TData>(string dataPath)
    {
        string path = SVAE_PATH + dataPath;
        if(File.Exists(path))
        {
            if(new FileInfo(path).Length != 0)
            {
                Debug.Log("SaveManager: readData not empty");
                string str = File.ReadAllText(path);
                return JsonUtility.FromJson<TData>(str);
            }else{
                Debug.Log("SaveManager: readData empty");
                return default(TData);
            }
        }
        else{
            File.WriteAllText(path,JsonUtility.ToJson(default(TData)));
            return default(TData);
        }

    }*/
}
