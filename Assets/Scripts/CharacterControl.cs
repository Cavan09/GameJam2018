using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterControl : MonoBehaviour {

    CharacterMovement m_CharacterMovement { get; set; }
    GunController m_GunController { get; set; }
    Rigidbody2D m_Body_2D { get; set; }
    BoxCollider2D m_Collier { get; set; }
    [SerializeField]
    public GameObject m_Gun { get; set; }
    [SerializeField]
    public GameObject m_GunFollowPos { get; set; }

    // Use this for initialization
    void Start ()
    {
        m_CharacterMovement = GetComponent<CharacterMovement>();
        if(m_CharacterMovement == null)
            m_CharacterMovement = gameObject.AddComponent<CharacterMovement>();

        m_GunController = GetComponent<GunController>();
        if (m_GunController == null)
            m_GunController = gameObject.AddComponent<GunController>();

        m_Body_2D = GetComponent<Rigidbody2D>();
        if(m_Body_2D == null)
            m_Body_2D = gameObject.AddComponent<Rigidbody2D>();

        if (m_Gun == null)
        {
            m_Gun = GameObject.Find("Gun");
        }
        if (m_GunFollowPos == null)
        {
            m_GunFollowPos = GameObject.Find("GunFollowPos");
        }

        m_Collier = m_Gun.GetComponent<BoxCollider2D>();
        if (m_Collier == null)
            m_Collier = m_Gun.AddComponent<BoxCollider2D>();

    }
	
    void Update()
    {
        var thing = m_Gun.transform.TransformDirection(Vector3.right) * 10;
        Debug.DrawRay(m_Gun.transform.position, thing, Color.red);
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        //Hold the current value of the controls
        float horizontalMovement = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalMovement = CrossPlatformInputManager.GetAxis("Vertical");
        float drainFeedAmount = CrossPlatformInputManager.GetAxis("Triggers");

        Vector2 ViewAngle = new Vector2(CrossPlatformInputManager.GetAxis("View_X"), CrossPlatformInputManager.GetAxis("View_Y"));

        
        m_GunController.Aim(m_Gun, ViewAngle);   
        

        m_Gun.transform.position = new Vector3(m_GunFollowPos.transform.position.x, m_GunFollowPos.transform.position.y);
        m_CharacterMovement.MoveVertical(m_Body_2D, verticalMovement);
        m_CharacterMovement.MoveHorizontal(m_Body_2D, horizontalMovement, ViewAngle.x);

        m_GunController.HittingTarget(m_Gun);

        if(drainFeedAmount > 0.1f)
        {
            var gunPos = new Vector3(m_Gun.transform.localPosition.x + (m_Collier.bounds.extents.x - m_Gun.transform.localPosition.x), m_Collier.bounds.center.y, 0);
            m_GunController.Push(drainFeedAmount, m_Gun.transform.position);
        }
        else if(drainFeedAmount < -0.1f)
        {
            var gunPos = new Vector3(m_Gun.transform.position.x + m_Collier.bounds.extents.x, m_Gun.transform.position.x + m_Collier.bounds.center.y, 0);
            m_GunController.Pull(drainFeedAmount, m_Gun.transform.position);
        }
        else
        {
            m_GunController.ResetRender();
        }
    }
}
