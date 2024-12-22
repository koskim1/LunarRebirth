using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public GameObject Fireball;
    public Transform shootPoint;

    private Fireball fireball;


    public bool canShootFireball = false;    
    [SerializeField] private int maxCharges = 3;       // �ִ� ���� ������ ź ��
    private int currentCharges;                        // ���� �����ִ� ź ��
    [SerializeField] private float rechargeTime = 3f;  // �� �� �������� �ʿ��� �ð�
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
            // �߻� ���� ����
            SpawnAndShoot();
            currentCharges--;
            // ������ �ڷ�ƾ ����
            StartCoroutine(RechargeCoroutine());
        }
    }

    private IEnumerator RechargeCoroutine()
    {
        // 2�� ��� �� �� �� ����
        yield return new WaitForSeconds(rechargeTime);
        // �ִ� ������ �̸��� ��쿡�� ����
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
