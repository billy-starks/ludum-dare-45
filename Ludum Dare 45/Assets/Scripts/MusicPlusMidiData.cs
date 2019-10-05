using Melanchall.DryWetMidi.MusicTheory;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlusMidiData : MonoBehaviour
{
    // Data provided from the Unity UI
    private AudioSource audioSource;
    [SerializeField] private string midiDataPath;

    // Queue stores (note name, note times) pairs
    private Queue<Tuple<NoteName, float>> notesQueue;

    // Active note (only one lane for now)
    private Tuple<NoteName, float> activeNote;

    private float sampleRate;

    private const float COMPARISON_WINDOW = .4F;
    private const float GREAT = COMPARISON_WINDOW * .375F;
    private const float GOOD = COMPARISON_WINDOW * .5F;
    private const float OK = COMPARISON_WINDOW * .8F;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        notesQueue = new Queue<Tuple<NoteName, float>>();
        var midiFile = MidiFile.Read(Application.dataPath + "/" + midiDataPath);
        var tempoMap = midiFile.GetTempoMap();


        // Load midi data into note names and note hit times
        foreach(var note in midiFile.GetNotes())
        {
            NoteName noteName = note.NoteName;
            MetricTimeSpan metricTimeSpan = note.TimeAs<MetricTimeSpan>(tempoMap);
            notesQueue.Enqueue(new Tuple<NoteName, float>(noteName, timeInSeconds(metricTimeSpan)));
        }

        sampleRate = audioSource.clip.frequency;
        Debug.Log(sampleRate);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float audioTime = audioSource.timeSamples / sampleRate;
        if (activeNote == null && notesQueue.Count > 0)
        {
            activeNote = notesQueue.Dequeue();
        }

        if (Input.anyKeyDown)
        {
            checkHit(audioTime);
        }

        //Clear active note if passed
        if (activeNote != null && audioTime - COMPARISON_WINDOW > activeNote.Item2)
        {
            Debug.Log("MISS");
            activeNote = null;
        }
    }

    private float timeInSeconds(MetricTimeSpan metricTimeSpan)
    {
        return metricTimeSpan.TotalMicroseconds / 1000000f;
    }

    private void checkHit(float audioTime)
    {
        if (audioSource.isPlaying && activeNote != null)
        {
            float timeDiff = Mathf.Abs(audioTime - activeNote.Item2);

            // Check if within comparison window
            if (timeDiff < COMPARISON_WINDOW)
            {

                if (timeDiff < GREAT)        // Great timing
                {
                    Debug.Log("Great!");
                }
                else if (timeDiff < GOOD)   // Good timing
                {
                    Debug.Log("Good!");
                }
                else if (timeDiff < OK)   // OK timing
                {
                    Debug.Log("OK");
                }
                else                        // Too early/late
                {
                    Debug.Log("MISS");
                }
                Debug.Log(audioTime + "  :  " + activeNote.Item2);
                activeNote = null;
            }
        }
    }
}
