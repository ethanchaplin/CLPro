using CLPro.Fixtures;
using System.Collections.Generic;
namespace CLPro.Show
{
    public static class Show
    {
        private static string Name = "";
        private static string ActiveShowFile = "";
        private static List<Fixture> AllFixtures = new();

        public static void LoadShow(string file)
        {
            ActiveShowFile = file;
        }

        public static string GetName()
        {
            return Name;
        }

        public static string GetActiveShowFile()
        {
            return ActiveShowFile;
        }

        public static void SaveShow()
        {
            //TODO Show Saving stuff





        }

        public static void RegisterFixture(Fixture fix)
        {
            AllFixtures.Add(fix);
        }

        public static List<Fixture> GetFixtures()
        {
            return AllFixtures;
        }
    }

}
