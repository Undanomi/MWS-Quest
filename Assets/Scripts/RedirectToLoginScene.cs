using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class RedirectToLoginScene : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Login");
        }
    }
}