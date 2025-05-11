using UnityEngine;
//Lo uso per uscire dal RunTime più tardi dall'editor
#if UNITY_EDITOR
using UnityEditor;
#endif
//Più corto da scrivere
using GF = GameFormulas;

public class M1ProjectTest : MonoBehaviour
{
    public Hero A;
    public bool randomizeA;
    private Stats sumA;

    public Hero B;
    public bool randomizeB;
    private Stats sumB;

    private bool startA;
    private int turno = 0;
    private int dannoUsaEGetta;   //Uso una variabile globale per ottimizzare la memoria

    void Start()
    {
        //Verifico che i dati inseriti nell'Inspector siano consistenti
        A.ControlValue();
        B.ControlValue();

        //
        Debug.Log($"Vita <b>{A.GetName()}</b>: {A.GetHp()}");
        Debug.Log($"Vita <b>{B.GetName()}</b>: {B.GetHp()}");

        //Sommo le statistiche tra Hero e Weapon
        sumA = Stats.Sum(A.GetHeroStats(), A.GetWeapon().GetWeaponStats());
        sumB = Stats.Sum(B.GetHeroStats(), B.GetWeapon().GetWeaponStats());

        //Valuto chi comincia il turno, la velocità non cambia perciò lo calcolo solo una volta all'inizio
        startA = sumA.spd >= sumB.spd;
    }

    //Hero attacker attaccante, Hero defender difensore
    private void Round(Hero attacker, Stats attackerAllStats, Hero defender, Stats defenderAllStats)
    {
        Debug.Log($"Attacca <b>{attacker.GetName()}</b>, difende <b>{defender.GetName()}</b>");

        //Controllo se attacker colpisce defender
        if (GF.HasHit(attackerAllStats, defenderAllStats))
        {
            //Controllo se attacker attacca un nemico con debolezza elementare
            if (GF.HasElementAdvantage(attacker.GetWeapon().GetElem(), defender))
            {
                Debug.Log("WEAKNESS");
            }
            //Controllo se A attacca un nemico con resistenza elementare
            if (GF.HasElementDisadvantage(attacker.GetWeapon().GetElem(), defender))
            {
                Debug.Log("RESIST");
            }

            //Calcolo il danno
            dannoUsaEGetta = GF.CalculateDamage(attacker, defender);
            Debug.Log($"Danno inflitto: {dannoUsaEGetta}");
            //Applico il danno
            defender.TakeDamage(dannoUsaEGetta);

            //Debug aggiuntivo per leggibilità della console
            Debug.Log($"HP di {defender.GetName()} restanti: <b>{defender.GetHp()}</b>");

            //Controllo se gli hp di defender sono uguali a zero
            if (!defender.IsAlive())
            {
                Debug.Log($"Il vincitore è: <b>{attacker.GetName()}</b>");
            }
        }
    }

    void Update()
    {
        if (A.IsAlive() && B.IsAlive())
        {
            //Entrambi gli Hero sono vivi
            Debug.Log($"════════════════════ TURNO {turno++} ═══════════════════════");
            //Imposto l'ordine di esecuzione
            if (startA)
            {
                Round(A, sumA, B, sumB);
                if (B.IsAlive())
                {
                    Round(B, sumB, A, sumA);
                }
            }
            else
            {
                Round(B, sumB, A, sumA);
                if (A.IsAlive())
                {
                    Round(A, sumA, B, sumB);
                }
            }
        }
        else
        {
            //Esco dall'esecuzione, il duello è finito
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }

    //Funzione chiamata solo in Editor per creare un Hero casuale
    private Hero Randomize()
    {
        string nome = (randomizeA) ? "WeaponA" : "WeaponB";
        Stats bonusStats = new Stats(-1, -1, -1, -1, -1, -1, -1);
        Weapon weapon = new Weapon(nome, Weapon.DAMAGE_TYPE.PHYSICAL, ELEMENT.NONE, bonusStats);

        nome = (randomizeA) ? "HeroA" : "HeroB";
        Stats baseStats = new Stats(-1, -1, -1, -1, -1, -1,-1);
        return new Hero(nome, 0, baseStats, ELEMENT.NONE, ELEMENT.NONE, weapon);
    }

    void OnValidate()
    {
        if (randomizeA)
        {
            A = Randomize();
            randomizeA = false;
        }

        if (randomizeB)
        {
            B = Randomize();
            randomizeB = false;
        }
    }
}
