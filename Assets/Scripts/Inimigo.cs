using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inimigo : MonoBehaviour
{

    public abstract void Flip();
    public abstract void Dano(int damage);
    public abstract IEnumerator DanoCoroutine();
    public abstract IEnumerator ParadoRoutine();
    public abstract void Morte();
   
}