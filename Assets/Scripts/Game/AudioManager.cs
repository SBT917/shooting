using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BGMの制御
public class AudioManager : MonoBehaviour
{
    [SerializeField]private AudioSource introSource; //イントロ部分のAudioSource
    [SerializeField]private AudioSource mainSource; //メイン部分のAudioSource
    [SerializeField]private AudioSource melodySource; //メロディ部分のAudioSource(メインゲーム以外はこの部分のボリュームを0にする)
    
    private GameManager gameManager;

    void Start()
    {
        gameManager = transform.root.GetComponent<GameManager>();
    }

    public void Play()
    {   
        //メインが再生されていなければイントロから再生する
        if(!mainSource.isPlaying){
            introSource.PlayScheduled(AudioSettings.dspTime); 
            mainSource.PlayScheduled(AudioSettings.dspTime + ((float)introSource.clip.samples / (float)introSource.clip.frequency)); //イントロの長さ分経過したらメインを再生
            melodySource.PlayScheduled(AudioSettings.dspTime + ((float)introSource.clip.samples / (float)introSource.clip.frequency)); //イントロの長さ分経過したらメロディを再生
            return;
        }

        switch(gameManager.GetState()){
            case GameState.BreakTime:
                StartCoroutine(VolumeFade(melodySource, 1.0f, false)); //GameStateがBreakTimeならメロディの音量を0にする
                break;
            case GameState.Game:
                StartCoroutine(VolumeFade(melodySource, 1.0f, true)); //ゲームがスタートしたらメロディの音量を戻す
                break;
        }
    }

    //AudioSourceの音量をフェードイン/フェードアウトさせる関数
    private IEnumerator VolumeFade(AudioSource source, float seconds, bool fadeIn){
        if(fadeIn){
            while(source.volume < 1){
                source.volume += seconds / 10;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else{
            while(source.volume > 0){
                source.volume -= seconds / 10;
                yield return new WaitForSeconds(0.1f);
            }
        }     
    }

}
