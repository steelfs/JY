using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    enum DoorState
    {
        Close,
        Forward,
        Back
    }
    DoorState state = DoorState.Close;
    DoorState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case DoorState.Close:
                        animator.SetInteger(Hash_Door, 0);
                        break;
                    case DoorState.Forward:
                        animator.SetInteger(Hash_Door, 1);
                        break;
                    case DoorState.Back:
                        animator.SetInteger(Hash_Door, 2);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public float speed = 4.0f;

    Transform door;
    Animator animator;
    Cho_PlayerMove player;
    InteractionUI interactionUI;
    AudioSource[] audioSources;

    readonly int Hash_Door = Animator.StringToHash("DoorState");

    private void Awake()
    {
        door = transform.parent.GetChild(0);
        animator = door.GetComponent<Animator>();
        player = FindObjectOfType<Cho_PlayerMove>();
        interactionUI = FindObjectOfType<InteractionUI>();
        audioSources = GetComponents<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.interaction += OnInteract;
            interactionUI.visibleUI?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.interaction = null;
            interactionUI.invisibleUI?.Invoke();
        }
    }

    private void OnInteract()
    {
        if (State == DoorState.Close)
        {
            Vector3 dir = player.transform.position - transform.position;
            float angle = Vector3.Angle(door.forward, dir);

            if (angle < 90.0f)
            {
                //animator.SetInteger(Hash_Door, 2);                // 뒤로 열기
                State = DoorState.Back;
            }
            else
            {
                //animator.SetInteger(Hash_Door, 1);                // 앞으로 열기
                State = DoorState.Forward;
            }

            audioSources[0].Play();
        }
        else
        {
            //animator.SetInteger(Hash_Door, 0);
            State = DoorState.Close;
            audioSources[1].Play();
        }
    }
}
