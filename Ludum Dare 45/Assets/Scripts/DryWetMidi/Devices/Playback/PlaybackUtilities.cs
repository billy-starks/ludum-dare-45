﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Smf;
using Melanchall.DryWetMidi.Smf.Interaction;
using Melanchall.DryWetMidi.Standards;

namespace Melanchall.DryWetMidi.Devices
{
    /// <summary>
    /// Contains methods to play MIDI data and retrieving an instance of the <see cref="Playback"/>
    /// which provides advanced features for MIDI data playing.
    /// </summary>
    public static class PlaybackUtilities
    {
        #region Methods

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the specified <see cref="TrackChunk"/>.
        /// </summary>
        /// <param name="trackChunk"><see cref="TrackChunk"/> containing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the <paramref name="trackChunk"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="trackChunk"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static Playback GetPlayback(this TrackChunk trackChunk, TempoMap tempoMap, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(trackChunk), trackChunk);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            return new Playback(trackChunk.Events, tempoMap, outputDevice, clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the specified <see cref="TrackChunk"/>.
        /// </summary>
        /// <param name="trackChunk"><see cref="TrackChunk"/> containing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the <paramref name="trackChunk"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="trackChunk"/> is null. -or-
        /// <paramref name="tempoMap"/> is null.</exception>
        public static Playback GetPlayback(this TrackChunk trackChunk, TempoMap tempoMap, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(trackChunk), trackChunk);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);

            return new Playback(trackChunk.Events, tempoMap, clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the specified collection of <see cref="TrackChunk"/>.
        /// </summary>
        /// <param name="trackChunks">Collection of <see cref="TrackChunk"/> containing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the <paramref name="trackChunks"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="trackChunks"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static Playback GetPlayback(this IEnumerable<TrackChunk> trackChunks, TempoMap tempoMap, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(trackChunks), trackChunks);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            return new Playback(trackChunks.Select(c => c.Events), tempoMap, outputDevice, clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the specified collection of <see cref="TrackChunk"/>.
        /// </summary>
        /// <param name="trackChunks">Collection of <see cref="TrackChunk"/> containing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the <paramref name="trackChunks"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="trackChunks"/> is null. -or-
        /// <paramref name="tempoMap"/> is null.</exception>
        public static Playback GetPlayback(this IEnumerable<TrackChunk> trackChunks, TempoMap tempoMap, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(trackChunks), trackChunks);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);

            return new Playback(trackChunks.Select(c => c.Events), tempoMap, clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the specified <see cref="MidiFile"/>.
        /// </summary>
        /// <param name="midiFile"><see cref="MidiFile"/> containing events to play.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the <paramref name="midiFile"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="midiFile"/> is null. -or-
        /// <paramref name="outputDevice"/> is null.</exception>
        public static Playback GetPlayback(this MidiFile midiFile, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(midiFile), midiFile);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            return GetPlayback(midiFile.GetTrackChunks(), midiFile.GetTempoMap(), outputDevice, clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the specified <see cref="MidiFile"/>.
        /// </summary>
        /// <param name="midiFile"><see cref="MidiFile"/> containing events to play.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events contained in
        /// the <paramref name="midiFile"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="midiFile"/> is null.</exception>
        public static Playback GetPlayback(this MidiFile midiFile, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(midiFile), midiFile);

            return GetPlayback(midiFile.GetTrackChunks(), midiFile.GetTempoMap(), clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events that will be
        /// produced by specified <see cref="Pattern"/>.
        /// </summary>
        /// <param name="pattern"><see cref="Pattern"/> producing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="channel">MIDI channel to play channel events on.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events that will be
        /// produced by the <paramref name="pattern"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static Playback GetPlayback(this Pattern pattern, TempoMap tempoMap, FourBitNumber channel, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(pattern), pattern);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            return pattern.ToTrackChunk(tempoMap, channel).GetPlayback(tempoMap, outputDevice, clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing MIDI events that will be
        /// produced by specified <see cref="Pattern"/>.
        /// </summary>
        /// <param name="pattern"><see cref="Pattern"/> producing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="channel">MIDI channel to play channel events on.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing MIDI events that will be
        /// produced by the <paramref name="pattern"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null. -or-
        /// <paramref name="tempoMap"/> is null.</exception>
        public static Playback GetPlayback(this Pattern pattern, TempoMap tempoMap, FourBitNumber channel, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(pattern), pattern);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);

            return pattern.ToTrackChunk(tempoMap, channel).GetPlayback(tempoMap, clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing musical objects using
        /// the specified program.
        /// </summary>
        /// <typeparam name="TObject">The type of objects to play.</typeparam>
        /// <param name="objects">Objects to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play <paramref name="objects"/> through.</param>
        /// <param name="programNumber">Program that should be used to play <paramref name="objects"/>.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing <paramref name="objects"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="objects"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static Playback GetPlayback<TObject>(this IEnumerable<TObject> objects, TempoMap tempoMap, OutputDevice outputDevice, SevenBitNumber programNumber, MidiClockSettings clockSettings = null)
            where TObject : IMusicalObject, ITimedObject
        {
            ThrowIfArgument.IsNull(nameof(objects), objects);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            return GetMusicalObjectsPlayback(objects,
                                             tempoMap,
                                             outputDevice,
                                             channel => new[] { new ProgramChangeEvent(programNumber) { Channel = channel } },
                                             clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing musical objects using
        /// the specified General MIDI 1 program.
        /// </summary>
        /// <typeparam name="TObject">The type of objects to play.</typeparam>
        /// <param name="objects">Objects to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play <paramref name="objects"/> through.</param>
        /// <param name="generalMidiProgram">Program that should be used to play <paramref name="objects"/>.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing <paramref name="objects"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="objects"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="generalMidiProgram"/> specified an invalid value.</exception>
        public static Playback GetPlayback<TObject>(this IEnumerable<TObject> objects, TempoMap tempoMap, OutputDevice outputDevice, GeneralMidiProgram generalMidiProgram, MidiClockSettings clockSettings = null)
            where TObject : IMusicalObject, ITimedObject
        {
            ThrowIfArgument.IsNull(nameof(objects), objects);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);
            ThrowIfArgument.IsInvalidEnumValue(nameof(generalMidiProgram), generalMidiProgram);

            return GetMusicalObjectsPlayback(objects,
                                             tempoMap,
                                             outputDevice,
                                             channel => new[] { generalMidiProgram.GetProgramEvent(channel) },
                                             clockSettings);
        }

        /// <summary>
        /// Retrieves an instance of the <see cref="Playback"/> for playing musical objects using
        /// the specified General MIDI 2 program.
        /// </summary>
        /// <typeparam name="TObject">The type of objects to play.</typeparam>
        /// <param name="objects">Objects to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play <paramref name="objects"/> through.</param>
        /// <param name="generalMidi2Program">Program that should be used to play <paramref name="objects"/>.</param>
        /// <returns>An instance of the <see cref="Playback"/> for playing <paramref name="objects"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="objects"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="generalMidi2Program"/> specified an invalid value.</exception>
        public static Playback GetPlayback<TObject>(this IEnumerable<TObject> objects, TempoMap tempoMap, OutputDevice outputDevice, GeneralMidi2Program generalMidi2Program, MidiClockSettings clockSettings = null)
            where TObject : IMusicalObject, ITimedObject
        {
            ThrowIfArgument.IsNull(nameof(objects), objects);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);
            ThrowIfArgument.IsInvalidEnumValue(nameof(generalMidi2Program), generalMidi2Program);

            return GetMusicalObjectsPlayback(objects,
                                             tempoMap,
                                             outputDevice,
                                             channel => generalMidi2Program.GetProgramEvents(channel),
                                             clockSettings);
        }

        /// <summary>
        /// Plays MIDI events contained in the specified <see cref="TrackChunk"/>.
        /// </summary>
        /// <param name="trackChunk"><see cref="TrackChunk"/> containing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <exception cref="ArgumentNullException"><paramref name="trackChunk"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static void Play(this TrackChunk trackChunk, TempoMap tempoMap, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(trackChunk), trackChunk);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            using (var playback = trackChunk.GetPlayback(tempoMap, outputDevice, clockSettings))
            {
                playback.Play();
            }
        }

        /// <summary>
        /// Plays MIDI events contained in the specified collection of <see cref="TrackChunk"/>.
        /// </summary>
        /// <param name="trackChunks">Collection of <see cref="TrackChunk"/> containing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <exception cref="ArgumentNullException"><paramref name="trackChunks"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static void Play(this IEnumerable<TrackChunk> trackChunks, TempoMap tempoMap, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(trackChunks), trackChunks);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            using (var playback = trackChunks.GetPlayback(tempoMap, outputDevice, clockSettings))
            {
                playback.Play();
            }
        }

        /// <summary>
        /// Plays MIDI events contained in the specified <see cref="MidiFile"/>.
        /// </summary>
        /// <param name="midiFile"><see cref="MidiFile"/> containing events to play.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <exception cref="ArgumentNullException"><paramref name="midiFile"/> is null. -or-
        /// <paramref name="outputDevice"/> is null.</exception>
        public static void Play(this MidiFile midiFile, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(midiFile), midiFile);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            midiFile.GetTrackChunks().Play(midiFile.GetTempoMap(), outputDevice, clockSettings);
        }

        /// <summary>
        /// Plays MIDI events that will be produced by specified <see cref="Pattern"/>.
        /// </summary>
        /// <param name="pattern"><see cref="Pattern"/> producing events to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="channel">MIDI channel to play channel events on.</param>
        /// <param name="outputDevice">Output MIDI device to play events through.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static void Play(this Pattern pattern, TempoMap tempoMap, FourBitNumber channel, OutputDevice outputDevice, MidiClockSettings clockSettings = null)
        {
            ThrowIfArgument.IsNull(nameof(pattern), pattern);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            pattern.ToTrackChunk(tempoMap, channel).Play(tempoMap, outputDevice, clockSettings);
        }

        /// <summary>
        /// Plays musical objects using the specified program.
        /// </summary>
        /// <typeparam name="TObject">The type of objects to play.</typeparam>
        /// <param name="objects">Objects to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play <paramref name="objects"/> through.</param>
        /// <param name="programNumber">Program that should be used to play <paramref name="objects"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="objects"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        public static void Play<TObject>(this IEnumerable<TObject> objects, TempoMap tempoMap, OutputDevice outputDevice, SevenBitNumber programNumber, MidiClockSettings clockSettings = null)
            where TObject : IMusicalObject, ITimedObject
        {
            ThrowIfArgument.IsNull(nameof(objects), objects);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);

            using (var playback = objects.GetPlayback(tempoMap, outputDevice, programNumber, clockSettings))
            {
                playback.Play();
            }
        }

        /// <summary>
        /// Plays musical objects using the specified General MIDI 1 program.
        /// </summary>
        /// <typeparam name="TObject">The type of objects to play.</typeparam>
        /// <param name="objects">Objects to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play <paramref name="objects"/> through.</param>
        /// <param name="generalMidiProgram">Program that should be used to play <paramref name="objects"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="objects"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="generalMidiProgram"/> specified an invalid value.</exception>
        public static void Play<TObject>(this IEnumerable<TObject> objects, TempoMap tempoMap, OutputDevice outputDevice, GeneralMidiProgram generalMidiProgram, MidiClockSettings clockSettings = null)
            where TObject : IMusicalObject, ITimedObject
        {
            ThrowIfArgument.IsNull(nameof(objects), objects);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);
            ThrowIfArgument.IsInvalidEnumValue(nameof(generalMidiProgram), generalMidiProgram);

            using (var playback = objects.GetPlayback(tempoMap, outputDevice, generalMidiProgram, clockSettings))
            {
                playback.Play();
            }
        }

        /// <summary>
        /// Plays musical objects using the specified General MIDI 2 program.
        /// </summary>
        /// <typeparam name="TObject">The type of objects to play.</typeparam>
        /// <param name="objects">Objects to play.</param>
        /// <param name="tempoMap">Tempo map used to calculate events times.</param>
        /// <param name="outputDevice">Output MIDI device to play <paramref name="objects"/> through.</param>
        /// <param name="generalMidi2Program">Program that should be used to play <paramref name="objects"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="objects"/> is null. -or-
        /// <paramref name="tempoMap"/> is null. -or- <paramref name="outputDevice"/> is null.</exception>
        /// <exception cref="InvalidEnumArgumentException"><paramref name="generalMidi2Program"/> specified an invalid value.</exception>
        public static void Play<TObject>(this IEnumerable<TObject> objects, TempoMap tempoMap, OutputDevice outputDevice, GeneralMidi2Program generalMidi2Program, MidiClockSettings clockSettings = null)
            where TObject : IMusicalObject, ITimedObject
        {
            ThrowIfArgument.IsNull(nameof(objects), objects);
            ThrowIfArgument.IsNull(nameof(tempoMap), tempoMap);
            ThrowIfArgument.IsNull(nameof(outputDevice), outputDevice);
            ThrowIfArgument.IsInvalidEnumValue(nameof(generalMidi2Program), generalMidi2Program);

            using (var playback = objects.GetPlayback(tempoMap, outputDevice, generalMidi2Program, clockSettings))
            {
                playback.Play();
            }
        }

        private static Playback GetMusicalObjectsPlayback<TObject>(IEnumerable<TObject> objects,
                                                                   TempoMap tempoMap,
                                                                   OutputDevice outputDevice,
                                                                   Func<FourBitNumber, IEnumerable<MidiEvent>> programChangeEventsGetter,
                                                                   MidiClockSettings clockSettings)
            where TObject : IMusicalObject, ITimedObject
        {
            var programChangeEvents = objects.Select(n => n.Channel)
                                             .Distinct()
                                             .SelectMany(programChangeEventsGetter)
                                             .Select(e => (ITimedObject)new TimedEvent(e));

            return new Playback(programChangeEvents.Concat((IEnumerable<ITimedObject>)objects), tempoMap, outputDevice, clockSettings);
        }

        #endregion
    }
}
