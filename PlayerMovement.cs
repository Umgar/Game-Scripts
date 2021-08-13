using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float playerSpeed;
    Vector3 mousePosition;
    public Weapon weapon;
    Vector2 movement;
    float xPos;

    void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
    }
    

    public void Look() {
        Vector2 mpos = InputSystem.GetDevice<Pointer>().position.ReadValue();
        mousePosition = Camera.main.ScreenToWorldPoint(mpos);
        mousePosition =  mousePosition - this.transform.position;
        xPos = mousePosition.x;
        if(xPos>=0) xPos = 1;
        else xPos = -1;
        Turn(xPos);
    }
    protected void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + movement * playerSpeed * Time.fixedDeltaTime);
    }
    public void ButtonPress(InputAction.CallbackContext context)
    {
        if(weapon == null) return;
        if(context.action.name == "Fire")
            weapon.Attack();
        else if(context.action.name == "Special")
            weapon.Special();
    }
        
    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    
    void Turn(float xPos)
    {
        Vector3 rot = this.transform.rotation.eulerAngles;
        rot = new Vector3(rot.x,xPos>0?0:180,rot.z);
        this.transform.rotation = Quaternion.Euler(rot);
    }
    public void StopAttack()
    {
        weapon.attacking = false;
        weapon.StopAttack();
    }
    public void SpecialAttackAnimationEnd()
    {
        weapon.StopSpecial();
    }
}
