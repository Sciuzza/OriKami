using UnityEngine;
using System.Collections;
namespace Orikami
{
    public class DestroyableObjects : MonoBehaviour
    {


        void Awake()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().brokeSomething.AddListener(DestroyingMySelf);
        }



        private void DestroyingMySelf()
        {
            Destroy(this.gameObject);
        }
    }
}
