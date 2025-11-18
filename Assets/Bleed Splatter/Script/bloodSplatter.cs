using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodSplatter : MonoBehaviour
{
    public GameObject blood1;
    public GameObject blood2;
    public GameObject blood3;
    public GameObject flashes;
    public GameObject heartBeat;
    private bool canBleed = true;
    public bool bleedNow;
    public bool heartBeatNow;
    void Start()
    {
    }
    public void bleed()
    {
        flashes.gameObject.GetComponent<bleeding>().isBleeding = true;
        if (canBleed)
        {
            int i = Random.Range(1, 4);
            switch (i)
            {
                case 1:
                    if (!blood1.gameObject.GetComponent<bleeding>().isBleeding)
                        blood1.gameObject.GetComponent<bleeding>().isBleeding = true;
                    else bleed();
                    break;

                case 2:
                    if (!blood2.gameObject.GetComponent<bleeding>().isBleeding)
                        blood2.gameObject.GetComponent<bleeding>().isBleeding = true;
                    else bleed();
                    break;
                case 3:
                    if (!blood3.gameObject.GetComponent<bleeding>().isBleeding)
                        blood3.gameObject.GetComponent<bleeding>().isBleeding = true;
                    else bleed();
                    break;

            }
        }
    }
    void Update()
    {
        if (blood1.gameObject.GetComponent<bleeding>().isBleeding &&
            blood2.gameObject.GetComponent<bleeding>().isBleeding &&
            blood3.gameObject.GetComponent<bleeding>().isBleeding)
            canBleed = false;
        else canBleed = true;
        if (bleedNow)
        {
            bleed();
            bleedNow = false;
        }
        if (heartBeatNow)
            heartBeat.gameObject.GetComponent<Animator>().SetBool("bleed", true);
        else heartBeat.gameObject.GetComponent<Animator>().SetBool("bleed", false);

    }
}
