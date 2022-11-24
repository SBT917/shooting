using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopObject : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler,IDropHandler
{
    public GameObject shot;
    public string shotName;
    public int price;

    private Player player;
    private Rarity rarity;
    private TextMeshProUGUI shotNameText;
    private TextMeshProUGUI priceText;
    private Image image;

    private Vector2 prevPos;

    void Start()
    {
        shotName = shot.GetComponent<Shot>().shotData.shotName;
        price = shot.GetComponent<Shot>().shotData.price;
        rarity = shot.GetComponent<Shot>().shotData.rarity;
        image = GetComponent<Image>();
        shotNameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        priceText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        shotNameText.text = shotName;   
        priceText.text = "x" + price.ToString();
        image.color = shot.GetComponent<Shot>().shotData.rarity.color;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = prevPos;
    }

    public  void OnDrop(PointerEventData eventData)
    {
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach(var hit in raycastResults){
            if(hit.gameObject.CompareTag("ShotSlot")){
                ShotSlot slot = hit.gameObject.GetComponent<ShotSlot>();
                if(player.nowPoint >= price && slot.shot != shot){
                    player.nowPoint -= price;
                    slot.ChangeShot(shot);
                    Destroy(gameObject);
                } 
            }       
        }
    }
}
