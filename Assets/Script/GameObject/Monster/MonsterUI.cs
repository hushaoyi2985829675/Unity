using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : MonoBehaviour
{
    GameObject hpBar;
    Monster monster;
    GameObject hpBarRef;
    private void Start()
    {
         monster = GetComponent<Monster>();        
    }

    private void Update()
    {
        if (monster.detectPlayer)
        {
            if (!hpBar)
            {
                createHpBar();
                setMaxHp();
            }
            hpBar.SetActive(true);
            var canvasPos = Camera.main.WorldToScreenPoint(new Vector2(transform.localPosition.x, transform.localPosition.y + 1.7f));
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.getCanvas().GetComponent<RectTransform>(), canvasPos, Camera.main, out localPos);
            hpBar.GetComponent<RectTransform>().localPosition = localPos;
        }
        else
        {
            hpBar?.SetActive(false);
        }
    }

    public void setMaxHp()
    {
        hpBar.GetComponent<Slider>().maxValue = monster.monsterValue.MaxHp;
        hpBar.GetComponent<Slider>().value = monster.monsterValue.Hp;
    }
    public void setHp(float hp)
    {
        hpBar.GetComponent<Slider>().value = hp;
    }

    public void createHpBar()
    {
        hpBarRef = UIManager.Instance.getMonsterHpBar();
        hpBar = Instantiate<GameObject>(hpBarRef,UIManager.Instance.getCanvas()); 
    }

    public void delectHpBar()
    {
        Destroy(hpBar);
    }
}
