using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulToMLP : MonoBehaviour
{
    public float attractionRange = 6f;
    public float moveSpeed = 8;
    private Transform playerTransform;
    private bool isAttracted = false;
    private bool isCollected = false;

    public int mlpValue = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        StartCoroutine(SoulSpawnWait());
    }

    private IEnumerator SoulSpawnWait()
    {
        yield return new WaitForSeconds(1.3f);

        StartCoroutine(MoveTowardsPlayer());
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);

                if (distance <= attractionRange)
                {
                    isAttracted = true;
                }

                if (isAttracted)
                {
                    Vector3 direction = (playerTransform.position - transform.position).normalized;
                    direction.y = 0f;

                    transform.position += direction * moveSpeed * Time.deltaTime;
                }
            }

            yield return null;
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;
            PlayerAttributesManager.Instance.AddMLP(mlpValue);

            transform.DOScale(0, .8f).OnComplete(()=>Destroy(gameObject)).SetLink(gameObject);
        }
    }
}
