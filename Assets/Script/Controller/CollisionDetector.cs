using UnityEngine;

public class CollisionDetector {

    public static bool DectectCollision(CapsuleCollider capcaol, string layerName = "Default", Vector3 adjust = default(Vector3))
    {
        // CapsuleCollider = 兩顆大球相連的膠囊體，
        // m_collisionPoint_up = 上方的球圓心
        // m_collisionPoint_down = 下方的球圓心
        // 高度 = 兩球圓心距離 + 圓半徑 * 2 (也就是中間的圓柱體長度 = 兩球圓心距離 - 圓半徑 * 2)
        // 求真正的中心點位置 -> 加減高度的一半取得頂點位置 -> 加減圓半徑取得球心位置
        // 中心點位置 = GameObject位置 + Center位移

        Vector3 collisionPoint_up = capcaol.transform.position + capcaol.center + Vector3.up * (capcaol.height / 2) - Vector3.up * capcaol.radius;
        Vector3 collisionPoint_down = capcaol.transform.position + capcaol.center - Vector3.up * (capcaol.height / 2) + Vector3.up * capcaol.radius;

        collisionPoint_up += adjust;
        collisionPoint_down += adjust;

        Debug.DrawLine(collisionPoint_up + Vector3.up * capcaol.radius, collisionPoint_down - Vector3.up * capcaol.radius, Color.red);

        Collider[] collisions = Physics.OverlapCapsule(collisionPoint_up, collisionPoint_down, capcaol.radius, LayerMask.GetMask(layerName));
        /* if (collisions.Length == 0)
        {
            // 無碰撞時
        }
        else
        {
            for (int i = 0; i < collisions.Length; i++)
            {
                // 有碰撞時
            }
        }*/
        return collisions.Length != 0;
    }
}
