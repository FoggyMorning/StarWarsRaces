
using System;
using System.Collections.Generic;
using System.Reflection;
using AlienRace;
using HarmonyLib;
using Verse;

namespace StarWarsRaces
{
    [StaticConstructorOnStartup]
    internal static class Main
    {
        static Main()
        {
            var harmony = new Harmony ("com.rimworld.mod.starwarsraces.speciesoftherim");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            LongEventHandler.QueueLongEvent(new Action(InitLib), "LibraryStartup", false, null);
        }
        private static void InitLib()
        {
            string[] labels = SettingsController.Settings.labels;
            float[] chances = SettingsController.Settings.SpawnChance;

            Factions.AdjustAlienRaceSettingsSpawnChance(labels, chances);
            Factions.AddAliensToNPCFactions(labels, chances);

        }
    }
}