using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject hold_transform;
    public CapsuleCollider2D item_collider;

    public ParticleSystem ps;
    public Rigidbody2D player_rb;
    Rigidbody2D rb;
    bool is_held = false;
    bool is_shooting = false;
    Vector3 mousePos;
    Vector2 firedDirection;


    void Start()
    {
        item_collider = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        if (is_held)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // set item position to hold transform position
            this.transform.position = hold_transform.transform.position + new Vector3(0, 0, -1);

            // rotate item based on mouse position
            Vector2 direction = mousePos - this.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if(angle > 90 || angle < -90)
            {
                this.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                this.transform.localScale = new Vector3(1, 1, 1);
            }

            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            if(Input.GetMouseButton(0))
            {
                is_shooting = true;
                if(!ps.isPlaying) {ps.Play();}

                // add force to item.parent.parent.rigidbody2D in direction of mouse position

                Vector2 emitDirection = (mousePos - this.transform.position).normalized;
                player_rb.AddForce(-emitDirection * 10f, ForceMode2D.Force);
            } 
            else
            {
                is_shooting = false;
                ps.Stop();
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                is_held = false;
                this.transform.SetParent(null);
                
                // throw in direction of mouse position
                firedDirection = (mousePos - transform.position).normalized;

                rb.AddForce(firedDirection * 15f, ForceMode2D.Impulse);
                player_rb.AddForce(-firedDirection * 5f, ForceMode2D.Impulse);

            }


        }

        if (is_shooting && !is_held)
        {
            rb.AddForce(firedDirection * 10f, ForceMode2D.Force);
            RotateToFiringDirection();
        }



        if(!is_held && rb.linearVelocity.magnitude < 1.0f)
        {

            item_collider.enabled = true;
            if (rb.linearVelocity.magnitude < 0.1f)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    void RotateToFiringDirection()
    {
        float angle = Mathf.Atan2(firedDirection.y, firedDirection.x) * Mathf.Rad2Deg;
        angle += 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //check tag of collider
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Picked up item");
            is_held = true;
            // set item as child of hold transform
            this.transform.SetParent(hold_transform.transform);
            // set item position to local origin
            this.transform.localPosition = new Vector3(0, 0, -1);
            // disable item collider
            item_collider.enabled = false;
        }
    }
}
