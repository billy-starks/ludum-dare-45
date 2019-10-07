using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLane : MonoBehaviour
{
    [SerializeField] private int laneNumber = 1;
    [SerializeField] private Color laneColor = Color.green;
    private GameObject notePrefab;
    private SpriteRenderer renderer;
    private float audioTime;

    public Queue<NoteObject> inactiveNotes;
    public List<NoteObject> activeNotes;

    void Awake()
    {
        notePrefab = Resources.Load<GameObject>("Prefabs/Note");
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void Initialize(List<GameNote> gameNotes)
    {
        List<NoteObject> noteObjects = new List<NoteObject>();
        foreach (GameNote gameNote in gameNotes)
        {
            NoteObject noteObject = Instantiate(notePrefab).GetComponent<NoteObject>();
            noteObject.transform.parent = this.transform;
            noteObject.Initialize(gameNote);
            noteObject.setColor(laneColor);
            noteObjects.Add(noteObject);
        }
        inactiveNotes = new Queue<NoteObject>(noteObjects);
        activeNotes = new List<NoteObject>();
    }

    public void syncAudioTime(float audioTime)
    {
        this.audioTime = audioTime;
        foreach (NoteObject note in activeNotes)
        {
            note.syncAudioTime(audioTime);
        }
    }

    public GameNote.Accurary CheckHit()
    {
        StartCoroutine(fadeColor());

        var accuracy = GameNote.Accurary.INVALID;
        foreach (NoteObject note in activeNotes)
        {
            accuracy = note.checkHit();
            if (accuracy != GameNote.Accurary.INVALID)
            {
                activeNotes.Remove(note);
                break;
            }
        }
        return accuracy;
    }

    public void StartGame(float waitTimeBeforStarting)
    {

    }

    // Update is called once per frame
    void Update()
    {
        while (inactiveNotes.Count > 0 && audioTime + 2 > inactiveNotes.Peek().NoteTime)
        {
            activeNotes.Add(inactiveNotes.Dequeue());
        }

        // Remove missed notes from activeNotes
        while (activeNotes.Count > 0 && audioTime - .6f > activeNotes[0].NoteTime)
        {
            activeNotes.RemoveAt(0);
        }
    }

    private void LoadNextNotes()
    {
        var next = inactiveNotes.Count > 0 ? inactiveNotes.Peek() : null;
        while (next != null && next.NoteTime - audioTime < 1)
        {
            activeNotes.Add(inactiveNotes.Dequeue());
            next = inactiveNotes.Count > 0 ? inactiveNotes.Peek() : null;
        }
    }

    private IEnumerator fadeColor()
    {
        renderer.color = Color.red;
        yield return new WaitForEndOfFrame();
        var t = 1f;
        while(t > 0)
        {
            t -= 4 * Time.deltaTime;
            renderer.color = Color.Lerp(Color.white, Color.red, t);
            yield return new WaitForEndOfFrame();
        }
    }

}
