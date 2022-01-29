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
            if (ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Star Wars - Factions" || m.Name == "Star Wars - Factions (Continued)"))
            {
                loadedFactions = true;
                return;
            }
            return;
        }
        public static void AddAliensToNPCFactions()
        {
            UpdateNPCFactions(ref SettingsController.Settings.Twilek);
            UpdateNPCFactions(ref SettingsController.Settings.Ewok);
            UpdateNPCFactions(ref SettingsController.Settings.Rodian);
            UpdateNPCFactions(ref SettingsController.Settings.Togruta);
            UpdateNPCFactions(ref SettingsController.Settings.Wookiee);
        }
        private static void UpdateNPCFactions(ref SpeciesControl speciesControl)
        {
            if (speciesControl.Pirate)
            {
                UpdateNPCFaction("Pirate", speciesControl.Label);
            }
            if (speciesControl.Outlander)
            {
                UpdateNPCFaction("OutlanderCivil", speciesControl.Label);
                UpdateNPCFaction("OutlanderRough", speciesControl.Label);
            }
            if (speciesControl.Tribal)
            {
                UpdateNPCFaction("TribeCivil", speciesControl.Label);
                UpdateNPCFaction("TribeRough", speciesControl.Label);
                UpdateNPCFaction("TribeSavage", speciesControl.Label);
            }

            DefDatabase<FactionDef>.AddAllInMods();
            if (Factions.IsStarWarsFactionsLoaded())
            {
                if (speciesControl.Rebels)
                {
                    UpdateNPCFaction("PJ_RebelFac", speciesControl.Label);
                }
                if (speciesControl.Scum)
                {
                    UpdateNPCFaction("PJ_Bounty", speciesControl.Label);
                }
                if (speciesControl.Empire)
                {
                    UpdateNPCFaction("PJ_GalacticEmpire", speciesControl.Label);
                }
            }
        }
        private static void UpdateNPCFaction(string factionName, string label)
        {
            FactionDef f = DefDatabase<FactionDef>.GetNamed(factionName, true);
            PawnGroupMaker[] g = f.pawnGroupMakers.ToArray();
            for (int x = 0; x < g.Length; x++)
            {
                f.pawnGroupMakers[x] = AddPawnKindsToFactions(g[x], label);
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
            string existingDefName = existing.kind.defName;

            PawnKindDef pkOld = PawnKindDef.Named(existingDefName);
            // if it is one of our defs then don't recreate it
            // if some alien race other than a Human then don't risk copying it
            if (existingDefName.Contains("StarWarsRaces") || pkOld.race != ThingDefOf.Human || pkOld.factionLeader)
            {
                return null;
            }
            string defname = "StarWarsRaces_" + label + "_" + existingDefName;
            CreateNewPawnKind(pkOld, label, defname);
            return new PawnGenOption
            {
                selectionWeight = existing.selectionWeight,
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
                label = pkOld.label + " (" + label.ToLower() + ")",

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
                case "Togruta":
                    ThingDef_AlienRace thisRace = DefDatabase<ThingDef_AlienRace>.GetNamed("StarWarsRaces_Togruta");
                    pk.race = thisRace;
                    pk.apparelAllowHeadgearChance = 0f;
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
