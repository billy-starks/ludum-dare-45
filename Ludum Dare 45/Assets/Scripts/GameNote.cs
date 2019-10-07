using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameNote
{
    public NoteName NoteName { get; }
    public float NoteTime { get; }

    public bool Hit { get; set;  }

    private const float COMPARISON_WINDOW = .175F;
    private const float GREAT = COMPARISON_WINDOW * .7F;
    private const float GOOD = COMPARISON_WINDOW * .85F;

    public enum Accurary {GREAT, GOOD, OK, INVALID = -1};

    public GameNote(NoteName noteName, float noteTime)
    {
        this.NoteName = noteName;
        this.NoteTime = noteTime;
        this.Hit = false;
    }

    public Accurary CheckHit(float audioTime)
    {
        float timeDiff = Mathf.Abs(audioTime - NoteTime);

        // Check if within comparison window
        if (timeDiff < COMPARISON_WINDOW)
        {
            if (timeDiff < GREAT)
            {
                return Accurary.GREAT;
            }
            else if (timeDiff < GOOD)
            {
                return Accurary.GOOD;
            }
            else { 
                return Accurary.OK;
            }
        }

        // Out of comparison time window
        return Accurary.INVALID;
    }

}