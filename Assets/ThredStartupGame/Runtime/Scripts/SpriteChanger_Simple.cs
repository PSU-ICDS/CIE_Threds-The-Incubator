using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger_Simple : MonoBehaviour
{
    [SerializeField] bool active;
    [SerializeField] Image targetImage;
    [SerializeField] List<TaggedSprite> sprites;

    [Serializable]
    public class TaggedSprite
    {
        public string name;
        public Sprite sprite;

        public bool NameMatch(string _name)
        {
            if (SpriteCheck())
            {
                if (_name.ToLower() == name.ToLower())
                    return true;
            }

            return false;
        }

        public bool SpriteCheck()
        {
            if (sprite != null)
                return true;

            return false;
        }
    }

    public void Sprite_ApplyByName(string _name)
    {
        if (active && targetImage != null)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] != null && sprites[i].NameMatch(_name))
                {
                    targetImage.sprite = sprites[i].sprite;
                    break;
                }
            }
        }
    }

    public void Sprite_ApplyByIndex(int _index)
    {
        if (active && targetImage != null)
        {
            if (_index >= 0 && _index < sprites.Count)
            {
                if (sprites[_index] != null && sprites[_index].SpriteCheck())
                    targetImage.sprite = sprites[_index].sprite;
            }
        }
    }


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
