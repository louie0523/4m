using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hero_slot : MonoBehaviour
{
    public GameObject[] HerosSlot;
    public string[] hero_name;
    public string[] hero_explain;

    public TMP_Text nameText;
    public TMP_Text explainText;

    private int slot;

    void Start()
    {
        ShowSelectedSlot();
    }

    void Slot_reset()
    {
        foreach (GameObject obj in HerosSlot)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    void ShowSelectedSlot()
    {
        Slot_reset();

        if (slot >= 0 && slot < HerosSlot.Length)
        {
            if (HerosSlot[slot] != null)
            {
                HerosSlot[slot].SetActive(true);
            }

            nameText.text = hero_name[slot];
            explainText.text = hero_explain[slot];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            slot = Mathf.Max(0, slot - 1);
            ShowSelectedSlot();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            slot = Mathf.Min(HerosSlot.Length - 1, slot + 1);
            ShowSelectedSlot();
        }
    }
}
