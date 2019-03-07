
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

            Log.Message("Working on adding StarWarsRaces to the Pirate Faction=======================================", false);
            FactionDef pirateFact = DefDatabase<FactionDef>.GetNamed("Pirate", true);
            PawnGroupMaker[] pgmsPirate = pirateFact.pawnGroupMakers.ToArray();
            for (int x = 0; x < pgmsPirate.Length; x++)
            {
                pirateFact.pawnGroupMakers[x] = Factions.AddPawnKindsToFactions(pgmsPirate[x], labels, chances);
            }

            DefDatabase<FactionDef>.AddAllInMods();
            if (Factions.IsStarWarsFactionsLoaded())
            {
                Log.Message("Working on adding StarWarsRaces to the Rebel Faction=======================================", false);
                FactionDef rebFact = DefDatabase<FactionDef>.GetNamed("PJ_RebelFac", true);
                PawnGroupMaker[] pgmsReb = rebFact.pawnGroupMakers.ToArray();
                for (int i = 0; i < pgmsReb.Length; i++)
                {
                    rebFact.pawnGroupMakers[i] = Factions.AddPawnKindsToFactions(pgmsReb[i], labels, chances);
                }

                Log.Message("Working on adding StarWarsRaces to the Scum Faction=======================================", false);
                FactionDef scumFact = DefDatabase<FactionDef>.GetNamed("PJ_Bounty", true);
                PawnGroupMaker[] pgmsScum = scumFact.pawnGroupMakers.ToArray();
                for (int j = 0; j < pgmsScum.Length; j++)
                {
                    scumFact.pawnGroupMakers[j] = Factions.AddPawnKindsToFactions(pgmsScum[j], labels, chances);
                }
            }


        }
        private static void AddApparelToTwilek(float chance)
        {
            if ((chance <= 0f))
            {
                return;
            }
            string message = "Adding apparel to twilek white apparel list: ";
            ThingDef_AlienRace twilekRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Twilek");
            foreach (ThingDef aThingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (aThingDef.IsApparel && 
                    !aThingDef.apparel.layers.Contains(DefDatabase<ApparelLayerDef>.GetNamed("Overhead")))
                {
                    if (RaceRestrictionSettings.apparelRestrictionDict.ContainsKey(key: aThingDef))
                    { continue; }
                    if (!RaceRestrictionSettings.apparelWhiteDict.ContainsKey(aThingDef))
                    {
                        message += aThingDef.defName + ", ";
                        RaceRestrictionSettings.apparelWhiteDict.Add(
                            aThingDef, 
                            new List<ThingDef_AlienRace>{
                                twilekRace
                            }
                        );
                        continue;
                    }
                    List<ThingDef_AlienRace> s = RaceRestrictionSettings.apparelWhiteDict.TryGetValue(aThingDef);
                    if (!s.Contains(twilekRace))
                    {
                        message += aThingDef.defName + ", ";
                        s.Add(twilekRace);
                    }
                }
            }
            if (message != "Adding apparel to twilek white apparel list: ")
            {
                Log.Message(message);
            }
        }
    }
}
