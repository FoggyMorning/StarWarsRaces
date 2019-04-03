
using System;
using System.Collections.Generic;
using System.Reflection;
using AlienRace;
using Harmony;
using Verse;

namespace StarWarsRaces
{
    [StaticConstructorOnStartup]
    internal static class Main
    {
        static Main()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("com.rimworld.mod.starwarsraces.speciesoftherim");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            LongEventHandler.QueueLongEvent(new Action(Init), "LibraryStartup", false, null);
        }
        private static void Init()
        {
            string[] labels = SettingsController.Settings.labels;
            float[] chances = SettingsController.Settings.SpawnChance;

            // add some modded apparel to races with a whitelist for apparel
            if (chances[0] > 0f)
            {
                AddApparelToWhiteList("StarWarsRaces_Twilek");
            }
            if (chances[3] > 0f)
            {
                AddHeadgearToWhiteList("StarWarsRaces_Ewok");
            }
            if (chances[4] > 0f)
            {
                AddApparelToWhiteList("StarWarsRaces_Togruta");
            }

            Factions.AdjustAlienRaceSettingsSpawnChance(labels, chances);
            Factions.AddAliensToNPCFactions(labels, chances);

        }
        // no hats
        private static void AddApparelToWhiteList(string defname)
        {
            ThingDef_AlienRace twilekRace = DefDatabase<ThingDef_AlienRace>.GetNamed(defname);


            foreach (ThingDef aThingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (aThingDef.IsApparel && !aThingDef.apparel.layers.Contains(DefDatabase<ApparelLayerDef>.GetNamed("Overhead")))
                {
                    if (RaceRestrictionSettings.apparelRestrictionDict.ContainsKey(key: aThingDef))
                    { continue; }
                    if (RaceRestrictionSettings.apparelWhiteDict.ContainsKey(aThingDef))
                    {
                        List<ThingDef_AlienRace> s = RaceRestrictionSettings.apparelWhiteDict.TryGetValue(aThingDef);
                        if (!s.Contains(twilekRace))
                        {
                            s.Add(twilekRace);
                        }
                        continue;

                    }
                    RaceRestrictionSettings.apparelWhiteDict.Add(
                        aThingDef,
                        new List<ThingDef_AlienRace>{
                            twilekRace
                        }
                    );
                }
            }
        }
        // yes hats
        private static void AddHeadgearToWhiteList(string defname)
        {
            ThingDef_AlienRace twilekRace = DefDatabase<ThingDef_AlienRace>.GetNamed(defname);


            foreach (ThingDef aThingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (aThingDef.IsApparel && aThingDef.apparel.layers.Contains(DefDatabase<ApparelLayerDef>.GetNamed("Overhead")))
                {
                    if (RaceRestrictionSettings.apparelRestrictionDict.ContainsKey(key: aThingDef))
                    { continue; }
                    if (RaceRestrictionSettings.apparelWhiteDict.ContainsKey(aThingDef))
                    {
                        List<ThingDef_AlienRace> s = RaceRestrictionSettings.apparelWhiteDict.TryGetValue(aThingDef);
                        if (!s.Contains(twilekRace))
                        {
                            s.Add(twilekRace);
                        }
                        continue;

                    }
                    RaceRestrictionSettings.apparelWhiteDict.Add(
                        aThingDef,
                        new List<ThingDef_AlienRace>{
                                twilekRace
                        }
                    );
                }
            }
        }
    }
}