using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip selectSound;
    [SerializeField] private AudioClip mergeSound;
    [SerializeField] private AudioClip errorSound;

    [SerializeField] private AudioSource buttonSounds;
    [SerializeField] private AudioSource gameSounds;
    [SerializeField] private AudioSource selectionSounds;

    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public void PlayClickSound()
    {
        buttonSounds.PlayOneShot(clickSound);
    }

    public void PLaySelectSound()
    {
        selectionSounds.PlayOneShot(selectSound);
    }

    public void PlayMergeSound()
    {
        gameSounds.PlayOneShot(mergeSound);
    }

    public void PlayErrorSound()
    {
        gameSounds.PlayOneShot(errorSound);
    }
}
