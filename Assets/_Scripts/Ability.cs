using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string name;
    public string description;
    public Effect effect;

    public Ability(string name, string description, Effect effect)
    {
        this.name = name;
        this.description = description;
        this.effect = effect;
    }
}

public class Effect
{
    // 능력 효과 구현
    // 대쉬할때 슬로우모션 걸리는거 연동해서 띄워보기
}
