using UnityEngine;
using System.Collections;

public class MoveToPlayer : MonoBehaviour {
    public float speed;
    private ZSH zsh;
    void Start () {
        GameObject ZSH = GameObject.FindWithTag("Player");
        Rigidbody rgbody = GetComponent<Rigidbody>();
        if (ZSH != null)
            zsh = ZSH.GetComponent<ZSH>();
        if (zsh != null)
        {
            float x1 = transform.position.x;
            float z1 = transform.position.z;
            float x2 = zsh.transform.position.x;
            float z2 = zsh.transform.position.z;
            if (Mathf.Abs(z1 - z2) > 1e-6)
            {
                float a = Mathf.Atan2(x2 - x1, z1 - z2);
                transform.localEulerAngles = new Vector3(0, 180 - a * 180 / Mathf.PI, 0);
            }       
        }
        rgbody.velocity = transform.forward * speed;
    }
}
