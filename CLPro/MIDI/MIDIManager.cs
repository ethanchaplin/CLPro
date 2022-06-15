using Melanchall.DryWetMidi.Multimedia;
using System;

namespace CLPro.MIDI
{
    public static class MIDIManager
    {

        private static IInputDevice inputDevice;

        public static void Begin()
        {



            inputDevice = InputDevice.GetByName("JADY");


            inputDevice.EventReceived += OnEventReceived;
            inputDevice.StartEventsListening();



        }



        private static void OnEventReceived(object sender, MidiEventReceivedEventArgs e)
        {
            var midiDevice = (MidiDevice)sender;
            Console.WriteLine($"Event received from '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");
        }

        private static void OnEventSent(object sender, MidiEventSentEventArgs e)
        {
            var midiDevice = (MidiDevice)sender;
            Console.WriteLine($"Event sent to '{midiDevice.Name}' at {DateTime.Now}: {e.Event}");
        }








    }
}
