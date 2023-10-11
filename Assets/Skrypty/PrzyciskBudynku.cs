using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PrzyciskBudynku : PrzyciskJednostki
{
    void UtworzBudynek()
    {
        KontrolerKamery.UtworzBudynek(prefabrykat);
    }

    protected override void UtworzJednostke()
    {
        //base.UtworzJednostke();
    }
}
