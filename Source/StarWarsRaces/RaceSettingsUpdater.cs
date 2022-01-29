using Verse;
using RimWorld;
using AlienRace;
using System.Linq;
using System.Collections.Generic;

namespace StarWarsRaces
{
    public static class RaceSettingsUpdater
    {
        public static void AdjustSpawnChance()
        {
            UpdateRaceSettings(ref SettingsController.Settings.Twilek);
            UpdateRaceSettings(ref SettingsController.Settings.Rodian);
            UpdateRaceSettings(ref SettingsController.Settings.Ewok);
            UpdateRaceSettings(ref SettingsController.Settings.Togruta);
            UpdateRaceSettings(ref SettingsController.Settings.Wookiee);
        }

        private static void UpdateRaceSettings(ref SpeciesControl speciesControl)
        {
            string label = speciesControl.Label;
            foreach (RaceSettings rs in DefDatabase<RaceSettings>.AllDefsListForReading)
            {
                if (rs.defName == "StarWarsRaces_Settings")
                {
                    foreach (FactionPawnKindEntry sc in rs.pawnKindSettings.startingColonists)
                    {
                        foreach (PawnKindEntry pke in sc.pawnKindEntries)
                        {
                            if (pke.kindDefs.Exists(k => k.defName.Contains(label)))
                            {
                                pke.chance = speciesControl.Colonist ? SettingsController.Settings.spawnChance : 0f;
                            };
                        };
                    };
                    foreach (FactionPawnKindEntry awk in rs.pawnKindSettings.alienwandererkinds)
                    {
                        foreach (PawnKindEntry pke in awk.pawnKindEntries)
                        {
                            if (pke.kindDefs.Exists(k => k.defName.Contains(label)))
                            {
                                pke.chance = speciesControl.Wanderer ? SettingsController.Settings.spawnChance : 0f;
                            };
                        };
                    };
                    foreach (PawnKindEntry pke in rs.pawnKindSettings.alienrefugeekinds)
                    {
                        if (pke.kindDefs.Exists(k => k.defName.Contains(label)))
                        {
                            pke.chance = speciesControl.Refugee ? SettingsController.Settings.spawnChance : 0f;
                        };
                    };
                    foreach (PawnKindEntry pke in rs.pawnKindSettings.alienslavekinds)
                    {
                        if (pke.kindDefs.Exists(k => k.defName.Contains(label)))
                        {
                            pke.chance = speciesControl.Slave ? SettingsController.Settings.spawnChance : 0f;
                        };
                    };
                }
            }
        }

    }
}
