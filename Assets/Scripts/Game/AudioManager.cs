using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

[System.Serializable]
public class AudioData
{
    public string name;
    public AudioClip clip;
    [Range(0, 1)]public float volume = 1;
}

//BGMの制御
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField]private AudioSource introSource; //イントロ部分のAudioSource
    [SerializeField]private AudioSource mainSource; //メイン部分のAudioSource
    [SerializeField]private AudioSource melodySource; //メロディ部分のAudioSource(メインゲーム以外はこの部分のボリュームを0にする)

    [SerializeField]private List<AudioData> SeDatas; //SEのデータリスト


    private GameManager gameManager;

    void Start()
    {
        gameManager = transform.root.GetComponent<GameManager>();
    }

    public void PlayBGM()
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
                StartCoroutine(VolumeFadeOut(melodySource, 1.0f)); //GameStateがBreakTimeならメロディの音量を0にする
                break;
            case GameState.Game:
                StartCoroutine(VolumeFadeIn(melodySource, 1.0f)); //ゲームがスタートしたらメロディの音量を戻す
                break;
            case GameState.GameOver:
                StartCoroutine(VolumeFadeOut(introSource, 3.0f));
                StartCoroutine(VolumeFadeOut(melodySource, 3.0f));
                StartCoroutine(VolumeFadeOut(mainSource, 3.0f));
                break;
        }
    }

    //指定した名前のSEを再生
    public void PlaySE(string name, AudioSource audioSource)
    {
        if(audioSource.gameObject == null) return;

        AudioData data = SeDatas.Find(se => se.name == name);
        audioSource.volume = data.volume;
        audioSource.PlayOneShot(data.clip);
    }

    //AudioSourceの音量をフェードイン/フェードアウトさせる関数
    private IEnumerator VolumeFadeIn(AudioSource source, float seconds){
        while(source.volume < 1){
            source.volume += (1.0f / 10.0f) / seconds;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator VolumeFadeOut(AudioSource source, float seconds){
        while(source.volume > 0){
            source.volume -= (1.0f / 10.0f) / seconds;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

}
