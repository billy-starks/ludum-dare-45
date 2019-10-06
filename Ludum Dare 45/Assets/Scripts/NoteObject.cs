using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoteObject : MonoBehaviour
{
    private GameNote gameNote;

    private static Vector2 start = new Vector2(0, 5.5f);
    private static Vector2 end = new Vector2(0, -4.0f);
    private static Vector2 dist = end - start;

    private float timeParameter = 0;
    private float audioTime = 0;
    public float NoteTime { get; private set;  }

    private SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(GameNote gameNote)
    {
        transform.localPosition = start;
        this.NoteTime = gameNote.NoteTime;
        this.gameNote = gameNote;
    }

    public void syncAudioTime(float audioTime)
    {
        this.timeParameter = (audioTime - (NoteTime - 2))/2;
        this.audioTime = audioTime;
    }

    public GameNote.Accurary checkHit()
    {
        var accurary = gameNote.CheckHit(audioTime);
        if (accurary != GameNote.Accurary.INVALID)
        {
            gameNote.Hit = true;
            renderer.enabled = false;
            
        }
        Debug.Log(accurary + ": " + this.NoteTime + ": " + audioTime);
        return accurary;
    }

    public bool Hit()
    {
        return gameNote.Hit;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = start + (timeParameter * dist);
    }
}

