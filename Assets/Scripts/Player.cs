using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool key_up = false,
         key_down = false,
         key_left = false,
         key_right = false;

    float moveSpeed = 4;

    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        key_up = Input.GetKey(KeyCode.W);
        key_down = Input.GetKey(KeyCode.S);
        key_left = Input.GetKey(KeyCode.A);
        key_right = Input.GetKey(KeyCode.D);

        //Movement calculation
        float hSpeed = (key_right ? 1 : 0) - (key_left ? 1 : 0);
        float vSpeed = (key_up ? 1 : 0) - (key_down ? 1 : 0);
        hSpeed = hSpeed * moveSpeed * Time.deltaTime;
        vSpeed = vSpeed * moveSpeed * Time.deltaTime;
        Vector3 pos = transform.position;

        //Horizontal Collision Check
        if (Physics2D.OverlapBox(new Vector2(pos.x + hSpeed, pos.y), boxCollider.size, 0, LayerMask.GetMask("Wall")))
            hSpeed = 0;
        pos.x += hSpeed;
        //Vertical collision check
        if (Physics2D.OverlapBox(new Vector2(pos.x, pos.y + vSpeed), boxCollider.size, 0, LayerMask.GetMask("Wall")))
            vSpeed = 0;
        pos.y += vSpeed;
        transform.position = pos;


        //Calculate look angle
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float lookAngle = Mathf.Atan2(mouseWorldPosition.y - transform.position.y, mouseWorldPosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(lookAngle, Vector3.forward);
    }
}
