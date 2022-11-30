using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{   
    [SerializeField]private GameObject[] mapObjects;
    [SerializeField]private GameObject player;
    [SerializeField]private int mapNum;
    
    float objSize = 5.0f;
    float offset = 37.5f;
    const int mapSizeX = 16;
    const int mapSizeZ = 16;
    string[,] mapData = new string[mapSizeX, mapSizeZ];

     void Awake() 
    {
        MapGenerate();
    }

    /* -1 = None
        0 = Wall
        1 = EnemySpawnArea
        2 = TargetSpawnArea
    */
    void MapGenerate()
    {   
        FileLoad(in mapData);
        for(int z = 0; z < mapData.GetLength(1); ++z){
            for(int x = 0; x < mapData.GetLength(0); ++x){
                GameObject obj;
                Vector3 pos = new Vector3(-offset + (objSize * x), 0, offset - (objSize * z));
                if(mapData[z, x] == "-1") continue;
                if(mapData[z, x] != "p"){
                    obj = mapObjects[int.Parse(mapData[z, x])];
                    Instantiate(obj, pos, Quaternion.identity, transform);
                }
                else{
                    pos.y = 1.0f;
                    player.transform.position = pos;
                }
            }
        }
    }
    
    void FileLoad(in string[,] datas)
    {
        TextAsset csvFile = Resources.Load("map" + mapNum) as TextAsset;
        if(csvFile == null) return;

        StringReader reader = new StringReader(csvFile.text);;
        while (reader.Peek() != -1)
        {   
            for(int z = 0; z < mapData.GetLength(1); ++z){
                string line = reader.ReadLine();
                for(int x = 0; x < mapData.GetLength(0); ++x){
                    if(string.IsNullOrEmpty(line.Split(',')[x])){
                        datas[z, x] = "-1";
                    }
                    else{
                        datas[z, x] = line.Split(',')[x];
                    }
                }
            }
        }
    }
}
