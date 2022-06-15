using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLPro
{
    public class OutputDevice
    {

        private OutputDeviceType _type;
        private List<int> universeRouting = new();
        private SerialPort _serialPort;

        public OutputDevice(string port, OutputDeviceType type)
        {
            _type = type;

            for (int i = 1; i < (int)type + 1; i++)
            {
                universeRouting.Add(-i);
            }

            _serialPort = new SerialPort();

            _serialPort.PortName = port;
            _serialPort.BaudRate = 3000000;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;



            _serialPort.ReadTimeout = 10000;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();


        }


        public SerialPort GetSerialPort()
        {
            return _serialPort;
        }


        public void RouteUniverse(int realUniv, int outUniv)
        {

            universeRouting[outUniv - 1] = realUniv;
        }

        public OutputDeviceType GetDeviceType()
        {
            return _type;
        }

        public List<int> GetUniverseRouting()
        {
            return universeRouting;
        }

        public void SetUniverseRouting(List<int> routing)
        {
            universeRouting = routing;
        }

        public void SetUniverseRouting(int realUniverse, int outUniverse)
        {
            universeRouting[outUniverse - 1] = realUniverse;
        }


    }

    public enum OutputDeviceType
    {
        CL_4 = 4, CL_8 = 8, CL_16 = 16
    }
}
