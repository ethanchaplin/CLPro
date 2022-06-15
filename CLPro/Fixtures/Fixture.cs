using System.Collections.Generic;

namespace CLPro.Fixtures
{
    public class Fixture
    {
        private int Universe;
        private int NumChannels;
        private int StartChannel;
        private string id;
        private List<ChannelType> ChannelTypes;
        private byte[] ChannelData;

        public Fixture(int Universe, int StartChannel, string FixtureFile = "default", string id = "unnamed")
        {
            this.Universe = Universe;
            this.StartChannel = StartChannel;
            this.ChannelTypes = new List<ChannelType>();
            this.ChannelData = new byte[NumChannels];

            SetFixtureType(FixtureFile);
            SetID(id);

            FixtureManager.AddFixture(this, this.id);

        }

        public int GetUniverse()
        {
            return Universe;
        }

        public void SetUniverse(int Universe)
        {
            this.Universe = Universe;
        }

        public void SetNumChannels(int NumChannels)
        {
            this.NumChannels = NumChannels;
            List<byte> temp = new(this.ChannelData);
            while (this.NumChannels > this.ChannelTypes.Count)
            {
                this.ChannelTypes.Add(ChannelType.INTENSITY);
            }

            while (this.NumChannels > temp.Count)
            {
                temp.Add(0);
            }
            this.ChannelData = temp.ToArray();
        }

        public int GetNumChannels()
        {
            return NumChannels;
        }

        public int GetStartChannel()
        {
            return StartChannel;
        }

        public void SetFixtureType(string FixtureFile)
        {
            if (FixtureFile != "default")
            {
                LoadDataFromFile();
            }
            else
            {
                this.NumChannels = 1;
                this.ChannelTypes.Add(ChannelType.INTENSITY);
            }
        }

        public void SetStartChannel(int channel)
        {
            this.StartChannel = channel;
        }

        public void SetChannelType(int channel, ChannelType type)
        {
            ChannelTypes[channel - 1] = type;
        }

        public string GetID()
        {
            return this.id;
        }

        public void SetID(string id)
        {
            if (FixtureManager.FixtureExists(id))
            {
                this.id = id + "_01";
            }
            else
            {
                this.id = id;
            }
        }

        public ChannelType GetChannelType(int channel)
        {
            return ChannelTypes[channel - 1];
        }

        public byte[] GetChannelData()
        {
            return ChannelData;
        }

        public byte GetIndividualChannelData(int Channel)
        {
            return ChannelData[Channel - 1];
        }

        public void SetChannelData(byte[] Data)
        {
            ChannelData = Data;
        }

        public void SetIndividualChannelData(int Channel, byte Data)
        {
            ChannelData[Channel - 1] = Data;
        }

        private void LoadDataFromFile()
        {
            //TODO Implement XML File parsing for fixture data
        }

        public int GetChannelByChannelType(ChannelType _type)
        {
            return ChannelTypes.IndexOf(_type) + 1;
        }


    }

    public enum ChannelType
    {
        INTENSITY, RED, GREEN, BLUE, CYAN, MAGENTA, YELLOW, PAN, TILT, PAN_FINE, TILT_FINE, STROBE, CUSTOM
    }

}
