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
        
        void Update()
        {
            if(Input.anyKey)
            {
                SceneManager.LoadScene("Login");
            }
        }
    }
}