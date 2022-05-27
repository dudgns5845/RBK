using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MapEditor
{
    //���� ����/���� �ϴ� ��ü 
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

        // ���������� �����ϴ� �޼ҵ�
        //public static void SaveStage(ProtoManager pm)
        //{
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    string path = Application.persistentDataPath + "/mapdata.save";
        //    FileStream stream = new FileStream(path, FileMode.Create);

        //    MapData data = new MapData(pm);

        //    formatter.Serialize(stream, data);
        //    stream.Close();
        //}

        //�����͸� �����ϴ� �Լ�
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
                Debug.LogError("���̺� ������ ã�� �� �����ϴ�. ��ġ:" + path);
                return null;
            }
        }
    }
}
