using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RhythmController : MonoBehaviour
{
    // Data provided from the Unity UI
    private AudioSource audioSource;
    [SerializeField] private string songName;
    [SerializeField] private AudioClip clapSfx;

    // List of all notes in the midi data
    List<GameNote> allNotes;

    // List of list of notes
    // Each inner list contains all the notes for a single lane
    List<List<GameNote>> notesInLanes;


    GameObject noteBoard;
    NoteLane[] noteLanes = new NoteLane[4];

    private float sampleRate;
    private Tempo startingTempo;
    private float noteAccurary;
    private int notesPassed;

    private KeyCode key1 = KeyCode.D;
    private KeyCode key2 = KeyCode.F;
    private KeyCode key3 = KeyCode.J;
    private KeyCode key4 = KeyCode.K;

    private const float COMPARISON_WINDOW = .3F;
    private const float GREAT = COMPARISON_WINDOW * .6F;
    private const float GOOD = COMPARISON_WINDOW * .7F;
    private const float OK = COMPARISON_WINDOW * .75F;

    // Song maps are encoded as midi data
    // A C is treated as a note in lane 1, A D as a note in lane 2, etc.
    NoteName[] laneNoteNames = { NoteName.C, NoteName.D, NoteName.E, NoteName.F };

    void Start()
    {
        LoadSong(songName);
        SetupNoteBoard();
        StartCoroutine(StartSong(3));
    }

    private IEnumerator StartSong(float delay)
    {
        audioSource.Stop();
        AudioSource.PlayClipAtPoint(clapSfx, Vector3.zero);
        float fakeAudioTime = -delay;
        while (fakeAudioTime < 0)
        {
            fakeAudioTime += Time.deltaTime;
            foreach (NoteLane lane in noteLanes)
            {
                lane.syncAudioTime(fakeAudioTime);
            }
            yield return new WaitForEndOfFrame();
        }
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (audioSource.isPlaying) { 
            // Calculate audio time in seconds from sample count
            float audioTime = audioSource.timeSamples / sampleRate;

            foreach (NoteLane lane in noteLanes)
            {
                lane.syncAudioTime(audioTime);
            }
        }

        // TODO: Check input; send correct hit to lane
        if (Input.GetKeyDown(key1))
        {
            noteLanes[0].CheckHit();
        }
        if (Input.GetKeyDown(key2))
        {
            noteLanes[1].CheckHit();
        }
        if (Input.GetKeyDown(key3))
        {
            noteLanes[2].CheckHit();
        }
        if (Input.GetKeyDown(key4))
        {
            noteLanes[3].CheckHit();
        }
    }

    private float timeInSeconds(MetricTimeSpan metricTimeSpan)
    {
        return metricTimeSpan.TotalMicroseconds / 1000000f;
    }

    public void LoadSong(String songName)
    {
        // Load audio clip (from "Assets/Resources/Music")
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = Resources.Load<AudioClip>("Music/" + songName);

        // Load midi data (from "Assets/Resources/MusicData")
        notesInLanes = new List<List<GameNote>>();
        var midiFile = MidiFile.Read(Application.dataPath + "/Resources/MusicData/" + songName + ".mid");
        var tempoMap = midiFile.GetTempoMap();

        // Load midi data into note names and note hit times
        allNotes = new List<GameNote>();
        foreach (var note in midiFile.GetNotes())
        {
            NoteName noteName = note.NoteName;
            MetricTimeSpan metricTimeSpan = note.TimeAs<MetricTimeSpan>(tempoMap);
            allNotes.Add(new GameNote(noteName, timeInSeconds(metricTimeSpan)));
        }

        for (int i = 0; i < 4; i++)
        {
            List<GameNote> laneNotes = allNotes.Where(note => note.NoteName == laneNoteNames[i]).ToList();
            notesInLanes.Add(laneNotes);
        }

        sampleRate = audioSource.clip.frequency;
        startingTempo = tempoMap.Tempo.AtTime(0);
    }

    public List<GameNote> GetLaneNotes(int laneNumber)
    {
        return notesInLanes[laneNumber];
    }


    private void SetupNoteBoard()
    {
        noteBoard = GameObject.FindGameObjectWithTag("Noteboard");
        for (int i = 0; i < noteBoard.transform.childCount; i++)
        {
            NoteLane lane = noteBoard.transform.GetChild(i).GetComponent<NoteLane>();
            lane.Initialize(notesInLanes[i]);
            noteLanes[i] = lane;
        }
    }
}
