using Verse;
using UnityEngine;
using Harmony;
using AlienRace;
using System;
using System.Reflection;

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
        public float spawnChanceTwilek = 6f;
        public float spawnChanceRodian = 6f;
        public float spawnChanceWookie = 6f;
        public string strSpawnTwilek = "";
        public string strSpawnRodian = "";
        public string strSpawnWookie = "";


        public void DoWindowContents(Rect canvas)
        {
            Listing_Standard list = new Listing_Standard
            {
                ColumnWidth = canvas.width
            };
            list.Begin(canvas);

            list.Gap();
            DrawSlider(list, "StarWarsRaces.IncludeTwilek".Translate() + " " + GetspawnChanceLabel(spawnChanceTwilek), ref spawnChanceTwilek, ref strSpawnTwilek, 0.0f, 10f);
            list.Gap(24);
            DrawSlider(list, "StarWarsRaces.IncludeRodian".Translate() + " " + GetspawnChanceLabel(spawnChanceRodian), ref spawnChanceRodian, ref strSpawnRodian, 0.0f, 10f);
            list.Gap(24);
            DrawSlider(list, "StarWarsRaces.IncludeWookie".Translate() + " " + GetspawnChanceLabel(spawnChanceWookie), ref spawnChanceWookie, ref strSpawnWookie, 0.0f, 10f);
            list.Gap(24);
            list.End();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref spawnChanceTwilek, "spawnChanceTwilek", 6f);
            Scribe_Values.Look(ref spawnChanceRodian, "spawnChanceRodian", 6f);
            Scribe_Values.Look(ref spawnChanceWookie, "spawnChanceWookie", 6f);

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                strSpawnTwilek = ((int)spawnChanceTwilek).ToString();
                strSpawnRodian = ((int)spawnChanceRodian).ToString();
                strSpawnWookie = ((int)spawnChanceWookie).ToString();
            }
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                string[] labels = new string[] { "Twilek", "Rodian", "Wookie" };
                float[] chances = new float[] { spawnChanceTwilek, spawnChanceRodian, spawnChanceWookie };
                LSUtil.LoadASpecies(labels, chances);
            }
        }
        private static string GetspawnChanceLabel(float spawnChance)
        {
            if (spawnChance <= 0.0)
            {
                return "StarWarsRaces.spawnChanceVeryLow".Translate();
            }
            else if (spawnChance < .75)
            {
                return "StarWarsRaces.spawnChanceLow".Translate();
            }
            else if (spawnChance < 1.5)
            {
                return "StarWarsRaces.spawnChanceNormal".Translate();
            }
            else if (spawnChance < 4)
            {
                return "StarWarsRaces.spawnChanceHigh".Translate();
            }
            else if (spawnChance < 8)
            {
                return "StarWarsRaces.spawnChanceVeryHigh".Translate();
            }
            return "StarWarsRaces.spawnChanceInsane".Translate();
        }
        public static void DrawSlider(Listing_Standard list, string label, ref float value, ref string buffer, float min, float max)
        {
            float f;
            string s = buffer;
            buffer = list.ModTextEntryLabeled(label, buffer);
            if (!s.Equals(buffer))
            {
                if (float.TryParse(buffer, out f))
                {
                    if (f > 0)
                        value = f;
                }
            }

            f = value;
            value = list.Slider(value, min, max);
            if (f != value)
            {
                buffer = ((int)value).ToString();
            }
        }
    }

    public static class LSUtil
    {
        public static string ModTextEntryLabeled(this Listing_Standard ls, string label, string buffer, int lineCount = 1)
        {
            Rect rect = ls.GetRect(Text.LineHeight * (float)lineCount);
            Widgets.Label(new Rect(rect.x, rect.y, rect.width - 75, rect.height), label);
            string result = Widgets.TextField(new Rect(rect.xMax - 65, rect.y, 65, rect.height), buffer);
            ls.Gap(ls.verticalSpacing);
            return result;
        }
        public static void LoadASpecies(string[] labels, float[] chances)
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
                                foreach (string kd in pke.kindDefs)
                                {
                                    if (kd == "StarWarsRaces_" + label + "Colonist" || kd == "StarWarsRaces_" + label + "Tribesperson")
                                    {
                                        pke.chance = chances[i] * 10;
                                    };
                                };
                            };
                        };
                        foreach (FactionPawnKindEntry awk in rs.pawnKindSettings.alienwandererkinds)
                        {
                            foreach (PawnKindEntry pke in awk.pawnKindEntries)
                            {
                                foreach (string kd in pke.kindDefs)
                                {
                                    if (kd == "StarWarsRaces_" + label + "Colonist" || kd == "StarWarsRaces_" + label + "Tribesperson")
                                    {
                                        pke.chance = chances[i] * 10;

                                    };
                                };
                            };
                        };
                        foreach (PawnKindEntry ark in rs.pawnKindSettings.alienrefugeekinds)
                        {
                            foreach (string kd in ark.kindDefs)
                            {
                                if (kd == "StarWarsRaces_" + label + "Colonist" || kd == "StarWarsRaces_" + label + "Tribesperson")
                                {
                                    ark.chance = chances[i] * 10;

                                };
                            };
                        };
                        foreach (PawnKindEntry ask in rs.pawnKindSettings.alienslavekinds)
                        {
                            foreach (string kd in ask.kindDefs)
                            {
                                if (kd == "StarWarsRaces_" + label + "Colonist" || kd == "StarWarsRaces_" + label + "Tribesperson")
                                {
                                    ask.chance = chances[i] * 10;

                                };
                            };
                        };
                        break;

                    }
                }
            }
        }
    }
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
            string[] labels = new string[] { "Twilek", "Rodian", "Wookie" };
            float[] chances = new float[] { SettingsController.Settings.spawnChanceTwilek, SettingsController.Settings.spawnChanceRodian, SettingsController.Settings.spawnChanceWookie };
            LSUtil.LoadASpecies(labels, chances);
        }
    }
}

