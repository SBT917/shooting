using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの状態
public enum PlayerState
{
    Normal, //通常状態
    Invisible, //透明化状態
    Death //やられた
}

//プレイヤーの制御
public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer mesh;
    private Collider coll;
    public AudioSource audioSource;

    private GameManager gm;
    private AudioManager audioManager;
    public HpContainer hpContainer;
    [SerializeField]private GameObject menu;
    [SerializeField]private GameObject pauseMenu;
    [SerializeField]private Material defaultMaterial;
    [SerializeField]private Material invisibleMaterial;
    [SerializeField]private ParticleSystem particle;

    public int hp; //現在のHP
    public float energy; //現在のENERGY
    public int maxHp; //最大HP
    public float maxEnergy; //最大ENERGY
    public float moveSpeed; //移動速度
    public float defaultMoveSpeed; //移動速度の初期値
    public ShotSlot[] shotSlots;
    public int nowScore; //現在のスコア
    public int nowPoint; //現在のポイント数
    public bool inWall; //壁の中にいるかどうか
    
    [SerializeField] private float invTime; //ダメージを受けた時の無敵時間
    [SerializeField] private Vector3 velocity; //移動に使用するベクトル
    [SerializeField] private PlayerState state; //プレイヤーの状態

    private bool isInv = false; //ダメージを受けた際の無敵時間かどうか
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponentInChildren<MeshRenderer>();
        coll = GetComponent<Collider>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();

        hp = maxHp;
        energy = maxEnergy;
        moveSpeed = defaultMoveSpeed;
        state = PlayerState.Normal;
        inWall = false;
        

        StartCoroutine(HealEnergy());
    }

    void Update()
    {
        if (state != PlayerState.Death && gm.GetState() != GameState.GameOver){
            PauseMenuController();
            if(pauseMenu.activeSelf) return;

            LookMousePoint();
            MovingRangeFixed(); 
            Shot();
            Recharge();
            Invisible();
            EnergyController();
            BreakTimeController(); 
        }
    }

    void FixedUpdate()
    {
        if (state != PlayerState.Death && gm.GetState() != GameState.GameOver){
            Move();     
        }
    }

    public PlayerState GetState()
    {
        return state;
    }

    public void SetState(PlayerState newState)
    {
        state = newState;
    }

    //倒されたときの処理
    private void Dead()
    {
        state = PlayerState.Death;
        ParticleSystem p = Instantiate<ParticleSystem>(particle, transform.position, Quaternion.identity);
        audioManager.PlaySE("Dead", p.GetComponent<AudioSource>());
        p.Play();
        gameObject.SetActive(false);
    }

    //Energyが最小値、最大値を超えないように
    private void EnergyController()
    {
        if (energy < 0.0f){
            energy = 0.0f;
        }

        if (energy > maxEnergy){
            energy = maxEnergy;
        }
    }

    //Energyの回復コルーチン
    private IEnumerator HealEnergy()
    {
        float defaultEnergy = maxEnergy; //開始時の最大Energyをデフォルトとする
        float healSpeed = 5.0f;
        
        while (true){
            if(state == PlayerState.Normal && energy < maxEnergy){
                float healingAmount = maxEnergy / defaultEnergy; //回復量は現在の最大Energyをデフォルトで割った値(Energyが強化されても最大回復するまでの時間を一定にするため)
                energy = Mathf.MoveTowards(energy, energy + healingAmount, Time.deltaTime * healingAmount * healSpeed); //スライダーがなめらかになるよう補完して回復
            }
            yield return null;
        }
    }

    //Energyを継続して消費する時のコルーチン
    private IEnumerator DecreaseEnergy(float limit, float value, float span)
    {
        while(energy > limit){
            energy = Mathf.MoveTowards(energy, energy - value, Time.deltaTime * span); //スライダーがなめらかになるよう補完して消費
            yield return null;
        }
    }

    //ダメージを受けた時の処理
    public void TakeDamage(int damage)
    {
        if(!(state == PlayerState.Death || state == PlayerState.Invisible) && !isInv){
            audioManager.PlaySE("Damage", audioSource);
            hp -= damage;
            isInv = true;
            hpContainer.TakeDamage();
            StartCoroutine(InvTimeCo(invTime));
        }
            
        if (hp <= 0){
            Dead();
        }
    }

    //ダメージを受けた時の無敵時間
    private IEnumerator InvTimeCo(float time)
    {
        StartCoroutine(Flashing());
        yield return new WaitForSeconds(time);
        isInv = false;
    }

    //無敵時間中は点滅する
    private IEnumerator Flashing()
    {
        while(isInv){
            mesh.enabled = false;
            yield return new WaitForSeconds(0.1f);
            mesh.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    //マップ上の移動範囲の制限
    private void MovingRangeFixed()
    {   
        GameObject floor = GameObject.FindWithTag("Floor");
        Vector3 currentPosition = transform.position;
        Vector3 floorEdge = floor.transform.localScale * 5.0f;
        floorEdge -= new Vector3(0.5f, 0.0f, 0.5f);

        currentPosition.x = Mathf.Clamp(currentPosition.x, -floorEdge.x, floorEdge.x);
        currentPosition.z = Mathf.Clamp(currentPosition.z, -floorEdge.z, floorEdge.z);
        transform.position = currentPosition;
    }

    //マウスポインターの方向を見る
    private void LookMousePoint()
    {
        if(state != PlayerState.Normal) return;
        if(menu.activeSelf == true) return;

            if(Input.GetButton("Fire1") || (shotSlots[1].shot != null && Input.GetButton("Fire2")) ){
            Plane plane = new Plane();
	        float distance = 0;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);            
            plane.SetNormalAndPosition(Vector3.up, transform.localPosition);
            if (plane.Raycast(ray, out distance)){
                Vector3 lookPoint = ray.GetPoint(distance);
                transform.LookAt(lookPoint);
            }
        }
    }

    //移動処理
    private protected void Move()
    {
        //velocity = Vector3.zero;

        //velocity.x += Input.GetAxisRaw("Horizontal");
        //velocity.z += Input.GetAxisRaw("Vertical"); 

        //velocity = velocity.normalized * moveSpeed * Time.deltaTime;

        ////入力された方向に向く処理
        //if (velocity.magnitude > 0){
        //    float turnSpeed = 0.2f;
        //    rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), turnSpeed);
        //    rb.position += velocity;
        //}
    }

    //ショットを放つ
    private void Shot()
    {
        if(state != PlayerState.Normal) return;
        if(menu.activeSelf == true) return;
        
        if (Input.GetButton("Fire1") && !Input.GetButton("Fire2")){
            shotSlots[0].Fire();
        }

        if (Input.GetButton("Fire2") && !Input.GetButton("Fire1")){
            shotSlots[1].Fire();
        }
    }

    private void Recharge()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            for(int i = 0; i < 2; ++i){
                StartCoroutine(shotSlots[i].ShotRechargeCo());
            }
        }
    }

    //透明化状態の処理
    Coroutine invCo;
    private void Invisible()
    {
        float useEnergy = 1.0f; //透明化中の際に消費し続けるEnergy
        float useEnergySpeed = 10.0f; //Energyを消費するスピード
        float speedMagnification = 2.0f; //透明化状態の移動速度倍率
        float inWallSpeedMagnification = 1.2f; //透明化かつ壁の中にいる時の移動速度倍率

        //プレイヤーが通常状態の時にのみ透明化状態になる操作を受け付ける
        if(state == PlayerState.Normal){
            if (Input.GetButtonDown("Inv")){
                if (energy >= useEnergy){
                    audioManager.PlaySE("InvisibleOn", audioSource);
                    invCo = StartCoroutine(DecreaseEnergy(0.0f, useEnergy, useEnergySpeed));
                    state = PlayerState.Invisible;

                    mesh.material = invisibleMaterial;
                    coll.isTrigger = true;
                }
            }
        }   

        //透明化状態時の処理
        if(state == PlayerState.Invisible)
        {
            moveSpeed = defaultMoveSpeed * speedMagnification;

            if (!inWall){
                if (!Input.GetButton("Inv") || energy <= 0){ //壁の外にいる時にEnergyが切れてしまったら強制で通常に戻される
                    audioManager.PlaySE("InvisibleOff", audioSource);
                    StopCoroutine(invCo);
                    state = PlayerState.Normal;
                    moveSpeed = defaultMoveSpeed;
                    mesh.material = defaultMaterial;
                    coll.isTrigger = false;
                }
            }
            else{
                moveSpeed *= inWallSpeedMagnification;
                if (energy <= 0){ //壁の中にいる時にEnergyが切れてしまったら速度が半減
                    StopCoroutine(invCo);
                    moveSpeed = defaultMoveSpeed / 2;
                }
            }
        }  
    }

    //ゲームがwaveの間に入った時の処理(強化画面が開けるようになる)
    private void BreakTimeController()
    {
        if(gm.GetState() == GameState.BreakTime){
            if(Input.GetKeyDown(KeyCode.E)){
		        menu.SetActive(!menu.activeSelf);
            }
            if(Input.GetKeyDown(KeyCode.G)){
                gm.ForceGameStart();
            }
        }
        else{
            if(menu.activeSelf){
                menu.SetActive(false);
            }
        }
    }

    private void PauseMenuController()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            audioManager.PlaySE("Pause", audioSource);
            if(!pauseMenu.activeSelf){
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else{
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall")){
            inWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall")){
            inWall = false;
        }
    }
}
