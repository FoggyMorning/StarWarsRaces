using System;
using System.Collections.Generic;
using System.Reflection;
using AlienRace;
using Harmony;
using RimWorld;
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
            string[] labels = SettingsController.Settings.RaceIdentif;
            float[] chances = SettingsController.Settings.SpawnChance;
            SettingsUtil.LoadAllSpecies(labels, chances);
            AddApparelToTwilek(chances[0]);
            /*
            bool flag = Factions.IsStarWarsFactionsLoaded();
            if (flag)
            {
                Log.Message("Working on Rebel Faction=======================================", false);
                FactionDef rebFact = DefDatabase<FactionDef>.GetNamed("PJ_RebelFac", true);
                PawnGroupMaker[] pgmsReb = rebFact.pawnGroupMakers.ToArray();
                for (int i = 0; i < pgmsReb.Length; i++)
                {
                    rebFact.pawnGroupMakers[i].AddAllRacesAsOptions(labels, chances, false);
                }
                Log.Message("Working on Scum Faction=======================================", false);
                FactionDef scumFact = DefDatabase<FactionDef>.GetNamed("PJ_Bounty", true);
                PawnGroupMaker[] pgmsScum = scumFact.pawnGroupMakers.ToArray();
                for (int j = 0; j < pgmsScum.Length; j++)
                {
                    scumFact.pawnGroupMakers[j].AddAllRacesAsOptions(labels, chances, false);
                }
            }
            */
        }
        private static void AddApparelToTwilek(float chance)
        {
            if (!(chance <= 0f))
            {
                ThingDef_AlienRace t = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Twilek", true);
                foreach (ThingDef a in DefDatabase<ThingDef>.AllDefsListForReading)
                {
                    bool flag2 = a.IsApparel && !a.apparel.layers.Contains(DefDatabase<ApparelLayerDef>.GetNamed("Overhead", true)) && !t.alienRace.raceRestriction.whiteApparelList.Contains(a.ToString());
                    if (flag2)
                    {
                        t.alienRace.raceRestriction.whiteApparelList.Add(a.ToString());
                    }
                }
            }
        }

    }
}
