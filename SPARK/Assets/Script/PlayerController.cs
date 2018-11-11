﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController> {
    public float PlayerSpeed;
    public int PlayerMoveFlag = 1;
    public bool PlayerActive;
    
    public GameObject NowItem;
    public GameObject mainCamera;
    public Vector2 mousePosition;
    public Vector2 targetPosition;
    [SerializeField]
    ItemBagController itemBagController;
    [SerializeField]
    Rigidbody2D rig;
    public void SetPlayerActive(bool condition)
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
            itemBagController.PutInItemBag(NowItem);
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
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, PlayerSpeed);
            if (transform.position.x == targetPosition.x)
            {
                PlayerRotationUpdata();
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, PlayerSpeed);
                PlayerSearchMouse();
            }
        }

    }

    /// <summary>
    /// プレイヤーが目標地点に到達したか
    /// </summary>
    /// <returns></returns>
    public bool IsEnterTargetPosition() {
        return Mathf.Abs(targetPosition.x - transform.position.x) < 0.001f;
    }

    private void PlayerRotationUpdata() {
        if (IsEnterTargetPosition()) { return; }
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

    /// <summary>
    /// プレイヤーの移動を待機する処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForMove(System.Action comp) {
        yield return null;
        while (!IsEnterTargetPosition()) {
            yield return null;
        }

        comp();
    }
}
