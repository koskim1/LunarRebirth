using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public GameObject Fireball;
    public Transform shootPoint;

    private Fireball fireball;


    public bool canShootFireball = false;    
    [SerializeField] private int maxCharges = 3;       // 최대 충전 가능한 탄 수
    private int currentCharges;                        // 현재 남아있는 탄 수
    [SerializeField] private float rechargeTime = 3f;  // 한 발 재충전에 필요한 시간
    private float shootSpeed = 17f;
    // Start is called before the first frame update
    void Start()
    {
        fireball = GetComponent<Fireball>();
        currentCharges = maxCharges;
    }

    private void Update()
    {
        if (canShootFireball && Input.GetKeyDown(KeyCode.E))
        {
            TryShoot();
        }
    }

    public void EnableFireball()
    {
        canShootFireball = true;
        //DataManager.Instance.CanShootFireball = true;
        //DataManager.Instance.SaveData();
    }

    private void TryShoot()
    {        
        if (currentCharges > 0)
        {
            // 발사 로직 실행
            SpawnAndShoot();
            currentCharges--;
            // 재충전 코루틴 시작
            StartCoroutine(RechargeCoroutine());
        }
    }

    private IEnumerator RechargeCoroutine()
    {
        // 2초 대기 후 한 발 충전
        yield return new WaitForSeconds(rechargeTime);
        // 최대 장전수 미만일 경우에만 충전
        if (currentCharges < maxCharges)
        {
            currentCharges++;
        }
        
    }
    void SpawnAndShoot()
    {
        GameObject fireball = Instantiate(Fireball, shootPoint.transform.position, shootPoint.rotation);

        Rigidbody rb = fireball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward.normalized * shootSpeed, ForceMode.Impulse);
        }
        AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.fireballSFX, transform, .5f);
    }

}
