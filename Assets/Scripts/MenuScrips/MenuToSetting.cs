using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToSetting : MonoBehaviour
{
    public void OnClickSettingButton()
    {
        SceneManager.LoadScene("SettingScene");
    }
}