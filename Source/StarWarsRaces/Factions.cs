using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace StarWarsRaces
{
    class Factions
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
            //Log.Message("Mod Check Called");
            loadedFactions = false;
            foreach (ModContentPack ResolvedMod in LoadedModManager.RunningMods)
            {
                if (loadedFactions) break; //Save some loading
                if (ResolvedMod.Name.Contains("Star Wars - Factions"))
                {
                    //Log.Message("Star Wars - Factions Detected.");
                    loadedFactions = true;
                }
            }
            modCheck = true;
            return;
        }
        /*
        private static void AddAllRacesAsOptions(this PawnGroupMaker pgm, string[] labels, float[] chances, bool isTrader = false)
        {
            for (int i = 0; i < pgm.options.Count; i++)
            {
                pgm.addPawn(pgm.options[i], chances[i], labels[i], false);
            }
            for (int j = 0; j < pgm.guards.Count; j++)
            {
                pgm.addPawn(pgm.guards[j], chances[j], labels[j], true);
            }
        }
        private static void addPawn(this PawnGroupMaker pgm, PawnGenOption thisoption, float chance, string label, bool isTrader = false)
        {
            bool flag = chance <= 0f;
            if (!flag)
            {
                string pawnKindLabel = thisoption.kind.defName;
                float sw = thisoption.selectionWeight;
                PawnGenOption pgo = new PawnGenOption
                {
                    selectionWeight = sw,
                    kind = PawnKindDef.Named(Main.getDefName(pawnKindLabel, sw, label))
                };
                if (isTrader)
                {
                    pgm.guards.Add(pgo);
                    Log.Message("guards: " + pgm.guards.ToStringSafeEnumerable(), false);
                }
                else
                {
                    pgm.options.Add(pgo);
                    Log.Message("options: " + pgm.options.ToStringSafeEnumerable(), false);
                }
            }
        }
        private static string getDefName(string pawnKindLabel, float sw, string label)
        {
            string defname = "StarWarsRaces_" + label + "_" + pawnKindLabel;
            Main.CreateNewPawnKind(PawnKindDef.Named(pawnKindLabel), label, defname);
            return defname;
        }
        private static void CreateNewPawnKind(PawnKindDef pkOld, string label, string defname)
        {
            PawnKindDef pk = DefDatabase<PawnKindDef>.GetNamedSilentFail(defname);
            bool flag = pk == null;
            if (flag)
            {
                pk = PawnKindDefOf.SpaceRefugee;
                pk.race = ThingDef.Named("StarWarsRaces_" + label);
                pk.defName = defname;
                pk.label = pkOld.label + " (" + label + ")";
                pk.apparelAllowHeadgearChance = pkOld.apparelAllowHeadgearChance;
                pk.apparelMoney = pkOld.apparelMoney;
                pk.apparelRequired = pkOld.apparelRequired;
                pk.apparelTags = pkOld.apparelTags;
                pk.backstoryCategories = pkOld.backstoryCategories;
                pk.backstoryCryptosleepCommonality = pkOld.backstoryCryptosleepCommonality;
                pk.baseRecruitDifficulty = pkOld.baseRecruitDifficulty;
                pk.canBeSapper = pkOld.canBeSapper;
                pk.chemicalAddictionChance = pkOld.chemicalAddictionChance;
                pk.combatEnhancingDrugsChance = pkOld.combatEnhancingDrugsChance;
                pk.combatEnhancingDrugsCount = pkOld.combatEnhancingDrugsCount;
                pk.combatPower = pkOld.combatPower;
                pk.defaultFactionType = pkOld.defaultFactionType;
                pk.description = pkOld.description;
                pk.destroyGearOnDrop = pkOld.destroyGearOnDrop;
                pk.factionLeader = pkOld.factionLeader;
                pk.fixedInventory = pkOld.fixedInventory;
                pk.fleeHealthThresholdRange = pkOld.fleeHealthThresholdRange;
                pk.forceNormalGearQuality = pkOld.forceNormalGearQuality;
                pk.gearHealthRange = pkOld.gearHealthRange;
                pk.generated = false;
                pk.inventoryOptions = pkOld.inventoryOptions;
                pk.invFoodDef = pkOld.invFoodDef;
                pk.invNutrition = pkOld.invNutrition;
                pk.isFighter = pkOld.isFighter;
                pk.itemQuality = pkOld.itemQuality;
                pk.maxGenerationAge = pkOld.maxGenerationAge;
                pk.techHediffsChance = pkOld.techHediffsChance;
                pk.techHediffsMoney = pkOld.techHediffsMoney;
                pk.techHediffsTags = pkOld.techHediffsTags;
                pk.weaponMoney = pkOld.weaponMoney;
                pk.weaponTags = pkOld.weaponTags;
                switch (label) {
                    case ("Twilek"):
                        ThingDef_AlienRace t = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Twilek", true);
                        bool flag3 = pk.apparelTags.NullOrEmpty<string>();
                        if (flag3)
                        {
                            pk.apparelTags = new List<string>();
                        }
                        pk.apparelTags.Add("StarWarsRaces_TwilekApparelTag");
                        pk.apparelAllowHeadgearChance = 100f;
                        bool flag4 = !pk.apparelRequired.NullOrEmpty<ThingDef>();
                        if (flag4)
                        {
                            ThingDef[] pkar = pk.apparelRequired.ToArray();
                            foreach (ThingDef ar in pkar)
                            {
                                bool flag5 = ar.IsApparel && (ar.apparel.layers.Contains(DefDatabase<ApparelLayerDef>.GetNamed("Overhead", true)) || !t.alienRace.raceRestriction.whiteApparelList.Contains(ar.ToString()));
                                if (flag5)
                                {
                                    pk.apparelRequired.Remove(ar);
                                }
                            }
                        }
                        break;
                    case ("Wookie"):
                        if (pk.apparelTags.NullOrEmpty<string>())
                        {
                            pk.apparelTags = new List<string>
                            {
                                "StarWarsRaces_WookieApparelTag"
                            };
                        }
                        // wookies are naked except for the bandolier
                        pk.apparelRequired = new List<ThingDef>
                        {
                            DefDatabase<ThingDef>.GetNamed("StarWarsRaces_Wookie_Bandolier")
                        };
                        break;
                    case ("Ewok"):
                        if (pk.apparelTags.NullOrEmpty<string>())
                        {
                            pk.apparelTags = new List<string>
                            {
                                "StarWarsRaces_EwokApparelTag"
                            };
                        }
                        // ewoks are naked except for the hood and wrap
                        pk.apparelRequired = new List<ThingDef>
                        {
                            DefDatabase<ThingDef>.GetNamed("StarWarsRaces_Ewok_Parka"),
                            DefDatabase<ThingDef>.GetNamed("StarWarsRaces_Ewok_Hood")
                        };
                        break;
                }
                DefDatabase<PawnKindDef>.Add(pk);
            }
        }
            */
    }
}
