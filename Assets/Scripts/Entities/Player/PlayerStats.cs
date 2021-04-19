using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : EntityStats
{
    public Scrollbar hpBar;

    public Image hpPanel;
    AnimatorManager animatorManager;

    int totalCollectibles;

    public bool isIncreasing;
    public int totalIncrease;
    float increase;

    public bool collecting = false;

    void Start()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
        hpMax = SetMaxHealth();
        hpCurrent = hpMax;
        hpBar.size = hpCurrent / 100;
        hpPanel.CrossFadeAlpha((hpMax - hpCurrent)/100, 0.1f, false);
        SetPanelFade();
    }

    private void Update()
    {
        if (isIncreasing)
        {
            collecting = true;
            TakeDamage(-0.1f);
            increase += 0.1f;
            if (increase >= totalIncrease)
            {
                isIncreasing = false;
                increase = 0;

                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2.4f);

        Time.timeScale = 1;
        collecting = false;
    }

    void SetPanelFade()
    {
        //hpPanel.CrossFadeAlpha((hpMax - hpCurrent)/100, 0.1f, false);
        hpBar.size = 0 + hpCurrent /100;
    }

    public void TakeDamage(float damage)
    {
        if (hpCurrent == 100 && damage < 0)
        {
            return;
        }

        hpCurrent -= damage;
        SetPanelFade();

        if (hpCurrent >= 0)
        {
            if (!animatorManager.animator.GetBool("isInteracting"))
                animatorManager.PlayTargetAnimation("Idle Hurt", false);
        }
        else
        {
            hpBar.enabled = false;
            hpBar.GetComponent<CanvasGroup>().alpha = 0;
            animatorManager.PlayTargetAnimation("Death", true);

            StartCoroutine(DeathTimer());

            //hpPanel.color = Color.black;
            //hpPanel.CrossFadeAlpha(100, 0.01f, false);
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Lose");
    }

    public void CollectibleCounter()
    {
        totalCollectibles += 1;
        animatorManager.animator.SetInteger("collectibles", totalCollectibles);

        if (totalCollectibles < 7)
        {
            //lightning
        }
        else
        {
            SceneManager.LoadScene("Win");
        }
    }


}
