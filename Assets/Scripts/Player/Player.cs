using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Invisible,
    Death
}

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer mesh;
    private Collider coll;

    private GameManager gm;
    public GameObject hpContainer;
    public GameObject menu;
    [SerializeField]private Camera minimapCamera;
    [SerializeField]private ShotSlot[] shotSlots;
    [SerializeField]private GameObject shot1;
    [SerializeField]private GameObject shot2;

    public int hp;
    public float energy;
    public int maxHp;
    public float maxEnergy;
    public float moveSpeed;
    public float defaultMoveSpeed;
    public int nowScore;
    public int nowPoint;
    public float invTime;
    
    [SerializeField] private Vector3 velocity;
    [SerializeField] private PlayerState state;
    [SerializeField] private bool inWall;

    private bool isInv = false;
    private bool isShotWait1 = false;
    private bool isShotWait2 = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        hp = maxHp;
        energy = maxEnergy;
        moveSpeed = defaultMoveSpeed;
        state = PlayerState.Normal;
        inWall = false;

        StartCoroutine(HealEnergy(1.0f, 0.1f));
    }

    void Update()
    {
        if (state != PlayerState.Death && gm.GetState() != GameState.GameOver){
            Invisible();
            Shot();

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

    private void Dead()
    {
        state = PlayerState.Death;
        gameObject.SetActive(false);
    }

    private void EnergyController()
    {
        if (energy < 0.0f){
            energy = 0.0f;
        }

        if (energy > maxEnergy){
            energy = maxEnergy;
        }
    }

    private IEnumerator HealEnergy(float value, float span)
    {
        while (true){
            if(state == PlayerState.Normal && energy < maxEnergy){
                energy += value;
            }
            yield return new WaitForSeconds(span);
        }
    }

    private IEnumerator DecreaseEnergy(float limit, float value, float span)
    {
        while(energy > limit){
            energy -= value;
            yield return new WaitForSeconds(span);
        }
    }

    public void TakeDamage(int damage)
    {
        if(!(state == PlayerState.Death || state == PlayerState.Invisible) && !isInv){
            hp -= damage;
            isInv = true;
            hpContainer.GetComponent<HpContainer>().Relocation();
            StartCoroutine(InvTimeCo(invTime));
        }
            
        if (hp <= 0){
            Dead();
        }
    }

    private IEnumerator InvTimeCo(float time)
    {
        StartCoroutine(Flashing());
        yield return new WaitForSeconds(time);
        isInv = false;
    }

    private IEnumerator Flashing()
    {
        while(isInv){
            mesh.enabled = false;
            yield return new WaitForSeconds(0.1f);
            mesh.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private bool CheckRaycastHit(float distance, string tag)
    {   
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, distance)){
            return hit.collider.CompareTag(tag);
        }
        else{
            return false;
        }
            
    }

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

    private protected void Move()
    {
        velocity = Vector3.zero;

        velocity.x += Input.GetAxisRaw("Horizontal");
        velocity.z += Input.GetAxisRaw("Vertical"); 

        velocity = velocity.normalized * moveSpeed * Time.deltaTime;
        
        if (velocity.magnitude > 0){
            float turnSpeed = 0.2f;
            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), turnSpeed);
            rb.position += velocity;
        }

        LookMousePoint();
        MovingRangeFixed(); 
    }

    private Coroutine shotCo;
    private void Shot()
    {
        if(state != PlayerState.Normal) return;
        if(menu.activeSelf == true) return;
        
        shot1 = shotSlots[0].shot;
        shot2 = shotSlots[1].shot;
        if (Input.GetButton("Fire1") && !isShotWait1 && !Input.GetButton("Fire2") && shot1 != null){
            if(energy < shot1.GetComponent<Shot>().shotData.useEnergy) return;
            energy -= shot1.GetComponent<Shot>().shotData.useEnergy;
            shot1.GetComponent<Shot>().Instance();
            float shotRate = shot1.GetComponent<Shot>().shotData.rate;
            isShotWait1 = true;
            StartCoroutine(ShotRateCo1(shotRate));
        }

        if (Input.GetButton("Fire2") && !isShotWait2 && !Input.GetButton("Fire1") && shot2 != null){
            if(energy < shot2.GetComponent<Shot>().shotData.useEnergy) return;
            energy -= shot2.GetComponent<Shot>().shotData.useEnergy;
            shot2.GetComponent<Shot>().Instance();
            float shotRate = shot2.GetComponent<Shot>().shotData.rate;
            isShotWait2 = true;
            StartCoroutine(ShotRateCo2(shotRate));
        }
    }

    private IEnumerator ShotRateCo1(float rate)
    {
        yield return new WaitForSeconds(rate);
        isShotWait1 = false;
    }

    private IEnumerator ShotRateCo2(float rate)
    {
        yield return new WaitForSeconds(rate);
        isShotWait2 = false;
    }

    Coroutine invCo;
    private void Invisible()
    {
        float useEnergy = 1.0f;
        float useEnergySpan = 0.1f;
        float speedMagnification = 2.0f;

        if(state == PlayerState.Normal){
            if (Input.GetButtonDown("Inv")){
                if (energy >= useEnergy){
                    invCo = StartCoroutine(DecreaseEnergy(0.0f, useEnergy, useEnergySpan));

                    state = PlayerState.Invisible;

                    mesh.material.color = new Color32(255, 255, 255, 100);
                    coll.isTrigger = true;
                }
            }
        }

        if(state == PlayerState.Invisible)
        {
            moveSpeed = defaultMoveSpeed * speedMagnification;

            if (!inWall){
                if (!Input.GetButton("Inv") || energy <= 0){
                    StopCoroutine(invCo);
                    state = PlayerState.Normal;
                    moveSpeed = defaultMoveSpeed;
                    mesh.material.color = new Color32(255, 255, 255, 255);
                    coll.isTrigger = false;
                }
            }
            else{
                if (energy <= 0){
                    StopCoroutine(invCo);
                    moveSpeed = defaultMoveSpeed / 2;
                }
            }
        }  
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall")){
            inWall = true;
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
