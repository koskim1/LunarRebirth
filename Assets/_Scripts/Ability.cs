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
    // �ɷ� ȿ�� ����
    // �뽬�Ҷ� ���ο��� �ɸ��°� �����ؼ� �������
}
