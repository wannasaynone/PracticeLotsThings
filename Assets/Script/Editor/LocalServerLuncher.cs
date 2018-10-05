using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class LocalServerLuncher
{
	[MenuItem("Tools/Lunch Local Server")]
    public static void Lunch()
    {
        Process _lunchServerPrecess = new Process();
        _lunchServerPrecess.StartInfo.FileName = Application.dataPath + "/LocalServer/practicelotsthings-pairserver.exe";
        _lunchServerPrecess.Start();
    }
}
