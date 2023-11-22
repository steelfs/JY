using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidingDoor : MonoBehaviour
{
    public float speed = 4.0f;
    public Transform Icon;              // 문에 붙은 스티커
    public Transform Icon2;

    AudioSource audioSource;
    Transform door;

    float openHeight = 0.0f;
    float closeHeight = 0.0f;

    public bool lockedDoor = false;
    bool unlock = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        door = transform.parent.GetChild(0);
        if (Icon != null)
        {
            Icon.SetParent(door);
        }
        if (Icon2 != null)
        {
            Icon2.SetParent(door);
        }

        openHeight = door.position.y + 3.0f;
        closeHeight = door.position.y;

        //SpaceSurvival_GameManager.Instance.StageClear = StageList.All;
    }

    private void Start()
    {
        if (IsAllStageClear())
        {
            unlock = true;
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Npc"))
        {
            if (!lockedDoor || (lockedDoor && unlock))
            {
                StopAllCoroutines();
                StartCoroutine(Open());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Npc"))
        {
            if (!lockedDoor || (lockedDoor && unlock))
            {
                StopAllCoroutines();
                StartCoroutine(Close());
            }
        }
    }

    IEnumerator Open()
    {
        audioSource.Stop();
        audioSource.Play();
        while (door.position.y < openHeight)
        {
            door.position += new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
        door.position = new Vector3(door.position.x, openHeight, door.position.z);
        //audioSource.Play();
        //while (door.position.y < 3)
        //{
        //    door.position += new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
        //    yield return null;
        //}
    }

    IEnumerator Close()
    {
        audioSource.Stop();
        audioSource.Play();
        while (door.position.y > closeHeight)
        {
            door.position -= new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
        door.position = new Vector3(door.position.x, closeHeight, door.position.z);

        //audioSource?.Play();
        //while (door.position.y > 0)
        //{
        //    door.position -= new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
        //    yield return null;
        //}
    }

    /// <summary>
    /// 해당 함수 실행시켜서 전체 스테이지가 클리어 됬는지 체크한다 .
    /// </summary>
    /// <returns></returns>
    private bool IsAllStageClear()
    {
        return SpaceSurvival_GameManager.Instance.StageClear == StageList.All;
    }

}
