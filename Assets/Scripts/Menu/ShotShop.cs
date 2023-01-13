using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotShop : MonoBehaviour
{
    [SerializeField]private Shot[] shots;
    [SerializeField]private Rarity[] rarities;
    [SerializeField]private ShotSlot[] shotSlots;
    [SerializeField]private ShopObject shopObject;
    [SerializeField]private List<Shot> shopLineup;
    private Dictionary<string, List<Shot>> shotBox = new Dictionary<string, List<Shot>>();
    
    void Awake()
    {
        foreach(Rarity rarity in rarities){
            shotBox.Add(rarity.name, new List<Shot>());
        }

        foreach(Shot shot in shots){
            shotBox[shot.shotData.rarity.name].Add(shot);
        }

    }

    public void DrawingShop()
    {   
        if(shopLineup != null){
            for(int i = 0; i < transform.childCount; ++i){
                Destroy(transform.GetChild(i).gameObject);
            }
            shopLineup.Clear();
        }

        while(shopLineup.Count < 3){
            List<Shot> box = shotBox[ChooseRarity()];
            int value = Random.Range(0, box.Count);
            Shot choosedShot = box[value];
            if(!shopLineup.Contains(choosedShot) && choosedShot != shotSlots[0].shot && choosedShot != shotSlots[1].shot){
                ShopObject so = Instantiate<ShopObject>(shopObject, transform);
                so.shot = choosedShot;
                shopLineup.Add(so.shot);
            }  
        }
    }

    private string ChooseRarity()
    {
        float total = 0;

        foreach(Rarity rarity in rarities){
            total += rarity.weight;
        }

        float randomValue = Random.value * total;

        foreach(Rarity rarity in rarities){
            if(randomValue < rarity.weight){
                return rarity.name;
            }
            else{
                randomValue -= rarity.weight;
            }
        }

        return rarities[rarities.Length - 1].name;
    }
}

    
