using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotShop : MonoBehaviour
{
    [SerializeField]private Shot[] shots;
    [SerializeField]private ShotSlot[] shotSlots;
    [SerializeField]private ShopObject shopObject;
    [SerializeField]private List<Shot> shopLineup;
    
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
                    ShopObject so = Instantiate<ShopObject>(shopObject, transform);
                    so.shot = shots[value];
                    shopLineup.Add(so.shot);
                }  
            }
        }
    }
