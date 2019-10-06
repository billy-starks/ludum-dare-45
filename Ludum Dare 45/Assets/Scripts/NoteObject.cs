using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoteObject : MonoBehaviour
{
    private GameNote gameNote;

    private static Vector2 start = new Vector2(0, 5.5f);
    private static Vector2 end = new Vector2(0, -4.5f);
    private static Vector2 dist = end - start;

    private float timeParameter = 0;
    public float NoteTime { get; private set;  }

    public void Initialize(float noteTime)
    {
        transform.localPosition = start;
        this.NoteTime = noteTime;
    }

    public void syncAudioTime(float audioTime)
    {
        this.timeParameter = Mathf.Clamp(audioTime - (NoteTime - 1), 0, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = start + (timeParameter * dist);
    }
}

