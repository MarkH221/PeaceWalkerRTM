using System.Collections.Generic;

namespace PWRTM
{
    internal static class Enums
    {
        public enum Sex
        {
            Male = 0x11,
            Female = 0x12
        }

        public static readonly string[] Type =
        {
            //Remember to +1
            "[VOL] Volunteer Soldier",
            "[COL] Collaboration Partner",
            "[TRD] Traded Staff",
            "[UNQ] Unique Soldier",
            "[COL] Collaboration Partner",
            "[POW] Former Prisoner",
            "[VOL] Volunteer Soldier",
            "[NML] Military Soldier"
        };

        public static readonly string[] Jobs =
        {
            //+1
            "Infantry",
            "Sharpshooter",
            "Commando",
            "Scout",
            "Guerrilla",
            "Elite Commando",
            "Mechanic",
            "Researcher",
            "Doctor",
            "Nurse",
            "Cook",
            "Medic",
            "Engineer",
            "Supply Soldier",
            "Industrial Spy",
            "Spy",
            "Food Technician",
            "Nutritionist",
            "Medical Researcher",
            "Big Boss",
            "High School Student",
            "MSF Subcommander",
            "KGB Agent",
            "AI Researcher",
            "Child Soldier",
            "FSLN Commander",
            "Bipedal Weapon Developer",
            "Ornithologist",
            "Normal",
            "Music Trooper",
            "UT (N425A)",
            "UT (N425B)",
            "UT (N425C)",
            "UT (N425D - NAVY BLUE)",
            "UT (N425E - WHITE)",
            "UT (N425E - BLACK)",
            "UT (N425F)",
            "UT (N425G - GRAY)",
            "UT (N425H - WHITE)",
            "UT (N425H - RED)",
            "UT (N425J)",
            "UT (N425L)",
            "UT (N425D - GRAY)",
            "UT (N425G - WHITE)",
            "UNIQLO Trooper",
            "FOX UNIT Member",
            "HORI Trooper",
            "Veteran Voice Actor",
            "New Voice Actor",
            "Game Designer",
            "UNIQLO Trooper"
        };

        public static readonly Dictionary<string, int> Skills = new Dictionary<string, int>
        {
            {"None", 0},
            {"Sidekick", 1},
            {"Radio Technology", 2},
            {"SWAT", 3},
            {"Rescue", 4},
            {"Decoy", 5},
            {"Engineering", 6},
            {"Channeler", 7},
            {"Voice Actor", 8},
            {"Green Beret", 9},
            {"Pro Wrestling Maniac", 0xc},
            {"Three-Star Chef", 0xe},
            {"Four-Star Chef", 0x10},
            {"Five-Star Chef", 0x11},
            {"Pharmacist", 0x12},
            {"Expert Pharmacist", 0x13},
            {"Counselor", 0x14},
            {"Physician", 0x16},
            {"Surgeon", 0x19},
            {"Gunsmith (Handguns)", 0x1b},
            {"Gunsmith (Shotguns)", 0x1c},
            {"Gunsmith (Assault Rifles)", 0x1d},
            {"Gunsmith (Machine Guns)", 0x1e},
            {"Gunsmith (Sniper Rifles)", 0x1f},
            {"Bipedal Weapons Design", 0x23},
            {"Gung Ho", 0x27},
            {"Optical Technology", 0x2A},
            {"FSLN Comandante", 0x2B},
            {"AI Development Technology", 0x2C},
            {"Bird Watcher", 0x2D},
            {"Home Cooking", 0x2E},
            {"Mother Base Deputy Commander", 0x2F},
            {"Gunsmith (Submachine Guns)", 0x30},
            {"Patriot", 0x31},
            {"Japanese Patriot", 0x32},
            {"Anti-tank Rifle Design", 0x33},
            {"M134 Design", 0x34},
            {"EM Weapons Design", 0x35},
            {"Metamaterials Technology", 0x36}
        };
    }
}