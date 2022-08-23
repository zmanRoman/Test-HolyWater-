using UnityEngine;

public sealed class MenuHandler : MonoBehaviour
{
    public void QuitAplication()
    {
        Application.Quit();
    }
    public void OpenLink(string link)
    { 
        Application.OpenURL(link);
    }
}
