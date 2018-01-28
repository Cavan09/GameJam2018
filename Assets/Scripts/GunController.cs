using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    LayerMask m_Mask = new LayerMask();
    public GameObject m_Target { get; set; }
    public Vector3 m_HitPoint { get; set; }
    LineRenderer m_Renderer { get; set; }
    public int m_LengthOfLineRenderer = 20;

    public float StoredMomentum = 0;

    public Color m_StartPullColor = Color.blue;
    public Color m_EndPullColor = Color.green;

    public Color m_StartPushColor = Color.yellow;
    public Color m_EndPushColor = Color.red;

    // Use this for initialization
    void Start () {
        m_Mask.value = LayerMask.GetMask("Toys");
        m_Renderer = GetComponent<LineRenderer>();
        if(m_Renderer == null)
        {
            m_Renderer = gameObject.AddComponent<LineRenderer>();
        }

        #region SetUp Line Renderer
        m_Renderer.material = new Material(Shader.Find("Particles/Additive"));

        m_Renderer.startWidth = 1.0f;
        m_Renderer.endWidth = 1.0f;

        m_Renderer.widthMultiplier = 0.1f;
        m_Renderer.positionCount = m_LengthOfLineRenderer;

        m_Renderer.sortingLayerID = 0;
        #endregion
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_Target == null)
        {
            m_Renderer.enabled = false;
        }
	}

    public void Aim(GameObject gun, Vector2 viewAngle)
    {
        int dir = viewAngle.x > 0 ? 1 : -1;
        if(viewAngle.x == 0)
        {
            dir = transform.localScale.x > 0 ? 1 : -1;
            gun.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(viewAngle.y * -1, viewAngle.x * dir) * 180 / Mathf.PI);
        }
        else
        {
            gun.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(viewAngle.y * -1, viewAngle.x) * 180 / Mathf.PI);
        }
        gun.transform.localScale = new Vector3(Mathf.Abs(gun.transform.localScale.x), gun.transform.localScale.y, gun.transform.localScale.z);        
        
    }

    public void Pull(float drain, Vector3 gun)
    {
        if (m_Target == null)
            return;

        var toy = m_Target.GetComponent<Toy>();

        if (toy == null || toy.m_Momentum <= 0)
        {
            m_Renderer.enabled = false;
            return;
        }
            
        StoredMomentum += toy.UpdateMometum(drain * Time.deltaTime);
        SetRendererColor(m_StartPullColor, m_EndPullColor);
        var center = m_Target.GetComponent<BoxCollider2D>().bounds.center;
        Shoot(m_HitPoint, gun);

        UpdateColor(Color.blue);
    }

    public void Push(float feed, Vector3 gun)
    {
        if (m_Target == null)
            return;

        var toy = m_Target.GetComponent<Toy>();

        if (toy == null || toy.m_Momentum >= 2 || StoredMomentum <= 0)
        {
            m_Renderer.enabled = false;
            return;
        }

        StoredMomentum += toy.UpdateMometum(feed * Time.deltaTime);
        SetRendererColor(m_StartPushColor, m_EndPushColor);
        var center = m_Target.GetComponent<BoxCollider2D>().bounds.center;
        Shoot(gun, m_HitPoint);

        UpdateColor(Color.red);
    }

    public void Shoot(Vector3 to, Vector3 from)
    {
        var startPos = to;
        var endPos = from;
        var direction = (endPos - startPos);

        for(int i = 0; i < m_LengthOfLineRenderer; i++)
        {
            float posNumber = i;
            var pos = startPos + ((posNumber / m_LengthOfLineRenderer) * direction);
            m_Renderer.SetPosition(i, pos);
        }

        m_Renderer.enabled = true;
    }

    private void SetRendererColor(Color start, Color end)
    {
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(start, 0.0f), new GradientColorKey(end, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        m_Renderer.colorGradient = gradient;
    }

    public void HittingTarget(GameObject gun)
    {
        RaycastHit2D hit = Physics2D.BoxCast(gun.transform.position, new Vector2(gun.transform.localScale.x, gun.transform.localScale.y), Vector2.Angle(transform.position, gun.transform.position), gun.transform.right, 100.0f, m_Mask);
        if (hit.collider != null)
        {
            var target = hit.collider.gameObject;
            m_Target = hit.collider.gameObject;
            m_HitPoint = new Vector3(hit.point.x, hit.point.y, 0);

            UpdateColor(Color.green);
        }
        else
        {
            UpdateColor(Color.black);
            m_Target = null;
        }
    }

    private void UpdateColor(Color color)
    {
        if (m_Target != null)
        {
            var outline = m_Target.GetComponent<SpriteOutline>();
            if (outline != null)
            {
                outline.color = color;
            }
        }
    }

    public void ResetRender()
    {
        m_Renderer.enabled = false;
    }

    public void AnimateColor(GameObject Gun)
    {

    }
}
