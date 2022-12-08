using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{   
    [SerializeField]private GameObject[] mapObjects;
    [SerializeField]private GameObject[] blocks;
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
        0 = Block
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
                    GameObject block = Instantiate(obj, pos, Quaternion.identity, transform);
                    BlockConnect(block, x, z);
                }
                else{
                    pos.y = 1.0f;
                    player.transform.position = pos;
                }
            }
        }
    }

    void BlockConnect(GameObject block, int x, int z)
    {
        int blockType = 0;
        if(mapData[z, x] == "0"){
            if(z - 1 >= 0 && mapData[z - 1, x] == "0"){
                blockType += 1;
            }
            if(x + 1 <= mapData.GetLength(0) - 1 && mapData[z, x + 1] == "0"){
                blockType += 2;
            }
            if(z + 1 <= mapData.GetLength(1) - 1 && mapData[z + 1, x] == "0"){
                blockType += 4;
            }
            if(x - 1 >= 0 && mapData[z, x - 1] == "0"){
                blockType += 8;
            }

            Instantiate(blocks[blockType], block.transform.position, blocks[blockType].transform.rotation, transform);
            Destroy(block);
        }
    }
    
    void FileLoad(in string[,] datas)
    {
        TextAsset csvFile = Resources.Load("MapData/map" + mapNum) as TextAsset;
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
