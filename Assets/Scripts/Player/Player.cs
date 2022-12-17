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

    private GameManager gm;
    public HpContainer hpContainer;
    [SerializeField]private GameObject menu;
    [SerializeField]private Camera minimapCamera;
    [SerializeField]private ShotSlot[] shotSlots;
    [SerializeField]private Material defaultMaterial;
    [SerializeField]private Material invisibleMaterial;
    [SerializeField]private ParticleSystem particle;

    public int hp; //現在のHP
    public float energy; //現在のENERGY
    public int maxHp; //最大HP
    public float maxEnergy; //最大ENERGY
    public float moveSpeed; //移動速度
    public float defaultMoveSpeed; //移動速度の初期値
    public int nowScore; //現在のスコア
    public int nowPoint; //現在のポイント数
    
    [SerializeField] private float invTime; //ダメージを受けた時の無敵時間
    [SerializeField] private Vector3 velocity; //移動に使用するベクトル
    [SerializeField] private PlayerState state; //プレイヤーの状態
    [SerializeField] private bool inWall; //壁の中にいるかどうか

    private bool isInv = false; //ダメージを受けた際の無敵時間かどうか
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponentInChildren<MeshRenderer>();
        coll = GetComponent<Collider>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

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
            Shot();
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
        while (true){
            if(state == PlayerState.Normal && energy < maxEnergy){
                energy += maxEnergy / defaultEnergy; //回復量は現在の最大Energyをデフォルトで割った値(Energyが強化されても最大回復するまでの時間を一定にするため)
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Energyを消費する時のコルーチン
    private IEnumerator DecreaseEnergy(float limit, float value, float span)
    {
        while(energy > limit){
            energy -= value;
            yield return new WaitForSeconds(span);
        }
    }

    //ダメージを受けた時の処理
    public void TakeDamage(int damage)
    {
        if(!(state == PlayerState.Death || state == PlayerState.Invisible) && !isInv){
            hp -= damage;
            isInv = true;
            hpContainer.Relocation();
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

        if(Input.GetButton("Fire1") || Input.GetButton("Fire2")){
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
        velocity = Vector3.zero;

        velocity.x += Input.GetAxisRaw("Horizontal");
        velocity.z += Input.GetAxisRaw("Vertical"); 

        velocity = velocity.normalized * moveSpeed * Time.deltaTime;
        
        //入力された方向に向く処理
        if (velocity.magnitude > 0){
            float turnSpeed = 0.2f;
            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), turnSpeed);
            rb.position += velocity;
        }

        LookMousePoint();
        MovingRangeFixed(); 
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

    //透明化状態の処理
    Coroutine invCo;
    private void Invisible()
    {
        float useEnergy = 1.0f; //透明化中の際に消費し続けるEnergy
        float useEnergySpan = 0.1f; //Energyを消費するスパン
        float speedMagnification = 2.0f; //透明化状態の移動速度倍率

        //プレイヤーが通常状態の時にのみ透明化状態になる操作を受け付ける
        if(state == PlayerState.Normal){
            if (Input.GetButtonDown("Inv")){
                if (energy >= useEnergy){
                    invCo = StartCoroutine(DecreaseEnergy(0.0f, useEnergy, useEnergySpan));

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
                if (!Input.GetButton("Inv") || energy <= 0){ //壁の外にいる時にEnergyが切れてしまったら強制的に戻される
                    StopCoroutine(invCo);
                    state = PlayerState.Normal;
                    moveSpeed = defaultMoveSpeed;
                    mesh.material = defaultMaterial;
                    coll.isTrigger = false;
                }
            }
            else{
                if (energy <= 0){ //壁の中にいる時にEnergyが切れてしまったら速度が半減
                    StopCoroutine(invCo);
                    moveSpeed = defaultMoveSpeed / 2;
                }
            }
        }  
    }

    //ゲームがwaveの間に入った時の処理(強化画面が開ける様になる)
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
