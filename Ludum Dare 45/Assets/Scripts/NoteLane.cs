using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLane : MonoBehaviour
{
    [SerializeField] private int laneNumber = 1;
    private GameObject notePrefab;
    private float audioTime;

    public Queue<NoteObject> inactiveNotes;
    public List<NoteObject> activeNotes;

    void Awake()
    {
        notePrefab = Resources.Load<GameObject>("Prefabs/Note");
    }

    public void Initialize(List<GameNote> gameNotes)
    {
        List<NoteObject> noteObjects = new List<NoteObject>();
        foreach (GameNote gameNote in gameNotes)
        {
            NoteObject noteObject = Instantiate(notePrefab).GetComponent<NoteObject>();
            noteObject.transform.parent = this.transform;
            noteObject.Initialize(gameNote.NoteTime);
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

    public void StartGame(float waitTimeBeforStarting)
    {

    }

    // Update is called once per frame
    void Update()
    {
        while (inactiveNotes.Count > 0 && audioTime + 1 > inactiveNotes.Peek().NoteTime)
        {
            activeNotes.Add(inactiveNotes.Dequeue());
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
}
