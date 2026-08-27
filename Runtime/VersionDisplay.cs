using UnityEngine;
using TMPro;

/// <summary>
/// Puts the current version of the game into a TMP_Text.
/// The current version is taken from Project Settings > Player > Version.
/// </summary>
public class VersionDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text versionText;

    void Start()
    {
        // Grabs the string from Project Settings > Player > Version
        string gameVersion = Application.version; 
        
        if (versionText != null)
        {
            versionText.text = $"{gameVersion}";
        }
    }
}