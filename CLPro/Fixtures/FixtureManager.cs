using System.Collections.Generic;

namespace CLPro.Fixtures
{
    public static class FixtureManager
    {
        private static Dictionary<string, Fixture> FixtureList = new();


        public static Fixture GetFixtureByID(string id)
        {
            return FixtureList[id];
        }

        public static bool FixtureExists(string id)
        {
            return FixtureList.ContainsKey(id);
        }

        public static void RemoveFixture(string id)
        {
            FixtureList.Remove(id);
        }
        public static void AddFixture(Fixture fix, string id)
        {
            FixtureList.Add(id, fix);
        }



    }
}
