using System.Collections.Generic;
using Verse;
using RimWorld;
using AlienRace;
using System.Linq;

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
            if (ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Star Wars - Factions"))
            {
                loadedFactions = true;
                return;
            }
            return;
        }

        // adds labels to the alien race RaceSettings def using the chances values
        public static void AdjustAlienRaceSettingsSpawnChance(string[] labels, float[] chances)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                string label = labels[i];
                foreach (RaceSettings rs in DefDatabase<RaceSettings>.AllDefsListForReading)
                {
                    if (rs.defName == "StarWarsRaces_Settings")
                    {
                        foreach (FactionPawnKindEntry sc in rs.pawnKindSettings.startingColonists)
                        {
                            foreach (PawnKindEntry pke in sc.pawnKindEntries)
                            {
                                if (pke.kindDefs.Any(k => k.Contains(label))) 
                                {
                                    pke.chance = chances[i] * 10;
                                };
                            };
                        };
                        foreach (FactionPawnKindEntry awk in rs.pawnKindSettings.alienwandererkinds)
                        {
                            foreach (PawnKindEntry pke in awk.pawnKindEntries)
                            {
                                    if (pke.kindDefs.Any(k => k.Contains(label)))
                                    {
                                        pke.chance = chances[i] * 10;
                                    };
                            };
                        };
                        foreach (PawnKindEntry pke in rs.pawnKindSettings.alienrefugeekinds)
                        {
                            if (pke.kindDefs.Any(k => k.Contains(label)))
                            {
                                pke.chance = chances[i] * 10;
                            };
                        };
                        foreach (PawnKindEntry pke in rs.pawnKindSettings.alienslavekinds)
                        {
                            if (pke.kindDefs.Any(k => k.Contains(label)))
                            {
                                pke.chance = chances[i] * 10;
                            };
                        };
                        break;

                    }
                }
            }
        }


        // why make a bunch of defs when you can do some sloppy c# list manipulation
        public static void AddAliensToNPCFactions(string[] labels, float[] chances)
        {

            for (int i = 0; i < labels.Length; i++)
            {
                if (chances[i] <= 0f) {
                    continue;
                }

                if (SettingsController.Settings.IncludeInPirate)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("Pirate", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = addPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInOutlander)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("OutlanderCivil", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = addPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInOutlander)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("OutlanderRough", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = addPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInTribal)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("TribeCivil", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = addPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInTribal)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("TribeRough", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = addPawnKindsToFactions(g[x], labels[i]);
                    }
                }

                DefDatabase<FactionDef>.AddAllInMods();
                if (Factions.IsStarWarsFactionsLoaded())
                {
                    FactionDef rebFact = DefDatabase<FactionDef>.GetNamed("PJ_RebelFac", true);
                    PawnGroupMaker[] pgmsReb = rebFact.pawnGroupMakers.ToArray();
                    for (int j = 0; j < pgmsReb.Length; j++)
                    {
                        rebFact.pawnGroupMakers[j] = addPawnKindsToFactions(pgmsReb[j], labels[i]);
                    }

                    FactionDef scumFact = DefDatabase<FactionDef>.GetNamed("PJ_Bounty", true);
                    PawnGroupMaker[] pgmsScum = scumFact.pawnGroupMakers.ToArray();
                    for (int j = 0; j < pgmsScum.Length; j++)
                    {
                        scumFact.pawnGroupMakers[j] = addPawnKindsToFactions(pgmsScum[j], labels[i]);
                    }
                }
            }
        }

 
        private static PawnGroupMaker addPawnKindsToFactions(PawnGroupMaker pgm, string label)
        {
                // groups can be either an options or a guards for whatever reason, so check for both.
                PawnGenOption[] options = pgm.options.ToArray();
                for (int i = 0; i < options.Length; i++)
                {
                    PawnGenOption pgo = makePawnGenOption(options[i], label);
                    if (pgo != null) { pgm.addPawn(pgo, false); }
                }
                PawnGenOption[] guards = pgm.guards.ToArray();
                for (int j = 0; j < guards.Length; j++)
                {
                    PawnGenOption pgo = makePawnGenOption(guards[j], label);
                    if (pgo != null) { pgm.addPawn(pgo, true); }
                }
            return pgm;
        }
        private static void addPawn(this PawnGroupMaker pgm, PawnGenOption pgo, bool isTrader = false)
        {
            if (pgo.kind == null) { return; };
            if (isTrader)
            {
                pgm.guards.Add(pgo);
                return;
            }
            pgm.options.Add(pgo);
        }
        private static PawnGenOption makePawnGenOption(PawnGenOption existing, string label)
        {
            string pawnKindLabel = existing.kind.defName;
            float sw = existing.selectionWeight * 0.075f;

            // if it is one of our defs then don't recreate it
            // if some alien race other than a Human then don't risk copying it
            if (pawnKindLabel.Contains("StarWarsRaces_") || PawnKindDef.Named(pawnKindLabel).race != ThingDefOf.Human)
            {
                return null;
            }

            string defname = "StarWarsRaces_" + label + "_" + pawnKindLabel;
            createNewPawnKind(PawnKindDef.Named(pawnKindLabel), label, defname);
            return new PawnGenOption
            {
                selectionWeight = sw,
                kind = PawnKindDef.Named(defname)
            };
        }
        
        private static void createNewPawnKind(PawnKindDef pkOld, string label, string defname)
        {
            // if it already exists then don't recreate it
            if (DefDatabase<PawnKindDef>.GetNamedSilentFail(defname) != null)
            {
                return;
            }
            PawnKindDef pk = new PawnKindDef();
            pk.defName = defname;
            pk.label = pkOld.label + " (" + label + ")";

            pk.allowRoyalApparelRequirements = pkOld.allowRoyalApparelRequirements;
            pk.allowRoyalRoomRequirements = pkOld.allowRoyalRoomRequirements;
            pk.alternateGraphicChance = pkOld.alternateGraphicChance;
            pk.alternateGraphics = pkOld.alternateGraphics.ListFullCopyOrNull<AlternateGraphic>();
            pk.apparelRequired = pkOld.apparelRequired.ListFullCopyOrNull<ThingDef>();
            pk.apparelDisallowTags = pkOld.apparelDisallowTags.ListFullCopyOrNull<string>();
            pk.apparelTags = pkOld.apparelTags.ListFullCopyOrNull<string>();
            pk.apparelAllowHeadgearChance = pkOld.apparelAllowHeadgearChance;   
            pk.aiAvoidCover = pkOld.aiAvoidCover;
            pk.apparelColor = pkOld.apparelColor;
            pk.apparelIgnoreSeasons = pkOld.apparelIgnoreSeasons;
            pk.apparelMoney = new FloatRange(min:pkOld.apparelMoney.min, max:pkOld.apparelMoney.max);
            pk.backstoryCategories = pkOld.backstoryCategories.ListFullCopyOrNull<string>();
            pk.backstoryFilters = pkOld.backstoryFilters.ListFullCopyOrNull<BackstoryCategoryFilter>();
            pk.backstoryFiltersOverride = pkOld.backstoryFiltersOverride.ListFullCopyOrNull<BackstoryCategoryFilter>();
            pk.backstoryCryptosleepCommonality = pkOld.backstoryCryptosleepCommonality;
            pk.baseRecruitDifficulty = pkOld.baseRecruitDifficulty;
            pk.biocodeWeaponChance = pkOld.biocodeWeaponChance;
            pk.canArriveManhunter = pkOld.canArriveManhunter;
            pk.canBeSapper = pkOld.canBeSapper;
            pk.chemicalAddictionChance = pkOld.chemicalAddictionChance;
            pk.combatEnhancingDrugsChance = pkOld.combatEnhancingDrugsChance;
            pk.combatEnhancingDrugsCount = pkOld.combatEnhancingDrugsCount;
            pk.combatPower = pkOld.combatPower;
            pk.defaultFactionType = pkOld.defaultFactionType;
            pk.description = pkOld.description;
            pk.defendPointRadius = pkOld.defendPointRadius;
            pk.destroyGearOnDrop = pkOld.destroyGearOnDrop;
            pk.disallowedTraits = pkOld.disallowedTraits.ListFullCopyOrNull<TraitDef>();
            pk.ecoSystemWeight = pkOld.ecoSystemWeight;
            pk.factionLeader = pkOld.factionLeader;
            pk.fixedInventory = pkOld.fixedInventory.ListFullCopyOrNull<ThingDefCountClass>();
            pk.fleeHealthThresholdRange = new FloatRange(min: pkOld.fleeHealthThresholdRange.min, max: pkOld.fleeHealthThresholdRange.max);
            pk.forceNormalGearQuality = pkOld.forceNormalGearQuality;
            pk.gearHealthRange = new FloatRange(min: pkOld.gearHealthRange.min, max: pkOld.gearHealthRange.max);
            pk.generated = pkOld.generated;
            pk.hairTags = pkOld.hairTags;
            pk.inventoryOptions = pkOld.inventoryOptions;
            pk.invFoodDef = pkOld.invFoodDef;
            pk.invNutrition = pkOld.invNutrition;
            pk.isFighter = pkOld.isFighter;
            pk.itemQuality = pkOld.itemQuality;
            pk.labelMale = pkOld.labelMale;
            pk.labelMalePlural = pkOld.labelMalePlural;
            pk.labelFemale = pkOld.labelFemale;
            pk.labelFemalePlural = pkOld.labelFemalePlural;
            pk.labelPlural = pkOld.labelPlural;
            pk.lifeStages = pkOld.lifeStages.ListFullCopyOrNull<PawnKindLifeStage>();
            pk.maxGenerationAge = pkOld.maxGenerationAge;
            pk.minGenerationAge = pkOld.minGenerationAge;
            pk.modContentPack = pkOld.modContentPack;
            pk.modExtensions = pkOld.modExtensions;
            pk.royalTitleChance = pkOld.royalTitleChance;
            pk.skills = pkOld.skills.ListFullCopyOrNull<SkillRange>(); ;
            pk.specificApparelRequirements = pkOld.specificApparelRequirements.ListFullCopyOrNull<SpecificApparelRequirement>(); ;
            pk.trader = pkOld.trader;
            pk.titleSelectOne = pkOld.titleSelectOne.ListFullCopyOrNull<RoyalTitleDef>(); ;
            pk.techHediffsRequired = pkOld.techHediffsRequired.ListFullCopyOrNull<ThingDef>(); ;
            pk.techHediffsChance = pkOld.techHediffsChance;
            pk.techHediffsMaxAmount = pkOld.techHediffsMaxAmount;
            pk.techHediffsMoney = new FloatRange(min: pkOld.techHediffsMoney.min, max: pkOld.techHediffsMoney.max);
            pk.techHediffsTags = pkOld.techHediffsTags.ListFullCopyOrNull<string>();
            pk.techHediffsDisallowTags = pkOld.techHediffsDisallowTags.ListFullCopyOrNull<string>();
            pk.weaponMoney = new FloatRange(min: pkOld.weaponMoney.min, max: pkOld.weaponMoney.max);
            pk.weaponTags = pkOld.weaponTags.ListFullCopyOrNull<string>();
            pk.wildGroupSize = pkOld.wildGroupSize;

            switch (label)
            {
                case ("Twilek"):
                    ThingDef_AlienRace twilekRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Twilek");
                    pk.race = twilekRace;
                    /*
                    
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
                            // if  in the white list then readd to the list of required apparel
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
                    */
                    break;
                case ("Togruta"):
                    ThingDef_AlienRace thisRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Togruta");
                    pk.race = thisRace;
                    pk.apparelAllowHeadgearChance = 0f;
                    /*

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
                            // if  in the white list then readd to the list of required apparel
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
                    */
                    break;
                case ("Wookie"):
                    ThingDef_AlienRace wookieRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Wookie");
                    pk.race = wookieRace;
                    /*
                    // wookies are usually naked except for the bandolier
                    // remove all required apparel except the bandolier
                    if (pk.apparelTags.NullOrEmpty<string>())
                    {
                        pk.apparelTags = new List<string>();
                    }
                    pk.apparelTags.Add("StarWarsRaces_WookieApparelTag");
                    */
                    break;
                case ("Ewok"):
                    ThingDef_AlienRace ewokRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Ewok");
                    pk.race = ewokRace;
                    /*
                    // ewoks are usually naked except for the traditional hood and wrap or some other primitive items
                    // remove all required apparel except those items
                    if (pk.apparelTags.NullOrEmpty<string>())
                    {
                        pk.apparelTags = new List<string>();
                    }
                    pk.apparelTags.Add("StarWarsRaces_EwokApparelTag");
                    */
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
