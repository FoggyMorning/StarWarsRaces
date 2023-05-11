using Verse;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace StarWarsRaces
{
    public static class Factions
    {
        private static bool modCheck = false;
        private static bool loadedFactions = false;
        public static bool IsStarWarsFactionsLoaded()
        {
            if (!modCheck) ModCheck();
            return loadedFactions;
        }
        private static void ModCheck()
        {
            modCheck = true;
            if (ModsConfig.ActiveModsInLoadOrder.Any(m => m.PackageId == "projectjedi.factions"))
            {
                loadedFactions = true;
                return;
            }
            return;
        }
        public static void AddAliensToNPCFactions()
        {
            List<SpeciesControl> x = new List<SpeciesControl> {
                SettingsController.Settings.Ewok,
                SettingsController.Settings.Twilek,
                SettingsController.Settings.Rodian,
                SettingsController.Settings.Togruta,
                SettingsController.Settings.Wookiee
                };
            UpdateNPCFactions(x);
        }
        private static void UpdateNPCFactions(List<SpeciesControl> speciesControl)
        {
            if (speciesControl.Any(s => s.Pirate == false))
            {
                List<string> defNames = new List<string> { };
                for (int i = 0; i < speciesControl.Count; i++) { if (!speciesControl[i].Pirate) { defNames.Add(speciesControl[i].Label); } };
                UpdateNPCFaction("Pirate", defNames);
            }
            if (speciesControl.Any(s => s.Outlander == false))
            {
                List<string> defNames = new List<string> { };
                for (int i = 0; i < speciesControl.Count; i++) { if (!speciesControl[i].Outlander) { defNames.Add(speciesControl[i].Label); } };
                UpdateNPCFaction("OutlanderCivil", defNames);
                UpdateNPCFaction("OutlanderRough", defNames);
            }
            if (speciesControl.Any(s => s.Tribal == false))
            {
                List<string> defNames = new List<string> { };
                for (int i = 0; i < speciesControl.Count; i++) { if (!speciesControl[i].Tribal) { defNames.Add(speciesControl[i].Label); } };
                UpdateNPCFaction("TribeCivil", defNames);
                UpdateNPCFaction("TribeRough", defNames);
                UpdateNPCFaction("TribeSavage", defNames);
            }

            DefDatabase<FactionDef>.AddAllInMods();
            
            if (Factions.IsStarWarsFactionsLoaded())
            {
                if (speciesControl.Any(s => s.Rebels == false))
                {
                    List<string> defNames = new List<string> { };
                    for (int i = 0; i < speciesControl.Count; i++) { if (!speciesControl[i].Rebels) { defNames.Add(speciesControl[i].Label); } };
                    UpdateNPCFaction("PJ_RebelFac", defNames);
                }
                if (speciesControl.Any(s => s.Scum == false))
                {
                    List<string> defNames = new List<string> { };
                    for (int i = 0; i < speciesControl.Count; i++) { if (!speciesControl[i].Scum) { defNames.Add(speciesControl[i].Label); } };
                    UpdateNPCFaction("PJ_Bounty", defNames);
                }
                if (speciesControl.Any(s => s.Empire == false))
                {
                    List<string> defNames = new List<string> { };
                    for (int i = 0; i < speciesControl.Count; i++) { if (!speciesControl[i].Empire) { defNames.Add(speciesControl[i].Label); } };
                    UpdateNPCFaction("PJ_GalacticEmpire", defNames);
                }
            }
            
        }
        private static void UpdateNPCFaction(string factionName, List<string> labels)
        {
            FactionDef f = DefDatabase<FactionDef>.GetNamedSilentFail(factionName);
            if (f == null) { return; };
            //Log.Message($"Removing {f.label} faction pawnkinds for races: {string.Join(", ", labels)}");
            PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
            for (int x = 0; x < g.Length; x++)
            {
                // groups can be either an options or a guards for whatever reason, so check for both.
                // they default to an empty array so if there are no entries for one of them it will skip
                PawnGenOption[] options = g[x].options.ToArray();
                for (int i = 0; i < options.Length; i++)
                {
                    if (labels.Any(l => options[i].kind.defName.StartsWith(l)))
                    {

                        g[x].options.Remove(options[i]); 
                    }
                }
                PawnGenOption[] guards = g[x].guards.ToArray();
                for (int j = 0; j < guards.Length; j++)
                {
                    if (labels.Any(l => guards[j].kind.defName.StartsWith(l)))
                    {
                        g[x].guards.Remove(guards[j]);
                    }
                }
                f.pawnGroupMakers[x] = g[x];
            }
        }

    }
}
