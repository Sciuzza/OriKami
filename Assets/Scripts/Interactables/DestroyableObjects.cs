using UnityEngine;
using System.Collections;

    public class DestroyableObjects : MonoBehaviour
    {


        void Awake()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlCore>().brokeSomething.AddListener(DestroyingMySelf);
        }

    
        public void DestroyingMySelf()
        {
            Destroy(this.gameObject);
        }
    }

