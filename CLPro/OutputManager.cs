using System;
using System.Collections.Generic;
using System.IO.Ports;



namespace CLPro
{
    public static class OutputManager
    {
        private static Dictionary<int, OutputDevice> _outputDevices = new();
        
        private static volatile int RefreshMillis = 40;
        
        private static volatile Dictionary<int, byte[]> universes = new();

        private static System.Timers.Timer aTimer = new();
        
        

        public static void Init()
        {
            aTimer.Interval = RefreshMillis;

            aTimer.Elapsed += PushUniverses;


            aTimer.AutoReset = true;

            aTimer.Enabled = true;
        }


        public static void AddOutputDevice(OutputDevice _out)
        {


            foreach (int realUniv in _out.GetUniverseRouting())
            {
                _outputDevices.Add(realUniv, _out);
            }


        }
        


        public static void UpdateBuffer(int universe, int channel, byte data)
        {
            universes[universe][channel - 1] = data;

        }

        public static void UpdateBuffer(int universe, byte[] data, int startChannel)
        {

            data.CopyTo(universes[universe], startChannel - 1);

        }
        

        
        public static void Close(OutputDevice _out)
        {
            _out.GetSerialPort().Close();
        }

        public static int GetRefreshMillis()
        {
            return RefreshMillis;
        }
        public static byte[] GetUniverseData(int universe)
        {
            return universes[universe];
        }
        public static void SetRefreshMillis(int _RefreshMillis)
        {
            RefreshMillis = _RefreshMillis;
        }


        private static void PushUniverses(Object source, System.Timers.ElapsedEventArgs e)
        {

            byte[] message = new byte[518];
            message[0] = 0x7E;
            
            message[2] = (byte)((513) & 0xFF);
            message[3] = (byte)((512 >> 8) & 0xFF);
            message[4] = 0x00;

            message[517] = 0xE7;

            for (int universe = 1; universe < universes.Count + 1; universe++)
            {
                message[1] = (byte)(100 + universe - 1);
                universes[universe].CopyTo(message, 5);
                _outputDevices[universe].GetSerialPort().Write(message, 0, message.Length);
            }
        }





    }



}
