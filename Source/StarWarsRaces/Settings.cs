using Verse;
using UnityEngine;
using AlienRace;

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
        public string[] RaceIdentif = new string[] { "Twilek", "Rodian", "Wookie", "Ewok", "Togruta" };
        public float[] SpawnChance = new float[] { 5f, 5f, 5f, 5f, 5f };
        private string[] spawnChanceString = new string[] { "", "", "", "", "" };




        private Vector2 pos = new Vector2(0, 0);
        public void DoWindowContents(Rect canvas)
        {
            Listing_Standard list = new Listing_Standard
            {
                ColumnWidth = canvas.width - 20
            };
            list.Begin(canvas);


            Rect scrollView = new Rect(canvas.x, canvas.y, canvas.width, canvas.height);
            list.BeginScrollView(canvas, ref pos, ref scrollView);

            list.Gap(60);
            for (int i = 0; i < RaceIdentif.Length; i++)
            {
                string include = "StarWarsRaces.Include" + RaceIdentif[i];
                list.DrawSlider(include.Translate() + " " + GetspawnChanceLabel(SpawnChance[i]),
                    ref SpawnChance[i], ref spawnChanceString[i], 0f, 10f);
                list.Gap(24);
            }
            list.EndScrollView(ref scrollView);
            list.End();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            for (int i = 0; i < RaceIdentif.Length; i++)
            {
                string nameVal = "spawnChance" + RaceIdentif[i];
                Scribe_Values.Look(ref SpawnChance[i], nameVal, 6f);

                if (Scribe.mode == LoadSaveMode.PostLoadInit)
                {
                    spawnChanceString[i] = ((int)SpawnChance[i]).ToString();
                }
            }
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                SettingsUtil.LoadAllSpecies(RaceIdentif, SpawnChance);
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
    }

    public static class SettingsUtil
    {
        public static void DrawSlider(this Listing_Standard list, string label, ref float value, ref string buffer, float min, float max)
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
        public static string ModTextEntryLabeled(this Listing_Standard ls, string label, string buffer, int lineCount = 1)
        {
            Rect rect = ls.GetRect(Text.LineHeight * (float)lineCount);
            Widgets.Label(new Rect(rect.x, rect.y, rect.width - 75, rect.height), label);
            string result = Widgets.TextField(new Rect(rect.xMax - 65, rect.y, 65, rect.height), buffer);
            ls.Gap(ls.verticalSpacing);
            return result;
        }
        public static void LoadAllSpecies(string[] labels, float[] chances)
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
                                    if (kd.Contains(label))
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
                                    if (kd.Contains(label))
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
                                if (kd.Contains(label))
                                {
                                    ark.chance = chances[i] * 10;

                                };
                            };
                        };
                        foreach (PawnKindEntry ask in rs.pawnKindSettings.alienslavekinds)
                        {
                            foreach (string kd in ask.kindDefs)
                            {
                                if (kd.Contains(label))
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
}

