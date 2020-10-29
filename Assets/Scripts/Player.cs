using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameManager manager;
    public float life;
    public float maxLife;
    private Rigidbody _rb;
    public float force;
    public GameObject consoleObj;
    public Console console;
    public Text consoleText;

    private void Awake()
    {
        console.AddCommand("Heal", Heal, "Change your health. Introduce an integer number.");
        console.AddCommand("Knockback", Knockback, "Modify knockback force. Introduce a number that represents the desired force(0 will nulify knockback).");
        console.AddCommand("IgnoreGravity", IgnoreGravity, "Player will not be affected by gravity. Parameters: true ; false");
        console.AddCommand("IgnoreDamage", IgnoreDamage, "Player won't receive damage. Parameters: true ; false");
    }
    // Start is called before the first frame update
    void Start()
    {
        maxLife = life;
        _rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            manager.Checkpoint();
            life -= 10;
            if (life <= 0)
                FallLose();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("TutorialFloor"))
            manager.Checkpoint();


    }
    public void TakeDamage(float damage)
    {
        life -= damage;
        _rb.AddForce(transform.forward * -1 * force);
        if (life <= 0)
            Killed();
    }

    private void FallLose()
    {
        manager.ChangeScene(2);
    }

    private void Killed()
    {
        manager.ChangeScene(3);
    }

    private void Heal(List<string> data)
    {
        if (data.Count > 0)
        {
            life += int.Parse(data[0]);
            consoleText.text += "\n" + "Life healed.";
        }
    }

    private void Knockback(List<string> data)
    {
        if (data.Count > 0)
        {
            force = int.Parse(data[0]);
            consoleText.text += "\n" + "New knockback force value: " + force + ".";
        }
    }

    private void IgnoreGravity(List<string> data)
    {
        if (data.Count > 0)
        {
            _rb.useGravity = bool.Parse(data[0]);
            if (_rb.useGravity)
                consoleText.text += "\n" + "Player gravity activated.";
            else consoleText.text += "\n" + "Player gravity deactivated.";
        }
            
    }

    private void IgnoreDamage(List<string> data)
    {
        if (data.Count > 0)
        {
            if (bool.Parse(data[0]))
                gameObject.layer = LayerMask.NameToLayer("Default");
            else gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
