using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordTile : MonoBehaviour
{
    [SerializeField] TMP_Text Character;
    [SerializeField] SpriteRenderer SpriteRenderer;

    string _word;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Refresh(string word, bool selected)
    {
        if(word.Length > 0)
            _word = word;
        Character.text = _word;

        SpriteRenderer.color = selected ? Color.black : Color.white;
    }

}
