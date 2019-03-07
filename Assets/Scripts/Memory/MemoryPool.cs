using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPool : System.IDisposable
{
    //-------------------------------------------------------------------------------------
    // 아이템 클래스
    //-------------------------------------------------------------------------------------
    class Item
    {
        public bool active; //사용중인지 여부
        public GameObject gameObject;
    }
    List<Item> table;

    //-------------------------------------------------------------------------------------
    // 메모리 풀 생성
    // original : 미리 생성해 둘 원본소스
    // count : 풀 최고 갯수
    //-------------------------------------------------------------------------------------
    public void Create(Object original, int count, Transform parent)
    {
        Dispose();
        table = new List<Item>();
    
        for (int i = 0; i < count; i++)
        {
            Item item = new Item();
            item.active = false;
            item.gameObject = Object.Instantiate(original) as GameObject;
            item.gameObject.SetActive(false);
            item.gameObject.transform.SetParent(parent);
            table.Add(item);
        }
    }
    //-------------------------------------------------------------------------------------
    // 새 아이템 요청 - 쉬고 있는 객체를 반납한다.
    //-------------------------------------------------------------------------------------
    public GameObject NewItem()
    {
        if (table == null)
            return null;
        int count = table.Count;
        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            if (item.active == false)
            {
                item.active = true;
                item.gameObject.SetActive(true);
                return item.gameObject;
            }
        }

        return null;
    }
    //--------------------------------------------------------------------------------------
    // 아이템 사용종료 - 사용하던 객체를 쉬게한다.
    // gameOBject : NewItem으로 얻었던 객체
    //--------------------------------------------------------------------------------------
    public void RemoveItem(GameObject gameObject, Object original, Transform parent)
    {
        if (table == null || gameObject == null)
            return;

        int count = table.Count;
        Item item = null;

        for (int i = 0; i < count; i++)
        {
            item = table[i];
            if (item.gameObject == gameObject)
            {

                item.active = false;
                item.gameObject.SetActive(false);

                return;
            }
        }

        item = new Item();
        item.gameObject = gameObject;
        item.active = false;
        item.gameObject.SetActive(false);
        item.gameObject.transform.SetParent(parent);
        table.Add(item);

        return;
    }
    //--------------------------------------------------------------------------------------
    // 모든 아이템 사용종료 - 모든 객체를 쉬게한다.
    //--------------------------------------------------------------------------------------
    public void ClearItem()
    {
        if (table == null)
            return;
        int count = table.Count;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            if (item != null && item.active)
            {
                item.active = false;
                item.gameObject.SetActive(false);
            }
        }
    }
    //--------------------------------------------------------------------------------------
    // 메모리 풀 삭제
    //--------------------------------------------------------------------------------------
    public void Dispose()
    {
        if (table == null)
            return;
        int count = table.Count;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            GameObject.Destroy(item.gameObject);
        }
        table = null;
    }

}