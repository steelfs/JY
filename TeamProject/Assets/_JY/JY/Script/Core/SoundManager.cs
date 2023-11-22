using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip MouseOver;
    public AudioClip MouseClick;
    public AudioClip LevelUp;
    public AudioClip NormalAttack;
    public AudioClip Sniping;
    public AudioClip penetrate;
    public AudioClip rampage;
    public AudioClip buff;
    public AudioClip onOff_toggleUI;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayOneShot_MouseOver() { audioSource.PlayOneShot(MouseOver); }
    public void PlayOneShot_Click() { audioSource.PlayOneShot(MouseClick); }
    public void PlayOneShot_LevelUp() {  audioSource.PlayOneShot(LevelUp);}
    public void PlayOneShot_NormalAttack() {  audioSource.PlayOneShot(NormalAttack);}
    public void PlayOneShot_Sniping() { audioSource.PlayOneShot(Sniping); }
    public void PlayOneShot_Penetrate() { audioSource.PlayOneShot(penetrate); }
    public void PlayOneShot_Rampage() { audioSource.PlayOneShot(rampage); }
    public void PlayOneShot_Buff() { audioSource.PlayOneShot(buff); }
    public void PlayOneShot_OnOffToggle() { audioSource.PlayOneShot(onOff_toggleUI); }

}
