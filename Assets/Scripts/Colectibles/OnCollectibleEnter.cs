using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnCollectibleEnter : MonoBehaviour
{
    PlayerStats playerStats;
    AnimatorManager animatorManager;

    Transform HAND;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();
        HAND = GameObject.FindGameObjectWithTag("RightHand").transform;
        animatorManager = playerStats.GetComponentInChildren<AnimatorManager>();
    }

    private void OnTriggerEnter(Collider collided)
    {
        if (collided.tag == "Player")
        {
            Time.timeScale = 0;
            animatorManager.PlayTargetAnimation("Finding", true);
            playerStats.CollectibleCounter();

            StartCoroutine(Wait());
            StartCoroutine(SwordSpawn());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(0.7f);

        transform.SetParent(HAND);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    IEnumerator SwordSpawn()
    {

        yield return new WaitForSecondsRealtime(2.49f);

        playerStats.gameObject.GetComponent<PlayerAttacker>().weaponManager.LoadWeapon(true);
        playerStats.isIncreasing = true;
        playerStats.totalIncrease = 25;
        Destroy(gameObject);

    }
}
