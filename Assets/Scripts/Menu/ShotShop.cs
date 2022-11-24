using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotShop : MonoBehaviour
{
    [SerializeField]private GameObject[] shots;
    [SerializeField]private ShotSlot[] shotSlots;
    [SerializeField]private GameObject shopObject;
    [SerializeField]private List<GameObject> shopLineup;
    
    public void DrawingShop()
    {   
        if(shopLineup != null){
            for(int i = 0; i < transform.childCount; ++i){
                Destroy(transform.GetChild(i).gameObject);
            }
            shopLineup.Clear();
        }

        while(shopLineup.Count < 3){
            int value = Random.Range(0, shots.Length);
                if(!shopLineup.Contains(shots[value]) && shots[value] != shotSlots[0].shot && shots[value] != shotSlots[1].shot){
                    GameObject go = Instantiate(shopObject, transform);
                    go.GetComponent<ShopObject>().shot = shots[value];
                    shopLineup.Add(go.GetComponent<ShopObject>().shot);
                }  
            }
        }
    }
