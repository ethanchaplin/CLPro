using CLPro.Fixtures;
using System;
using System.Collections.Generic;

namespace CLPro.Effects
{
    public class Effect
    {

        private List<Fixture> _FixtureList;
        private int BPM;
        private ChannelType _EffectType;
        private EffectShape _EffectShape;
        private System.Timers.Timer TickTimer;
        private int _TickCount;
        private int TicksPerCycle;
        private int Stagger;
        public Effect(int BPM, List<Fixture> FixtureList, ChannelType EffectType, EffectShape effectShape = EffectShape.SINE)
        {
            _FixtureList = FixtureList;
            this.BPM = BPM;
            _EffectType = EffectType;
            this._EffectShape = effectShape;
            TickTimer = new System.Timers.Timer(OutputManager.GetRefreshMillis());
            _TickCount = 0;

            TicksPerCycle = (1000 / OutputManager.GetRefreshMillis()) / (this.BPM / 60);

            Stagger = 100;


            TickTimer.AutoReset = true;
            TickTimer.Elapsed += Tick;


        }

        public void Destroy()
        {
            TickTimer.Enabled = false;



        }
        public void Begin()
        {

            TickTimer.Enabled = true;
        }
        public void Pause()
        {
            TickTimer.Stop();
        }

        public static EffectShape GetEffectByName(string name)
        {
            List<string> types = new List<string>(new string[] { "sine", "square", "ramp", "sawtooth" } );
            return (EffectShape)types.IndexOf(name.ToLower());
        }

        public void Tick(object sender, EventArgs e)
        {
            _TickCount++;
            if (_TickCount == TicksPerCycle)
            {
                _TickCount = 0;
            }

            foreach (Fixture fixture in _FixtureList)
            {

                byte val = 0;

                switch (_EffectShape)
                {
                    case EffectShape.SINE:
                        val = (byte)((127.5) * (Math.Sin(((1 / ((TicksPerCycle / 2) / Math.PI)) * _TickCount) + ((2 * Math.PI)) * _FixtureList.IndexOf(fixture) / _FixtureList.Count) + 255 - (127.5)));
                        break;
                    case EffectShape.SQUARE:
                        val = ((_TickCount + ((_FixtureList.IndexOf(fixture) / (_FixtureList.Count * 4)) * TicksPerCycle)) % TicksPerCycle > (TicksPerCycle / 2)) ? (byte)255 : (byte)0;
                        break;
                    case EffectShape.SAWTOOTH:
                        val = (byte)((255 * _TickCount) / TicksPerCycle);
                        break;
                }
                OutputManager.UpdateBuffer(fixture.GetUniverse(), fixture.GetChannelByChannelType(_EffectType) + (fixture.GetStartChannel() - 1), val);

            }










        }

        public int GetBPM()
        {
            return BPM;
        }

        public void SetBPM(int BPM)
        {
            this.BPM = BPM;
        }

        public EffectShape GetEffectShape()
        {
            return this._EffectShape;
        }

        public void SetEffectShape(EffectShape effectShape)
        {
            this._EffectShape = effectShape;
        }

    }

    public enum EffectShape
    {
        SINE, SQUARE, RAMP, SAWTOOTH
    }


}
