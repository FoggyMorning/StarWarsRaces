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
                                if (pke.kindDefs.Any(k => k.label.Contains(label))) 
                                {
                                    pke.chance = chances[i] * 10;
                                };
                            };
                        };
                        foreach (FactionPawnKindEntry awk in rs.pawnKindSettings.alienwandererkinds)
                        {
                            foreach (PawnKindEntry pke in awk.pawnKindEntries)
                            {
                                if (pke.kindDefs.Any(k => k.label.Contains(label)))
                                {
                                        pke.chance = chances[i] * 10;
                                    };
                            };
                        };
                        foreach (PawnKindEntry pke in rs.pawnKindSettings.alienrefugeekinds)
                        {
                            if (pke.kindDefs.Any(k => k.label.Contains(label)))
                            {
                                pke.chance = chances[i] * 10;
                            };
                        };
                        foreach (PawnKindEntry pke in rs.pawnKindSettings.alienslavekinds)
                        {
                            if (pke.kindDefs.Any(k => k.label.Contains(label)))
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
                        f.pawnGroupMakers[x] = AddPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInOutlander)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("OutlanderCivil", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = AddPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInOutlander)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("OutlanderRough", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = AddPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInTribal)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("TribeCivil", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = AddPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInTribal)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("TribeRough", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = AddPawnKindsToFactions(g[x], labels[i]);
                    }
                }
                if (SettingsController.Settings.IncludeInTribal)
                {
                    FactionDef f = DefDatabase<FactionDef>.GetNamed("TribeSavage", true);
                    PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
                    for (int x = 0; x < g.Length; x++)
                    {
                        f.pawnGroupMakers[x] = AddPawnKindsToFactions(g[x], labels[i]);
                    }
                }

                DefDatabase<FactionDef>.AddAllInMods();
                if (Factions.IsStarWarsFactionsLoaded())
                {
                    FactionDef rebFact = DefDatabase<FactionDef>.GetNamed("PJ_RebelFac", true);
                    PawnGroupMaker[] pgmsReb = rebFact.pawnGroupMakers.ToArray();
                    for (int j = 0; j < pgmsReb.Length; j++)
                    {
                        rebFact.pawnGroupMakers[j] = AddPawnKindsToFactions(pgmsReb[j], labels[i]);
                    }

                    FactionDef scumFact = DefDatabase<FactionDef>.GetNamed("PJ_Bounty", true);
                    PawnGroupMaker[] pgmsScum = scumFact.pawnGroupMakers.ToArray();
                    for (int j = 0; j < pgmsScum.Length; j++)
                    {
                        scumFact.pawnGroupMakers[j] = AddPawnKindsToFactions(pgmsScum[j], labels[i]);
                    }
                }
            }
        }

 
        private static PawnGroupMaker AddPawnKindsToFactions(PawnGroupMaker pgm, string label)
        {
                // groups can be either an options or a guards for whatever reason, so check for both.
                PawnGenOption[] options = pgm.options.ToArray();
                for (int i = 0; i < options.Length; i++)
                {
                    PawnGenOption pgo = MakePawnGenOption(options[i], label);
                    if (pgo != null) { pgm.AddPawn(pgo, false); }
                }
                PawnGenOption[] guards = pgm.guards.ToArray();
                for (int j = 0; j < guards.Length; j++)
                {
                    PawnGenOption pgo = MakePawnGenOption(guards[j], label);
                    if (pgo != null) { pgm.AddPawn(pgo, true); }
                }
            return pgm;
        }
        private static void AddPawn(this PawnGroupMaker pgm, PawnGenOption pgo, bool isTrader = false)
        {
            if (pgo.kind == null) { return; };
            if (isTrader)
            {
                pgm.guards.Add(pgo);
                return;
            }
            pgm.options.Add(pgo);
        }
        private static PawnGenOption MakePawnGenOption(PawnGenOption existing, string label)
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
            PawnKindDef pkOld = PawnKindDef.Named(pawnKindLabel);
            if (pkOld.factionLeader)
            {
                return null;
            }
            CreateNewPawnKind(pkOld, label, defname);
            return new PawnGenOption
            {
                selectionWeight = sw,
                kind = PawnKindDef.Named(defname)
            };
        }
        
        private static void CreateNewPawnKind(PawnKindDef pkOld, string label, string defname)
        {
            // if it already exists then don't recreate it
            if (DefDatabase<PawnKindDef>.GetNamedSilentFail(defname) != null)
            {
                return;
            }
            PawnKindDef pk = new PawnKindDef
            {
                defName = defname,
                label = pkOld.label + " (" + label + ")",

                allowRoyalApparelRequirements = pkOld.allowRoyalApparelRequirements,
                allowRoyalRoomRequirements = pkOld.allowRoyalRoomRequirements,
                alternateGraphicChance = pkOld.alternateGraphicChance,
                alternateGraphics = pkOld.alternateGraphics.ListFullCopyOrNull<AlternateGraphic>(),
                apparelRequired = pkOld.apparelRequired.ListFullCopyOrNull<ThingDef>(),
                apparelDisallowTags = pkOld.apparelDisallowTags.ListFullCopyOrNull<string>(),
                apparelTags = pkOld.apparelTags.ListFullCopyOrNull<string>(),
                apparelAllowHeadgearChance = pkOld.apparelAllowHeadgearChance,
                aiAvoidCover = pkOld.aiAvoidCover,
                apparelColor = pkOld.apparelColor,
                apparelIgnoreSeasons = pkOld.apparelIgnoreSeasons,
                apparelMoney = new FloatRange(min: pkOld.apparelMoney.min, max: pkOld.apparelMoney.max),
                backstoryCategories = pkOld.backstoryCategories.ListFullCopyOrNull<string>(),
                backstoryFilters = pkOld.backstoryFilters.ListFullCopyOrNull<BackstoryCategoryFilter>(),
                backstoryFiltersOverride = pkOld.backstoryFiltersOverride.ListFullCopyOrNull<BackstoryCategoryFilter>(),
                backstoryCryptosleepCommonality = pkOld.backstoryCryptosleepCommonality,
                baseRecruitDifficulty = pkOld.baseRecruitDifficulty,
                biocodeWeaponChance = pkOld.biocodeWeaponChance,
                canArriveManhunter = pkOld.canArriveManhunter,
                canBeSapper = pkOld.canBeSapper,
                chemicalAddictionChance = pkOld.chemicalAddictionChance,
                combatEnhancingDrugsChance = pkOld.combatEnhancingDrugsChance,
                combatEnhancingDrugsCount = pkOld.combatEnhancingDrugsCount,
                combatPower = pkOld.combatPower,
                defaultFactionType = pkOld.defaultFactionType,
                description = pkOld.description,
                defendPointRadius = pkOld.defendPointRadius,
                destroyGearOnDrop = pkOld.destroyGearOnDrop,
                disallowedTraits = pkOld.disallowedTraits.ListFullCopyOrNull<TraitDef>(),
                ecoSystemWeight = pkOld.ecoSystemWeight,
                factionLeader = pkOld.factionLeader,
                fixedInventory = pkOld.fixedInventory.ListFullCopyOrNull<ThingDefCountClass>(),
                fleeHealthThresholdRange = new FloatRange(min: pkOld.fleeHealthThresholdRange.min, max: pkOld.fleeHealthThresholdRange.max),
                forceNormalGearQuality = pkOld.forceNormalGearQuality,
                gearHealthRange = new FloatRange(min: pkOld.gearHealthRange.min, max: pkOld.gearHealthRange.max),
                generated = pkOld.generated,
                inventoryOptions = pkOld.inventoryOptions,
                invFoodDef = pkOld.invFoodDef,
                invNutrition = pkOld.invNutrition,
                isFighter = pkOld.isFighter,
                itemQuality = pkOld.itemQuality,
                labelMale = pkOld.labelMale,
                labelMalePlural = pkOld.labelMalePlural,
                labelFemale = pkOld.labelFemale,
                labelFemalePlural = pkOld.labelFemalePlural,
                labelPlural = pkOld.labelPlural,
                lifeStages = pkOld.lifeStages.ListFullCopyOrNull<PawnKindLifeStage>(),
                maxGenerationAge = pkOld.maxGenerationAge,
                minGenerationAge = pkOld.minGenerationAge,
                modContentPack = pkOld.modContentPack,
                modExtensions = pkOld.modExtensions,
                royalTitleChance = pkOld.royalTitleChance,
                skills = pkOld.skills.ListFullCopyOrNull<SkillRange>()
            };
            pk.specificApparelRequirements = pkOld.specificApparelRequirements.ListFullCopyOrNull<SpecificApparelRequirement>();
            pk.trader = pkOld.trader;
            pk.titleSelectOne = pkOld.titleSelectOne.ListFullCopyOrNull<RoyalTitleDef>(); 
            pk.techHediffsRequired = pkOld.techHediffsRequired.ListFullCopyOrNull<ThingDef>(); 
            pk.techHediffsChance = pkOld.techHediffsChance;
            pk.techHediffsMaxAmount = pkOld.techHediffsMaxAmount;
            pk.techHediffsMoney = new FloatRange(min: pkOld.techHediffsMoney.min, max: pkOld.techHediffsMoney.max);
            pk.techHediffsTags = pkOld.techHediffsTags.ListFullCopyOrNull<string>();
            pk.techHediffsDisallowTags = pkOld.techHediffsDisallowTags.ListFullCopyOrNull<string>();
            pk.weaponMoney = new FloatRange(min: pkOld.weaponMoney.min, max: pkOld.weaponMoney.max);
            pk.weaponTags = pkOld.weaponTags.ListFullCopyOrNull<string>();
            pk.wildGroupSize = pkOld.wildGroupSize;
            pk.initialResistanceRange = pkOld.initialResistanceRange.GetValueOrDefault();
            pk.initialWillRange = pkOld.initialWillRange.GetValueOrDefault();


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
                case ("Wookiee"):
                    ThingDef_AlienRace WookieeRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Wookiee");
                    pk.race = WookieeRace;
                    /*
                    // Wookiees are usually naked except for the bandolier
                    // remove all required apparel except the bandolier
                    if (pk.apparelTags.NullOrEmpty<string>())
                    {
                        pk.apparelTags = new List<string>();
                    }
                    pk.apparelTags.Add("StarWarsRaces_WookieeApparelTag");
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
        }
        
    }
}
