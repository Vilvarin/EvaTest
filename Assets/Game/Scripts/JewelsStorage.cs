using UnityEngine;
using System.Collections;

public class JewelsStorage : MonoBehaviour
{
    public static JewelsStorage instance;

    public Sprite blackJewel;
    public Sprite silverJewel;
    public Sprite blueJewel;
    public Sprite greenJewel;
    public Sprite yellowJewel;
    public Sprite redJewel;
    public Sprite pinkJewel;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
}
