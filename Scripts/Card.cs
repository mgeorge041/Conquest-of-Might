using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject {

    public string cardName;
    public string cardDesc;

    public Sprite artwork;

    public int attack;
    public int health;
    public int might;
    public int range;
    public int speed;

    public int res1Cost;
    public int res2Cost;
}
