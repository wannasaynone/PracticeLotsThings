using System.Threading;
using System.Collections;
using UnityEngine;

public class Coroutiner {

    public class WaitForSeconds
    {
        public static float Seconds { get; private set; }

        public WaitForSeconds(float second)
        {
            Seconds = second;
        }
    }

    public static void StartCoroutine(IEnumerator coroutine)
    {
        while (coroutine.MoveNext())
        {
            if(coroutine.Current == null)
            {

            }

            if(coroutine.Current is WaitForSeconds)
            {
                System.Threading.Timer timer = new System.Threading.Timer(DoAfterTimer, null, 0, 1000);
            }
        }
    }

    private static void DoAfterTimer(object state)
    {
        int count = 0;
        while (count < 100)
        {
            Debug.Log(count);
        }
    }

}
