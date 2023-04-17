using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appearHelmet : MonoBehaviour
{
    private MeshRenderer mesh_renderer;

    private BoxCollider box_collider;

    private Rigidbody rigid_body;
    
    private PickUpItem pick_up_item;
    
    // Start is called before the first frame update
    void Start()
    {
        mesh_renderer = gameObject.GetComponentInChildren<MeshRenderer>();
        box_collider = gameObject.GetComponent<BoxCollider>();
        rigid_body = gameObject.GetComponent<Rigidbody>();
        pick_up_item = gameObject.GetComponent<PickUpItem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Fait apparaître le casque lorsque tous les ennemis sont morts
        if (GameObject.FindWithTag("Enemy") ==  null)
        {
            mesh_renderer.enabled = true;
            box_collider.enabled = true;
            rigid_body.useGravity = true;
            pick_up_item.enabled = true;
        }
    }
}
