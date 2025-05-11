using UnityEngine;

public static class GameFormulas
{
    //Vero se l'elemento dell'arma è lo stesso della debolezza del difensore
    public static bool HasElementAdvantage(ELEMENT attackElement, Hero defender)
    {
        return attackElement == defender.GetWeakness();
    }

    //Vero se l'elemento dell'arma è lo stesso della resistenza del difensore
    public static bool HasElementDisadvantage(ELEMENT attackElement, Hero defender)
    {
        return attackElement == defender.GetResistance();
    }

    /*Logicamente potrebbe non avere senso avere la resistenza uguale alla debolezza
    ma non essendoci in consegna un controllore da inserire per impedire che siano uguali
    procedo in questo modo*/
    //Modificatore del danno
    public static float EvaluateElementalModifier(ELEMENT attackElement, Hero defender)
    {
        float vantage = 1f;

        if (HasElementAdvantage((ELEMENT)attackElement, defender))
        {
            vantage += 0.5f;
        }

        if (HasElementDisadvantage((ELEMENT)attackElement, defender))
        {
            vantage -= 0.5f;
        }

        return vantage;
    }

    //Valori di aim e eva troppo vicini portano ad avere MISS troppo spesso
    //Vero se l'attaccante colpisce il difensore
    public static bool HasHit(Stats attacker, Stats defender)
    {
        int hitChange = attacker.aim - defender.eva;

        if (Random.Range(0, 99) > hitChange)
        {
            Debug.Log("MISS");
            return false;
        }
        else
        {
            return true;
        }
    }

    //Vero se il colpo è critico
    public static bool IsCrit(int critValue)
    {
        if (Random.Range(0, 99) < critValue)
        {
            Debug.Log("CRIT");
            return true;
        }
        else
        {
            return false;
        }
    }

    //Restituisce il danno subito
    public static int CalculateDamage(Hero attacker, Hero defender)
    {
        //Somma delle statistiche tra Hero e Weapon
        Stats atkWepHero = Stats.Sum(attacker.GetHeroStats(), attacker.GetWeapon().GetWeaponStats());
        Stats defWepHero = Stats.Sum(defender.GetHeroStats(), defender.GetWeapon().GetWeaponStats());

        //Difesa usata da defence
        int defence = 0;
        if (attacker.GetWeapon().GetDmgType() == Weapon.DAMAGE_TYPE.PHYSICAL)
        {
            defence = defWepHero.def;
        }
        else
        {
            defence = defWepHero.res;
        }

        //Danno base subito
        int baseDamage = atkWepHero.atk - defence;

        //Applico al danno base il modificatore, assicurandomi che mi venga restituito DOPO il calcolo un int (hp di Hero è int)
        baseDamage = (int)(baseDamage * EvaluateElementalModifier(attacker.GetWeapon().GetElem(), defender));

        //Applico la logica del colpo critico
        if (IsCrit(atkWepHero.crt))
        {
            baseDamage *= 2;
        }

        //Restituisco sempre un numero positivo
        if (baseDamage < 0)
        {
            return 0;
        }
        else
        {
            return baseDamage;
        }
    }
}
