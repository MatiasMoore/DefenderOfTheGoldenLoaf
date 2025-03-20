using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;

    private string _basePath = "Audio";

    public enum SFX
    {
        testSFX,
        cuttingBoard,
        frog,
        kick,
        win,
        lose,
        orderFinish,
        cooking,
        pickup,
        orderStart,
        shuffle,
        waterSplash1,
        waterSplash2
    }

    static public AudioPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            return;

        Instance = this;
    }

    [ContextMenu("Test Audio")]
    public void TestAudio()
    {
        this.PlaySFX(SFX.testSFX);
    }

    public void PlaySFX(SFX sfx)
    {
        var clip = GetAudioClip(sfx);
        _source.PlayOneShot(clip);
    }

    private AudioClip GetAudioClip(SFX sfx)
    {
        var path = GetPathForSFX(sfx);
        return GetAudioClip(path);
    }

    private string GetPathForSFX(SFX sfx)
    {
        return _basePath + "/" + sfx.ToString();
    }

    private AudioClip GetAudioClip(string path)
    {
        return UnityEngine.Resources.Load<AudioClip>(path);
    }
}