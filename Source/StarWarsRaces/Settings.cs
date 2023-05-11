using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace StarWarsRaces
{
    public class SettingsController : Mod
    {
        public static Settings Settings;

        public SettingsController(ModContentPack content) : base(content)
        {
            Settings = GetSettings<Settings>();
        }
        public override string SettingsCategory()
        {
            return "StarWarsRaces.SpeciesOfTheRim".Translate();
        }
        public override void DoSettingsWindowContents(Rect canvas) { Settings.DoWindowContents(canvas); }

    }
    public class Settings : ModSettings
    {
        private Vector2 scrollPosition;
        public SpeciesControl Twilek = new SpeciesControl("StarWarsRaces_Twilek");
        public SpeciesControl Togruta = new SpeciesControl("StarWarsRaces_Togruta");
        public SpeciesControl Wookiee = new SpeciesControl("StarWarsRaces_Wookiee");
        public SpeciesControl Ewok = new SpeciesControl("StarWarsRaces_Ewok");
        public SpeciesControl Rodian = new SpeciesControl("StarWarsRaces_Rodian");
        public void DoWindowContents(Rect canvas)
        {
            List<SpeciesControl> speciesList = new List<SpeciesControl> {
                Twilek,
                Togruta,
                Wookiee,
                Ewok,
                Rodian,
            };
            GUI.BeginGroup(position: canvas);
            Widgets.BeginScrollView(
                outRect: canvas,
                scrollPosition: ref this.scrollPosition,
                viewRect: new Rect(x: 0f, y: 0f, width: canvas.width, height: 55 * 55f)
            );

            float num = 6f;
            float grower = 40f;
            Text.Font = GameFont.Small;
            Rect rect;
            Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
            rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: 40f);
            Text.Font = GameFont.Medium;
            Widgets.Label(rect, "Colonist, Wanderer, Refugees, and Slaves");
            Text.Font = GameFont.Small;
            for (int x = 0; x < speciesList.Count; x++)
            {
                Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
                rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: grower);
                Widgets.Label(rect, speciesList[x].Label + " settings");
                Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
                rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: grower);
                Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " as possible colonists", ref speciesList[x].Colonist);
                rect = new Rect(x: 0f, y: num += grower, width: canvas.width - 16f, height: grower);
                Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " as possible wanderers", ref speciesList[x].Wanderer);
                rect = new Rect(x: 0f, y: num += grower, width: canvas.width - 16f, height: grower);
                Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " as possible refugees", ref speciesList[x].Refugee);
                rect = new Rect(x: 0f, y: num += grower, width: canvas.width - 16f, height: grower);
                Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " as potential slaves", ref speciesList[x].Slave);
            }
            Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
            rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: 40f);
            Text.Font = GameFont.Medium;
            Widgets.Label(rect, "Factions: Pirate, Outlander, and Tribals");
            Text.Font = GameFont.Small;
            for (int x = 0; x < speciesList.Count; x++)
            {
                Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
                rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: grower);
                Widgets.Label(rect, speciesList[x].Label + " settings");
                Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
                rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: grower);
                Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " in pirate faction", ref speciesList[x].Pirate);
                rect = new Rect(x: 0f, y: num += grower, width: canvas.width - 16f, height: grower);
                Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " in outlander factions", ref speciesList[x].Outlander);
                rect = new Rect(x: 0f, y: num += grower, width: canvas.width - 16f, height: grower);
                Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " in tribal factions", ref speciesList[x].Tribal);
            }
             if (Factions.IsStarWarsFactionsLoaded())
             {
                 Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
                 rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: 40f);
                 Text.Font = GameFont.Medium;
                 Widgets.Label(rect, "Star Wars Factions");
                 Text.Font = GameFont.Small;
                 Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
             }
             for (int x = 0; x < speciesList.Count; x++)
             {
                 if (Factions.IsStarWarsFactionsLoaded())
                 {
                     Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
                     rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: grower);
                     Widgets.Label(rect, speciesList[x].Label + " settings");
                     Widgets.DrawLineHorizontal(x: 0, y: num += grower, length: canvas.width);
                     rect = new Rect(x: 0f, y: num, width: canvas.width - 16f, height: grower);
                     Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " in Star Wars rebel faction", ref speciesList[x].Rebels);
                     rect = new Rect(x: 0f, y: num += grower, width: canvas.width - 16f, height: grower);
                     Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " in Star Wars bounty hunter faction", ref speciesList[x].Scum);
                     rect = new Rect(x: 0f, y: num += grower, width: canvas.width - 16f, height: grower);
                     Widgets.CheckboxLabeled(rect, "include " + speciesList[x].Label + " in Star Wars empire faction", ref speciesList[x].Empire);
                 }
             }
            Widgets.EndScrollView();
            GUI.EndGroup();
        }


        public float spawnChance = 20f;
        public override void ExposeData()
        {
            List<SpeciesControl> speciesList = new List<SpeciesControl> { 
                Twilek,
                Togruta,
                Wookiee,
                Ewok,
                Rodian,
            };
            base.ExposeData();
            Scribe_Values.Look(ref spawnChance, "StarWarsRaces.SpawnChance", 20f);
            for (int x = 0; x < speciesList.Count; x++)
            {
                string label = speciesList[x].Label.Replace('_', '.');
                Scribe_Values.Look(ref speciesList[x].Colonist, label + ".IncludeAsColonist", true);
                Scribe_Values.Look(ref speciesList[x].Wanderer, label + ".IncludeAsWanderer", true);
                Scribe_Values.Look(ref speciesList[x].Refugee, label + ".IncludeAsRefugee", true);
                Scribe_Values.Look(ref speciesList[x].Slave, label + ".IncludeAsSlave", false);

            }
            SpeciesControl t = speciesList.Find(x => x.Label == Twilek.Label);
            string replacedLabel = t.Label.Replace('_', '.');
            Scribe_Values.Look(ref t.Pirate, replacedLabel + ".IncludeAsPirate", true);
            Scribe_Values.Look(ref t.Outlander, replacedLabel + ".IncludeAsOutlander", false);
            Scribe_Values.Look(ref t.Tribal, replacedLabel + ".IncludeAsTribal", false);

            Scribe_Values.Look(ref t.Rebels, replacedLabel + ".IncludeAsRebels", true);
            Scribe_Values.Look(ref t.Scum, replacedLabel + ".IncludeAsBounty", true);
            Scribe_Values.Look(ref t.Empire, replacedLabel + ".IncludeAsEmpire", false);


            t = speciesList.Find(x => x.Label == Togruta.Label);
            replacedLabel = t.Label.Replace('_', '.');
            Scribe_Values.Look(ref t.Pirate, replacedLabel + ".IncludeAsPirate", true);
            Scribe_Values.Look(ref t.Outlander, replacedLabel + ".IncludeAsOutlander", false);
            Scribe_Values.Look(ref t.Tribal, replacedLabel + ".IncludeAsTribal", false);

            Scribe_Values.Look(ref t.Rebels, replacedLabel + ".IncludeAsRebels", true);
            Scribe_Values.Look(ref t.Scum, replacedLabel + ".IncludeAsBounty", true);
            Scribe_Values.Look(ref t.Empire, replacedLabel + ".IncludeAsEmpire", false);


            t = speciesList.Find(x => x.Label == Wookiee.Label);
            replacedLabel = t.Label.Replace('_', '.');
            Scribe_Values.Look(ref t.Pirate, replacedLabel + ".IncludeAsPirate", false);
            Scribe_Values.Look(ref t.Outlander, replacedLabel + ".IncludeAsOutlander", true);
            Scribe_Values.Look(ref t.Tribal, replacedLabel + ".IncludeAsTribal", true);

            Scribe_Values.Look(ref t.Rebels, replacedLabel + ".IncludeAsRebels", true);
            Scribe_Values.Look(ref t.Scum, replacedLabel + ".IncludeAsBounty", true);
            Scribe_Values.Look(ref t.Empire, replacedLabel + ".IncludeAsEmpire", false);


            t = speciesList.Find(x => x.Label == Rodian.Label);
            replacedLabel = t.Label.Replace('_', '.');
            Scribe_Values.Look(ref t.Pirate, replacedLabel + ".IncludeAsPirate", true);
            Scribe_Values.Look(ref t.Outlander, replacedLabel + ".IncludeAsOutlander", true);
            Scribe_Values.Look(ref t.Tribal, replacedLabel + ".IncludeAsTribal", false);

            Scribe_Values.Look(ref t.Rebels, replacedLabel + ".IncludeAsRebels", true);
            Scribe_Values.Look(ref t.Scum, replacedLabel + ".IncludeAsBounty", true);
            Scribe_Values.Look(ref t.Empire, replacedLabel + ".IncludeAsEmpire", false);


            t = speciesList.Find(x => x.Label == Ewok.Label);
            replacedLabel = t.Label.Replace('_', '.');
            Scribe_Values.Look(ref t.Pirate, replacedLabel + ".IncludeAsPirate", false);
            Scribe_Values.Look(ref t.Outlander, replacedLabel + ".IncludeAsOutlander", false);
            Scribe_Values.Look(ref t.Tribal, replacedLabel + ".IncludeAsTribal", true);

            Scribe_Values.Look(ref t.Rebels, replacedLabel + ".IncludeAsRebels", true);
            Scribe_Values.Look(ref t.Scum, replacedLabel + ".IncludeAsBounty", false);
            Scribe_Values.Look(ref t.Empire, replacedLabel + ".IncludeAsEmpire", false);
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                RaceSettingsUpdater.AdjustSpawnChance();
                Factions.AddAliensToNPCFactions();
            }
        }
    }
    public class SpeciesControl
    {
        public string Label;
        public SpeciesControl(string label)
        {
            Label = label;
        }
        public bool Colonist = true;
        public bool Wanderer = true;
        public bool Refugee = true;
        public bool Slave = true;
        public bool Pirate = true;
        public bool Outlander = false;
        public bool Tribal = false;
        public bool Rebels = true;
        public bool Scum = true;
        public bool Empire = false;
    }
}
