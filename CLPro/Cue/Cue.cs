using CLPro.Effects;
using CLPro.Fixtures;
using System;
using System.Collections.Generic;

namespace CLPro.Cue
{
    public class Cue
    {
        private int TransitionTime;
        private int FollowTime;
        private CueType _CueType;
        private Dictionary<Fixture, byte[]> FixtureValues;
        private List<Effect> effects;

        public Cue(int transitionTime = 1, int followTime = 0, CueType cueType = CueType.STOP)
        {
            TransitionTime = transitionTime;
            FollowTime = followTime;
            _CueType = cueType;
            FixtureValues = new Dictionary<Fixture, byte[]>();


            foreach (Fixture f in Show.Show.GetFixtures())
            {
                FixtureValues.Add(f, new byte[f.GetNumChannels()]);
            }

            effects = new();
        }

        public void ActivateCue()
        {

            foreach (var kvp in FixtureValues)
            {
                OutputManager.UpdateBuffer(kvp.Key.GetUniverse(), kvp.Value, kvp.Key.GetStartChannel());
                kvp.Key.SetChannelData(kvp.Value);
            }

            foreach (Effect _eff in effects)
            {
                _eff.Begin();
            }
        }

        public void DeactivateCue()
        {
            foreach (Effect _eff in effects)
            {
                _eff.Destroy();
            }
        }

        public void AddEffect(Effect e)
        {
            if (!effects.Contains(e))
            {
                effects.Add(e);
                e.Begin();
            }
        }


        public void SetFixtureValue(Fixture fix, byte[] Data)
        {

            fix.SetChannelData(Data);
            if (!FixtureValues.TryAdd(fix, Data))
            {
                FixtureValues[fix] = Data;
            }



            OutputManager.UpdateBuffer(fix.GetUniverse(), Data, fix.GetStartChannel());
        }

        public byte[] GetFixtureValue(Fixture fix)
        {
            return FixtureValues.GetValueOrDefault(fix);
        }

        public Dictionary<Fixture, byte[]> GetFixtureValues()
        {
            return FixtureValues;
        }

        public int GetTransitionTime()
        {
            return TransitionTime;
        }

        public int GetFollowTime()
        {
            return FollowTime;
        }

        public CueType GetCueType()
        {
            return _CueType;
        }

        public void SetCueType(CueType _cueType)
        {
            _CueType = _cueType;
        }

        public void SetFollowTime(int followTime)
        {
            FollowTime = followTime;
        }

        public void SetTransitionTime(int transitionTime)
        {
            TransitionTime = transitionTime;
        }
    }

    public enum CueType
    {
        FOLLOW, TIMECODE, STOP, MIDI
    }




}
