using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public Rigidbody2D thisRB;
    public float mvSpd, accel;
    public float jmpForce;
    public float mvInput;

    public bool isFaceRight = true, isGrounded, isJump, isClimbing = false;

    public Transform grndChkLoc;
    public float grndChkRad;
    public LayerMask grndMaskLayer;

    public int jmpCnt, ExtraJmp;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Update()
    {

        if (!isClimbing && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))
        {
            if (jmpCnt >= 0)
            {
                thisRB.velocity = new Vector2(thisRB.velocity.x, jmpForce); //+= Vector2.up * jmpForce;
                jmpCnt--;
            }
            // else if (jmpCnt == 0 && isGrounded)
        }
        if (Input.GetKeyDown(KeyCode.S) && canJmpThru.Count > 0)
        {
            int i = 0, j = 0; float chkDIst = Mathf.Infinity;
            foreach (platformJumpThrough thisPlat in canJmpThru)
            {
                if (Vector2.Distance(transform.position, thisPlat.transform.position) < chkDIst)
                {
                    chkDIst = Vector2.Distance(transform.position, thisPlat.transform.position);
                    j = i;
                }
                i++;
            }
            StartCoroutine(fncJmpThru(canJmpThru[j]));
        }

    }
    //
    void FixedUpdate()
    {
        // isGrounded = Physics2D.OverlapCircle(grndChkLoc.position, grndChkRad, grndMaskLayer);
        if (isGrounded) jmpCnt = ExtraJmp;
        mvInput = Input.GetAxis("Horizontal");
        thisRB.velocity = new Vector2(mvInput * mvSpd, thisRB.velocity.y);
        if (isClimbing)
        { thisRB.velocity = new Vector2(thisRB.velocity.x*.7f, Input.GetAxis("Vertical") * mvSpd); }

        // if (!isFaceRight == false && mvInput > 0) { fncFlip(); }
        //              isRT   isRF
        //      mvD F   F       T
        //      mvD T   T       F
        if (isFaceRight != (mvInput > 0 && (mvInput != 0))) fncFlip();

    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        fncChkGrounded();
        if (col.transform.TryGetComponent(out platformJumpThrough objJmpThru))
        {
            canJmpThru.Add(objJmpThru);
            print("Contact with Jump Thru platform");
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        fncChkGrounded();
        if (col.transform.TryGetComponent(out platformJumpThrough objJmpThru))
        {
            canJmpThru.Remove(objJmpThru);
            print("End of contact with Jump Thru platform");
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.TryGetComponent(out ladderScript thisLadder))
        {
            isClimbing = true;
            thisRB.velocity = new Vector2(thisRB.velocity.x, thisRB.velocity.y * .1f);
            thisRB.gravityScale = 0;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.TryGetComponent(out ladderScript thisLadder))
        {
            isClimbing = false;
            // thisRB.velocity = Vector2.one * .5f;
            thisRB.gravityScale = 1;
        }
    }
    void fncChkGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(grndChkLoc.position, grndChkRad, grndMaskLayer);
    }
    public List<platformJumpThrough> canJmpThru = new List<platformJumpThrough>();
    IEnumerator fncJmpThru(platformJumpThrough jmpThru)
    {
        // bool isOverlap;
        // Vector2 plyrExtent = GetComponent<BoxCollider2D>().bounds.size;
        // Rect plyrBounds = new Rect(GetComponent<BoxCollider2D>().size,;
        // Vector2 deltaPos = transform.position - jmpThru.transform.position, jmpThruBounds = jmpThru.GetComponent<BoxCollider2D>().bounds.extents;
        // deltaPos = (deltaPos.sqrMagnitude > jmpThruBounds.sqrMagnitude) ? (deltaPos.normalized*jmpThruBounds): deltaPos;
        // while(jmpThru.GetComponent<BoxCollider2D>().bounds.Contains()// Physics2D.OverlapBox(transform.position,plyrExtent,))
        BoxCollider2D plyrCol = GetComponent<BoxCollider2D>(), jmpThruCol = jmpThru.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(plyrCol, jmpThruCol, true);
        yield return new WaitForSeconds(1f);
        while (plyrCol.bounds.Contains(jmpThruCol.transform.position) && jmpThruCol.bounds.Contains(transform.position))
        {
            yield return new WaitForFixedUpdate();
        }
        Physics2D.IgnoreCollision(plyrCol, jmpThruCol, false);
        if (canJmpThru.Contains(jmpThru))
            canJmpThru.Remove(jmpThru);
    }

    void fncFlip()
    {
        isFaceRight = !isFaceRight;
    }
}