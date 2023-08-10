using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillCollider : MonoBehaviour
{
    public float skillTick = 0.2f;
    public float skillPower = 10.0f;

    private void OnEnable()
    {
        Clear();
    }
    List<Enemy> enemies = new List<Enemy>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (other != null)
            {
                enemies.Add(enemy);
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (other != null)
            {
                enemies.Remove(enemy);
            }

        }
    }
    public void Clear()
    {
        enemies.Clear();
        StartCoroutine(SkillCoroutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator SkillCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillTick);
            foreach(var enemy in enemies)
            {
                enemy.defence(skillPower);
            }
        }
    }
}
