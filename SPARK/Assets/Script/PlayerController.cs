﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float PlayerSpeed;
    public int PlayerMoveFlag = 1;
    public bool PlayerActive;
    
    public GameObject NowItem;
    public GameObject mainCamera;
    public Vector2 mousePosition;
    public Vector2 targetPosition;
    [SerializeField]
    ItemBagControllr itemBagControllr;
    [SerializeField]
    Rigidbody2D rig;
    void SetPlayerActive(bool condition)
    {
        PlayerActive = condition;
    }
    //当たり判定によるアイテム調査
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Item") {
            NowItem = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == NowItem) {
            NowItem = null;
        }
    }
    void PlayerSearchMouse()
    {
        if (NowItem != null)
        {
            SetPlayerActive(false);
            /*アイテム調査動作をここに入れる

            */
            itemBagControllr.PutInItemBag(NowItem);
            Destroy(NowItem);
            PlayerActive = true;
        }
    }

    //調査終了によるプレイヤー移動可能になる
    public void PlayerSearchMouseOver()
    {
        SetPlayerActive(true);
    }
    //マオス移動
    void PlayerMoveMouse(float moveSpeed)
    {
        Vector3 Position = this.GetComponent<Transform>().position;
        if (PlayerActive)
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = Input.mousePosition;
                if(mousePosition.x>=0.0f&& mousePosition.x<=1920.0f&& mousePosition.y >= 0.0f && mousePosition.y <= 880.0f)
                {
                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    targetPosition = new Vector2(mousePosition.x, transform.position.y);
                }
            }
            if (targetPosition != null)
            {
                PlayerRotationUpdata();
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, PlayerSpeed);

                if (transform.position.x == targetPosition.x)
                {
                    PlayerSearchMouse();
                }
            }
        }

    }

    private void PlayerRotationUpdata() {
        if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.001f) { return; }
        //進行方向に向く
        Vector3 scale = transform.localScale;
        if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        else if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }
    }

    public void PlayerUpdata()
    {
        if (PlayerActive == true)
        {
            PlayerMoveMouse(PlayerSpeed);
        }
            if (Input.GetKey(KeyCode.Space))
            {
                SetPlayerActive(true);
            }
        
    }
    // Use this for initialization
    void Start () {
		
	}
}