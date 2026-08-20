using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    // 0 - block types, 1 - colors
    ShopItem[] blocks;
    ShopItem[] colors;

    int[] selectedItems;

    void Start()
    {
        selectedItems = new int[2] { 0, 0 };
        blocks = new ShopItem[5] { new ShopItem(), new ShopItem(), new ShopItem(), new ShopItem(), new ShopItem() };

        blocks[0].Setup("BlockTriangle", null, 0, true);
        blocks[1].Setup("BlockRectangle", null, 10, false);
        blocks[2].Setup("BlockPentagon", null, 25, false);
        blocks[3].Setup("BlockHexagon", null, 50, false);
        blocks[4].Setup("BlockRound", null, 100, false);

    }

    public int ReadSetting(int _category)
    {
        return selectedItems[_category];
    }

    public void SettingDelta(int _category, int _delta)
    {
        selectedItems[_category] += _delta;
        int _length = 0;
        switch (_category)
        {
            default:
                _length = blocks.Length;
                break;
        }
        if (selectedItems[_category] < 0)
        {
            selectedItems[_category] = _length-1;
        }
        else if(selectedItems[_category] >= blocks.Length)
        {
            selectedItems[_category] = 0;
        }
    }

    public ShopItem ReadItem(int _category)
    {
        switch (_category)
        {
            default:
                return blocks[selectedItems[0]];
        }

    }

}

public class ShopItem
{
    int buyPrice;
    Sprite image;
    string name;
    bool unlocked;


    public void Setup(string _name, Sprite _image, int _buyPrice, bool _unlocked)
    {
        name = _name;
        image = _image;
        buyPrice = _buyPrice;
        unlocked = _unlocked;
    }

    public bool ReadUnlocked()
    {
        return unlocked;
    }

    public string ReadName()
    {
        return name;
    }

    public Sprite ReadImage()
    {
        return image;
    }

}