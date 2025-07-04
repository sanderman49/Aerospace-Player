using System.IO;
using System.Runtime.InteropServices.JavaScript;

namespace aeropad_player.Directory;

public class Aeropad
{
    public string[] Patches { get; set; }
    public string[] Scales { get; set; }
    public string[] Keys { get; set; }

    public Aeropad()
    {
        // Init each of the patches, scales and keys current available in the AeroPads Complete Collection.
        Patches = new string[10];
        // Atmosphere Soundpack
        Patches[0] = "Exosphere";
        Patches[1] = "Thermosphere";
        Patches[2] = "Mesosphere";
        Patches[3] = "Stratosphere";
        Patches[4] = "Troposphere";
        // Orbit Soundpack
        Patches[5] = "Gravity";
        Patches[6] = "Inertia";
        Patches[7] = "Satellite";
        Patches[8] = "Aphelion";
        Patches[9] = "Perihelion";

        Scales = new string[3];
        Scales[0] = "Major";
        Scales[1] = "Neutral";
        Scales[2] = "Minor";

        Keys = new string[12];
        Keys[0] = "C";
        Keys[1] = "C#";
        Keys[2] = "D";
        Keys[3] = "D";
        Keys[4] = "E";
        Keys[5] = "F";
        Keys[6] = "F#";
        Keys[7] = "G";
        Keys[8] = "G#";
        Keys[9] = "A";
        Keys[10] = "A#";
        Keys[11] = "B";

    }
    public string FindPad(string patch, string scale, string key)
    {
        string collection = "";

        // Logic to determine the soundpack. Will do something better later.
        for (int i = 0; i < Patches.Length; i++)
        {
            string p = Patches[i];

            if (patch == p && i < 5)
            {
                collection = "Atmosphere Soundpack";
            }
            else if (patch == p && i >= 5)
            {
                collection = "Orbit Soundpack";
            }
        }

        // The path to the desired soundfile.
        string padLocation = Path.Join(Config.GetPadLocation(), collection, "mp3", patch, scale, $"{key}.mp3");

        return padLocation;
    }
    
}