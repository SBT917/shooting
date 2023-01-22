using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//ショットを購入する時に使用するUIオブジェクトの制御
public class ShopObject : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler,IDropHandler, ISendShotData
{
    public Shot shot; //オブジェクトで購入できるショット
    public string shotName; //ショットの名前
    public int price; //ショットの値段

    private Player player;
    private Rarity rarity;
    private ShotData shotData;
    private TextMeshProUGUI shotNameText; //ショットの名前を表示するテキスト
    private TextMeshProUGUI priceText; //ショットの値段を表示するテキスト
    private Image icon; //ショットのアイコン
    private Image frame; //オブジェクトのフレーム
    private AudioManager audioManager;

    private Vector2 prevPos;

    void Start()
    {
        shotData = shot.GetComponent<Shot>().shotData;
        shotName = shotData.shotName;
        price = shotData.price;
        rarity = shotData.rarity;
        frame = transform.GetChild(0).GetComponent<Image>();
        shotNameText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        priceText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        icon = transform.GetChild(3).GetComponent<Image>();

        shotNameText.text = shotName;   
        priceText.text = price.ToString();
        frame.color = shot.shotData.rarity.color;
        icon.sprite = shot.shotData.icon;

        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
    }

    //オブジェクトをドラッグし始めた時の処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        prevPos = transform.position; //ドラッグの開始位置の保存
    }

    //オブジェクトをドラッグ中の処理
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //オブジェクトの位置をマウスポインタの位置にする
    }

    //オブジェクトをドラッグし終えた時の処理
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = prevPos; //オブジェクトをドラッグの位置に戻す
    }

    //オブジェクトがレイキャスト判定にドラッグされたときの処理
    public void OnDrop(PointerEventData eventData)
    {
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach(var hit in raycastResults){
            if(hit.gameObject.CompareTag("ShotInfo")){
                ShotInfo shotInfo = hit.gameObject.GetComponent<ShotInfo>();
                if(player.nowPoint >= price && shotInfo.shot != shot){
                    player.nowPoint -= price;
                    shotInfo.slot.SetShot(shot);
                    audioManager.PlaySE("Buy", player.audioSource);
                    Destroy(gameObject);
                }
                else{
                    audioManager.PlaySE("BuyFailure", player.audioSource);
                }
            }       
        }
    }

    public ShotData SendShotData()
    {
        return shot.shotData;
    }
}
