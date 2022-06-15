using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;
using CLPro.Fixtures;
using System.Text.RegularExpressions;
namespace CLPro.Cue
{
    public class CueStack
    {
        private List<Cue> Cues;
        private int ActiveCue;

        public CueStack(string name)
        {
            this.Cues = new List<Cue>();

            this.ActiveCue = 0;
            PopulateCueStack();
        }

        public void SaveCueStack()
        {
            string file = Show.Show.GetActiveShowFile();
        }

        private void PopulateCueStack()
        {
            string file = Show.Show.GetActiveShowFile();
            XElement show = XElement.Load(file);

            IEnumerable<XElement> cues = show.Descendants("cue");

            foreach(XElement cue in cues)
            {

                int transition = Int32.Parse(cue.Attribute("transition").Value);
                CueType cueType = (CueType)Int32.Parse(cue.Attribute("follow").Value);

                Cue BuildCue = new Cue(transition, 0, cueType);


                IEnumerable<XElement> effects = cue.Descendants("effect");

                foreach (XElement effect in effects)
                {

                    Effects.EffectShape effectShape = Effects.Effect.GetEffectByName(effect.Attribute("shape").Value);
                    int bpm = Int32.Parse(effect.Attribute("BPM").Value);
                    ChannelType EffectType = (ChannelType)Int32.Parse(effect.Attribute("type").Value);

                    IEnumerable<XElement> fixtures = show.Descendants("fixture");

                    List < Fixture > FixList= new();

                    foreach(XElement fixture in fixtures)
                    {
                        string FixName = fixture.Value;
                        
                        FixList.Add(FixtureManager.GetFixtureByID(FixName));
                    }

                    Effects.Effect e = new Effects.Effect(bpm, FixList, EffectType);

                    BuildCue.AddEffect(e);
                
                }

                    Cues.Add(BuildCue);
                
            }

        }

        public void AddNewCue()
        {
            Cues.Add(new Cue());
            GoToNextCue();
        }

        

        public void GoToNextCue()
        {

            Cue PrevCue = GetActiveCue();
            if (ActiveCue == Cues.Count - 1)
                ActiveCue = 0;
            else
            {
                ActiveCue++;
            }
            Cue NextCue = GetActiveCue();

            Transition _Transi = new Transition(PrevCue, NextCue, NextCue.GetTransitionTime(), this);


        }

        public void GoToPrevCue()
        {
            Cue PrevCue = GetActiveCue();
            if (ActiveCue == 0)
                ActiveCue = Cues.Count - 1;
            else
            {
                ActiveCue--;
            }
            Cue NextCue = GetActiveCue();

            Transition _Transi = new Transition(PrevCue, NextCue, NextCue.GetTransitionTime(), this);
        }

        public void SetActiveCue(int CueNumber)
        {
            ActiveCue = CueNumber;
            Cues[ActiveCue].ActivateCue();
        }

        public void SetActiveCue(Cue _cue)
        {
            ActiveCue = Cues.IndexOf(_cue);
            _cue.ActivateCue();
        }

        public Cue GetActiveCue()
        {
            return Cues[ActiveCue];
        }
        public List<Cue> GetCueList()
        {
            return Cues;
        }
        public int GetActiveCueNumber()
        {
            return ActiveCue;
        }

        public void DeleteCue(Cue cue)
        {
            Cues.Remove(cue);
        }

        public void DeleteCue(int index)
        {
            Cues.RemoveAt(index);
        }

        public void InsertNewCue(int insertionIndex)
        {
            Cues.Insert(insertionIndex, new Cue());
        }

        public void InsertCopyOfCue(int insertionIndex, Cue cue)
        {
            Cues.Insert(insertionIndex, cue);
        }

        public void MoveCue(int prevIndex, int newIndex)
        {
            Cue _temp = Cues.ElementAt(prevIndex);
            Cues.RemoveAt(prevIndex);
            Cues.Insert(newIndex, _temp);
        }
    }
}
