using System;
using System.Collections.Generic;
namespace CLPro.Cue
{
    public class Transition
    {

        private readonly Cue _Begin;
        private readonly Cue _End;
        private readonly int _Duration;
        private readonly CueStack _Caller;
        private System.Timers.Timer _TransitionTimer;
        private readonly int _TransitionFrameCount;
        private int _CurrentFrame = 0;
        private readonly List<int> _FixtureChannels;
        private readonly List<int> _ChannelUniverses;
        private readonly List<byte> _BeforeData;
        private readonly List<byte> _EndData;
        private readonly List<float> _CurrentData;
        private readonly List<float> _DataIncrement;
        public Transition(Cue Begin, Cue End, int Duration, CueStack caller)
        {
            _Begin = Begin;
            _End = End;
            _Duration = Duration;
            _Caller = caller;
            _TransitionTimer = new System.Timers.Timer(OutputManager.GetRefreshMillis());


            _TransitionFrameCount = ((1000 * _Duration) / OutputManager.GetRefreshMillis());

            _FixtureChannels = new();
            _ChannelUniverses = new();
            _BeforeData = new();
            _EndData = new();
            _CurrentData = new();
            _DataIncrement = new();

            PopulateDataArrays();
            CalculateDataIncrement();


            _Begin.DeactivateCue();

            _TransitionTimer.AutoReset = true;
            _TransitionTimer.Elapsed += PushTransition;
            _TransitionTimer.Enabled = true;
        }

        private void PopulateDataArrays()
        {






            foreach (var kvp in _Begin.GetFixtureValues())
            {
                int count = 0;
                foreach (byte b in kvp.Value)
                {
                    _BeforeData.Add(b);
                    _CurrentData.Add((float)b);
                    _ChannelUniverses.Add(kvp.Key.GetUniverse());
                    _FixtureChannels.Add(count + kvp.Key.GetStartChannel());
                    count++;
                }




            }
            foreach (var kvp in _End.GetFixtureValues())
            {
                foreach (byte b in kvp.Value)
                {
                    _EndData.Add(b);
                }
            }
        }
        public Cue GetStartCue()
        {
            return _Begin;
        }

        public Cue GetEndCue()
        {
            return _End;
        }

        public int GetDuration()
        {
            return _Duration;
        }

        public CueStack GetCueStack()
        {
            return _Caller;
        }

        public void Finish()
        {
            
            _TransitionTimer.Stop();
            _TransitionTimer.Dispose();
            _Caller.SetActiveCue(_End);



            if (_End.GetCueType() == CueType.FOLLOW)
            {
                _Caller.GoToNextCue();
            }
        }

        public void Pause()
        {
            _TransitionTimer.Stop();
        }

        public void Resume()
        {
            _TransitionTimer.Start();
        }

        public void PushTransition(Object source, System.Timers.ElapsedEventArgs e)
        {

            CalculateCurrentData();

            _CurrentFrame++;

            for (int i = 0; i < _CurrentData.Count; i++)
            {
                OutputManager.UpdateBuffer(_ChannelUniverses[i], _FixtureChannels[i], (byte)_CurrentData[i]);



            }

            if (_CurrentFrame == _TransitionFrameCount - 1)
            {
                Finish();
            }
        }

        private void CalculateDataIncrement()
        {
            while (_EndData.Count > _BeforeData.Count)
            {
                _BeforeData.Add(0);
            }
            while (_EndData.Count < _BeforeData.Count)
            {
                _EndData.Add(0);
            }

            for (int i = 0; i < _EndData.Count; i++)
            {

                _DataIncrement.Add((float)(_EndData[i] - _BeforeData[i]) / (float)_TransitionFrameCount);
            }
        }

        private void CalculateCurrentData()
        {
            if (_DataIncrement.Count > 0)
            {
                for (int i = 0; i < _CurrentData.Count; i++)
                {
                    _CurrentData[i] = _CurrentData[i] + _DataIncrement[i];
                }
            }
        }
    }
}
