using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public string allowTransform = null;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnMouseDown()
    {
        if (allowTransform != null)
        {
            PlayerController pc = GameObject.Find(allowTransform)?.GetComponent<PlayerController>();
            if (pc)
            {
                print(pc);
                print(gameObject.GetComponent<SpriteRenderer>().sprite);
                pc.AddObject(gameObject.GetComponent<SpriteRenderer>().sprite);
                // pc.objs1?.Add(gameObject.GetComponent<SpriteRenderer>().sprite);
                Debug.Log("Object clicked!");
            }
        }
    }
}
