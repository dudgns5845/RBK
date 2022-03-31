using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveStage(ProtoManager pm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/mapdata.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        DataSaver data = new DataSaver(pm);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static DataSaver LoadStage()
    {
        string path = Application.persistentDataPath + "/mapdata.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            DataSaver data = formatter.Deserialize(stream) as DataSaver;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("세이브 파일을 찾을 수 없습니다. 위치:" + path);
            return null;
        }
    }
}
