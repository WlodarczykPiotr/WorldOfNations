using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrzyciskBudynku : PrzyciskJednostki
{
    public void UtworzBudynek()
    {
        KontrolerKamery.UtworzBudynek(prefabrykat);
    }

    public override void UtworzJednostke()
    {
        //base.UtworzJednostke();
    }
}
