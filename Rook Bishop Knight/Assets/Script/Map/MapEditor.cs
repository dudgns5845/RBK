using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MapEditor
{
    //맵을 생성/저장 하는 객체 
    public class MapEditor : MonoBehaviour
    {
        public static MapEditor instance;
        public List<CubeData> cubes;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        private void Start()
        {
            GameObject Map = GameObject.Find("Map");
            for(int i = 0; i < Map.transform.childCount; i++)
            {
                cubes.Add(Map.transform.GetChild(i).GetComponent<CubeData>());
            }
        }

        // 스테이지를 저장하는 메소드
        //public static void SaveStage(ProtoManager pm)
        //{
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    string path = Application.persistentDataPath + "/mapdata.save";
        //    FileStream stream = new FileStream(path, FileMode.Create);

        //    MapData data = new MapData(pm);

        //    formatter.Serialize(stream, data);
        //    stream.Close();
        //}

        //데이터를 저장하는 함수
        public static MapData LoadStage()
        {
            string path = Application.persistentDataPath + "/mapdata.save";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                MapData data = formatter.Deserialize(stream) as MapData;
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
}
