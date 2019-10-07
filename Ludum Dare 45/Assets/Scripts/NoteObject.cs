using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoteObject : MonoBehaviour
{
    private GameNote gameNote;

    private static Vector2 start = new Vector2(0, 5.5f);
    private static Vector2 end = new Vector2(0, -4.5f);
    private static Vector2 dist = end - start;

    public AudioClip test;

    private float timeParameter = 0;
    private float audioTime = 0;
    public float NoteTime { get; private set;  }

    private SpriteRenderer renderer;

    private void Awake()
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
        if (timeParameter < 1 && (audioTime - (NoteTime - 2)) / 2 > 1)
        {
         //   renderer.enabled = false;
           //AudioSource.PlayClipAtPoint(test, Vector3.zero);
        }
        this.timeParameter = (audioTime - (NoteTime - 2))/2;
        this.audioTime = audioTime;
    }

    public void setColor(Color color)
    {
        renderer.color = color;
    }

    public GameNote.Accurary checkHit()
    {
        if (gameNote.Hit)
        {
            return GameNote.Accurary.INVALID;
        }

        var accurary = gameNote.CheckHit(audioTime);
        if (accurary != GameNote.Accurary.INVALID)
        {
            StartCoroutine(hitEffect());
            gameNote.Hit = true;
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

    private IEnumerator hitEffect()
    {
        var t = 0f;
        var startColor = renderer.color;
        var fadeColor = new Color(1, .8f, 0, 0);
        while (t < 1)
        {
            t += 4 * Time.deltaTime;
            renderer.color = Color.Lerp(startColor, fadeColor, t);
            yield return new WaitForEndOfFrame();
        }
        renderer.enabled = false;
    }
}

