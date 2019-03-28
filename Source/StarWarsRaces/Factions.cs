using System.Collections.Generic;
using Verse;
using RimWorld;
using AlienRace;

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
            loadedFactions = false;
            foreach (ModContentPack ResolvedMod in LoadedModManager.RunningMods)
            {
                if (ResolvedMod.Name.Contains("Star Wars - Factions"))
                {
                    //Log.Message("Star Wars - Factions Detected.");
                    loadedFactions = true;
                    modCheck = true;
                    return;
                }
            }
            modCheck = true;
            return;
        }

        public static PawnGroupMaker AddPawnKindsToFactions(PawnGroupMaker pgm, string[] labels, float[] chances)
        {
            string skipRaces = "skipping races: ";
            for (int ii = 0; ii < labels.Length; ii++)
            {
                if (chances[ii] <= 0f)
                {
                    skipRaces += labels[ii] + " ";
                    continue;
                }
                PawnGenOption[] options = pgm.options.ToArray();
                for (int i = 0; i < options.Length; i++)
                {
                    PawnGenOption pgo = makePawnGenOption(options[i], labels[ii]);
                    if (pgo != null) { pgm.addPawn(pgo, false); }
                }
                PawnGenOption[] guards = pgm.guards.ToArray();
                for (int j = 0; j < guards.Length; j++)
                {
                    PawnGenOption pgo = makePawnGenOption(guards[j], labels[ii]);
                    if (pgo != null) { pgm.addPawn(pgo, true); }
                }
            }
            return pgm;
        }
        private static PawnGenOption makePawnGenOption(PawnGenOption existing, string label)
        {
            string pawnKindLabel = existing.kind.defName;
            float sw = existing.selectionWeight * 0.1f;

            // if it is not a Humanlike than don't copy it
            if (pawnKindLabel.Contains("StarWarsRaces_") || PawnKindDef.Named(pawnKindLabel).race != ThingDefOf.Human)
            {
                return null;
            }
            return new PawnGenOption
            {
                selectionWeight = sw,
                kind = PawnKindDef.Named(getDefName(pawnKindLabel, sw, label))
            };
        }


        private static void addPawn(this PawnGroupMaker pgm, PawnGenOption pgo, bool isTrader = false)
        {
            if (pgo.kind == null){ return; };
            if (isTrader) {
                pgm.guards.Add(pgo);
                return;
            }
            pgm.options.Add(pgo);
        }

        private static string getDefName(string pawnKindLabel, float sw, string label)
        {
            string defname = "StarWarsRaces_" + label + "_" + pawnKindLabel;
            CreateNewPawnKind(PawnKindDef.Named(pawnKindLabel), label, defname);
            return defname;
        }
        private static void CreateNewPawnKind(PawnKindDef pkOld, string label, string defname)
        {
            // if it already exists then don't recreate it
            if (DefDatabase<PawnKindDef>.GetNamedSilentFail(defname) != null)
            {
                return;
            }
            PawnKindDef pk = new PawnKindDef();
            pk.defName = defname;
            pk.label = pkOld.label + " (" + label + ")";

            pk.apparelRequired = pkOld.apparelRequired.ListFullCopyOrNull<ThingDef>();
            pk.apparelTags = pkOld.apparelTags.ListFullCopyOrNull<string>();
            pk.apparelAllowHeadgearChance = pkOld.apparelAllowHeadgearChance;
            pk.aiAvoidCover = pkOld.aiAvoidCover;
            pk.apparelColor = pkOld.apparelColor;
            pk.apparelIgnoreSeasons = pkOld.apparelIgnoreSeasons;
            pk.apparelMoney = new FloatRange(min:pkOld.apparelMoney.min, max:pkOld.apparelMoney.max);
            pk.backstoryCategories = pkOld.backstoryCategories.ListFullCopyOrNull<string>();
            pk.backstoryCryptosleepCommonality = pkOld.backstoryCryptosleepCommonality;
            pk.baseRecruitDifficulty = pkOld.baseRecruitDifficulty;
            pk.canArriveManhunter = pkOld.canArriveManhunter;
            pk.canBeSapper = pkOld.canBeSapper;
            pk.chemicalAddictionChance = pkOld.chemicalAddictionChance;
            pk.combatEnhancingDrugsChance = pkOld.combatEnhancingDrugsChance;
            pk.combatEnhancingDrugsCount = new IntRange(min:pkOld.combatEnhancingDrugsCount.min, max:pkOld.combatEnhancingDrugsCount.max);
            pk.combatPower = pkOld.combatPower;
            pk.defPackage = pkOld.defPackage;
            pk.defaultFactionType = pkOld.defaultFactionType;
            pk.description = pkOld.description;
            pk.destroyGearOnDrop = pkOld.destroyGearOnDrop;
            pk.ecoSystemWeight = pkOld.ecoSystemWeight;
            pk.factionLeader = pkOld.factionLeader;
            pk.fixedInventory = pkOld.fixedInventory.ListFullCopyOrNull<ThingDefCountClass>();
            pk.fleeHealthThresholdRange = new FloatRange(min: pkOld.fleeHealthThresholdRange.min, max: pkOld.fleeHealthThresholdRange.max);
            pk.forceNormalGearQuality = pkOld.forceNormalGearQuality;
            pk.gearHealthRange = new FloatRange(min: pkOld.gearHealthRange.min, max: pkOld.gearHealthRange.max);
            pk.generated = pkOld.generated;
            pk.inventoryOptions = pkOld.inventoryOptions;
            pk.invFoodDef = pkOld.invFoodDef;
            pk.invNutrition = pkOld.invNutrition;
            pk.isFighter = pkOld.isFighter;
            pk.itemQuality = pkOld.itemQuality;
            pk.lifeStages = pkOld.lifeStages;
            pk.maxGenerationAge = pkOld.maxGenerationAge;
            pk.minGenerationAge = pkOld.minGenerationAge;
            pk.modContentPack = pkOld.modContentPack;
            pk.modExtensions = pkOld.modExtensions;
            pk.trader = pkOld.trader;
            pk.techHediffsChance = pkOld.techHediffsChance;
            pk.techHediffsMoney = new FloatRange(min: pkOld.techHediffsMoney.min, max: pkOld.techHediffsMoney.max);
            pk.techHediffsTags = pkOld.techHediffsTags.ListFullCopyOrNull<string>();
            pk.weaponMoney = new FloatRange(min: pkOld.weaponMoney.min, max: pkOld.weaponMoney.max);
            pk.weaponTags = pkOld.weaponTags.ListFullCopyOrNull<string>();
            pk.wildGroupSize = pkOld.wildGroupSize;

            switch (label)
            {
                case ("Twilek"):
                    ThingDef_AlienRace twilekRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Twilek");
                    pk.race = twilekRace;
                    
                    // apparel tags
                    if (pk.apparelTags == null)
                    {
                        pk.apparelTags = new List<string>();
                    }
                    pk.apparelTags.Add("StarWarsRaces_TwilekApparelTag");
                    pk.apparelTags.RemoveDuplicates<string>();

                    // twileks like their headgear
                    pk.apparelAllowHeadgearChance = 50f;
                    
                    // gotta remove the non-white-listed required apparel.
                    if (!pk.apparelRequired.NullOrEmpty<ThingDef>())
                    {
                        pk.apparelRequired.RemoveDuplicates<ThingDef>();
                        List<ThingDef> required = pk.apparelRequired.ListFullCopy<ThingDef>();
                        for (int i = 0; i < required.Count; i++)
                        {
                            ThingDef entry = required[i];
                            // if  in the white list then add to the list of required apparel
                            if (!twilekRace.alienRace.raceRestriction.whiteApparelList.Contains(entry.defName))
                            {
                                pk.apparelRequired.Remove(entry);
                            }
                        }
                        if (pk.apparelRequired.Count <= 0)
                        {
                            pk.apparelRequired = null;
                        }
                    }
                    break;
                case ("Togruta"):
                    ThingDef_AlienRace thisRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Togruta");
                    pk.race = thisRace;

                    // apparel tags
                    if (pk.apparelTags == null)
                    {
                        pk.apparelTags = new List<string>();
                    }
                    pk.apparelTags.Add("StarWarsRaces_TogrutaApparelTag");
                    pk.apparelTags.RemoveDuplicates<string>();


                    // gotta remove the non-white-listed required apparel.
                    if (!pk.apparelRequired.NullOrEmpty<ThingDef>())
                    {
                        pk.apparelRequired.RemoveDuplicates<ThingDef>();
                        List<ThingDef> required = pk.apparelRequired.ListFullCopy<ThingDef>();
                        for (int i = 0; i < required.Count; i++)
                        {
                            ThingDef entry = required[i];
                            // if  in the white list then add to the list of required apparel
                            if (!thisRace.alienRace.raceRestriction.whiteApparelList.Contains(entry.defName))
                            {
                                pk.apparelRequired.Remove(entry);
                            }
                        }
                        if (pk.apparelRequired.Count <= 0)
                        {
                            pk.apparelRequired = null;
                        }
                    }
                    break;

                case ("Wookie"):
                    ThingDef_AlienRace wookieRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Wookie");
                    pk.race = wookieRace;
                    // wookies are usually naked except for the bandolier
                    // remove all required apparel except the bandolier
                    if (pk.apparelTags.NullOrEmpty<string>())
                    {
                        pk.apparelTags = new List<string>();
                    }
                    pk.apparelTags.Add("StarWarsRaces_WookieApparelTag");
                    pk.apparelRequired = new List<ThingDef>
                    {
                        DefDatabase<ThingDef>.GetNamed("StarWarsRaces_Wookie_Bandolier")
                    };
                    pk.apparelAllowHeadgearChance = pkOld.apparelAllowHeadgearChance;
                    break;
                case ("Ewok"):
                    ThingDef_AlienRace ewokRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Ewok");
                    pk.race = ewokRace;
                    // ewoks are usually naked except for the traditional hood and wrap or some other primitive items
                    // remove all required apparel except those items
                    if (pk.apparelTags.NullOrEmpty<string>())
                    {
                        pk.apparelTags = new List<string>();
                    }
                    pk.apparelTags.Add("StarWarsRaces_EwokApparelTag");
                    pk.apparelTags.Add("Neolithic");
                    pk.apparelRequired = new List<ThingDef>
                    {
                        DefDatabase<ThingDef>.GetNamed("StarWarsRaces_Ewok_Hood")
                    }; 
                    pk.apparelAllowHeadgearChance = 100f;
                    pk.weaponTags.Add("NeolithicMeleeBasic");
                    pk.weaponTags.Add("NeolithicRangedBasic");
                    pk.weaponTags.Add("NeolithicMeleeDecent");
                    pk.weaponTags.Add("NeolithicRangedDecent");
                    pk.weaponTags.Add("NeolithicMeleeAdvanced");
                    pk.weaponTags.Add("NeolithicRangedHeavy");
                    pk.weaponTags.Add("MedievalMeleeAdvanced");
                    pk.weaponTags.Add("NeolithicRangedChief");
                    break;
                default:
                    ThingDef_AlienRace newRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_" + label);
                    pk.race = newRace;
                    break;
                    
            }
            DefDatabase<PawnKindDef>.Add(pk);
            DefDatabase<PawnKindDef>.ErrorCheckAllDefs();
            DefDatabase<PawnKindDef>.ResolveAllReferences();
        }
    }
}
