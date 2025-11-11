using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackWizard : MonoBehaviour
{
    public GameObject lightAttack;
    public AttackCounter lightAttackCounter;

    public GameObject heavyAttack;
    public AttackCounterWizard heavyAttackCounter;
    //角色伤害 
    public float damage;
    //重击相关
    public GameObject bullet;
    public Transform firePoint;
    public float bulletSpeed = 7;

    void Start()
    {
        lightAttack = this.transform.Find("LightAttack").gameObject;
        lightAttackCounter = lightAttack.GetComponent<AttackCounter>();
    }

    //轻击--在动画中调用 
    public void LightAttack()
    {
        damage = PlayerDataManager.Instance.GetCurrentPlayerData().atk;
        StartCoroutine(AttackCoroutine(lightAttack, lightAttackCounter, damage));
        SoundManager.Instance.PlaySound(2, "playerLightAttack", false);
    }
    //轻击--在动画中调用 
    public void HeavyAttack()
    {
        damage = PlayerDataManager.Instance.GetCurrentPlayerData().atk;
        //得到方位制造子弹
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 fireDirection = (new Vector2(mousePos.x, mousePos.y) - new Vector2(firePoint.position.x, firePoint.position.y)).normalized;

        heavyAttack = Instantiate(bullet, firePoint.position, Quaternion.identity);
        heavyAttackCounter = heavyAttack.GetComponent<AttackCounterWizard>();
        //传入伤害
        heavyAttackCounter.InitAttack(damage);
        //给子弹速度
        Rigidbody2D rb = heavyAttack.GetComponent<Rigidbody2D>();
        rb.velocity = fireDirection * bulletSpeed;
   
        SoundManager.Instance.PlaySound(2, "playerLightAttack", false);
    }

    public virtual IEnumerator AttackCoroutine(GameObject attackCollider, AttackCounter attackCounter, float damage)
    {
        attackCounter.InitAttack(damage);
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.3f);//0.3s检测时间
        attackCollider.SetActive(false);
    }
}
