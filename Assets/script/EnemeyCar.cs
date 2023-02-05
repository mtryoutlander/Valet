using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyCar : MonoBehaviour
{
    [SerializeField] float speed;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        this.rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveEnemy();
    }
    private void moveEnemy()
    {
        rb.velocity =(Vector3.forward*speed);
    }
}
