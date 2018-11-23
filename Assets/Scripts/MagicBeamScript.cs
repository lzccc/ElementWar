using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MagicBeamScript : MonoBehaviour
{
    

    public static MagicBeamScript beamEffect;
    [Header("Prefabs")]
    public GameObject[] beamLineRendererPrefab;
    public GameObject[] beamStartPrefab;
    public GameObject[] beamEndPrefab;
    private int beamIndex = 0;
    [Header("法杖的顶点")]
    public Transform startSendPos;
    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture
    
    private void Awake()
    {
        beamEffect = this;
    }

    // Use this for initialization
    void Start()
    {

    }
    private Enemy enemy;
    // Update is called once per frame
    void Update()
    {
        if (saveSkill == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            beamStart = Instantiate(beamStartPrefab[beamIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beamEnd = Instantiate(beamEndPrefab[beamIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beam = Instantiate(beamLineRendererPrefab[beamIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            if (saveSkill.skillId[0]==13)
            {
                //beamEnd.GetComponent<ParticleSystem>().main.startSizeMultiplier
                //beam.GetComponent<LineRenderer>().startWidth = 1.7f;
                //beam.GetComponent<LineRenderer>().endWidth = 1.7f;
            }
            line = beam.GetComponent<LineRenderer>();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Destroy(beamStart);
            Destroy(beamEnd);
            Destroy(beam);
        }

        if (Input.GetMouseButton(0) && FireController.isFire == true)
        {
            float num = saveSkill.flySpeed * saveSkill.flyTime;
            Vector3 tdir =  transform.forward.normalized * num;
            ShootBeamInDir(startSendPos.position, tdir);
            if (saveSkill.canThrough)//可以穿透的话表现力改表
            {
                RaycastHit[] hits;
                hits = Physics.SphereCastAll(startSendPos.position, saveSkill.sizeScale, transform.forward, saveSkill.flySpeed * saveSkill.flyTime);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.CompareTag(CharacterType.Enemy.ToString()))
                    {
                        enemy = hits[i].collider.gameObject.GetComponent<Enemy>();
                        saveSkill.UseSkill(hits[i].collider.gameObject, enemy, Vector3.zero);
                        if (enemy.attributeType == ElementAttributeType.Soil)//免疫状态
                        {
                            return;
                        }
                        float repel = -(saveSkill.flySpeed / 150f) * PlayerController.player.skillLv[1];
                        enemy.gameObject.transform.position -= transform.forward.normalized * repel;
                    }
                    else if (hits[i].collider.CompareTag(CharacterType.Arrow.ToString()))
                    {
                        if (saveSkill.canAttackArrow)
                        {
                            saveSkill.UseSkill(hits[i].collider.gameObject, enemy, transform.forward);
                            Destroy(hits[i].collider.gameObject);
                        }
                    }
                }
            }
            else//不能穿透
            {
                RaycastHit hit;
                if (Physics.SphereCast(startSendPos.position, saveSkill.sizeScale, transform.forward, out hit, saveSkill.flySpeed * saveSkill.flyTime))
                {
                    if (hit.collider.CompareTag(CharacterType.Enemy.ToString()))
                    {
                        enemy = hit.collider.gameObject.GetComponent<Enemy>();
                        saveSkill.UseSkill(hit.collider.gameObject, enemy, Vector3.zero);
                        if (enemy.attributeType == ElementAttributeType.Soil)//免疫状态
                        {
                            return;
                        }
                        float repel = -(saveSkill.flySpeed / 150f) * PlayerController.player.skillLv[1];
                        enemy.gameObject.transform.position -= transform.forward.normalized * repel;
                    }
                    else if (hit.collider.CompareTag(CharacterType.Arrow.ToString()))
                    {
                        if (saveSkill.canAttackArrow)
                        {
                            saveSkill.UseSkill(hit.collider.gameObject, enemy, transform.forward);
                            Destroy(hit.collider.gameObject);
                        }
                    }
                }
            }

        }
        else
        {
            saveSkill = null;
        }

    }
    public void DestroyEffect()
    {
        if (beamStart != null)
        {
            Destroy(beamStart);
            Destroy(beamEnd);
            Destroy(beam);
        }
    }
    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        if (line == null)
        {
            beamStart = Instantiate(beamStartPrefab[beamIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beamEnd = Instantiate(beamEndPrefab[beamIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beam = Instantiate(beamLineRendererPrefab[beamIndex], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            line = beam.GetComponent<LineRenderer>();
        }
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        if (saveSkill.canThrough)//可以穿透的话表现力改表
        {
            end = startSendPos.position + transform.forward.normalized * saveSkill.flySpeed * saveSkill.flyTime;
            beamEnd.transform.position = end;
            line.SetPosition(1, end);
        }
        else
        {
            RaycastHit hit;
            if (Physics.SphereCast(startSendPos.position, saveSkill.sizeScale, transform.forward, out hit, saveSkill.flySpeed * saveSkill.flyTime))
            {
                if (hit.collider.CompareTag(CharacterType.Enemy.ToString()) || hit.collider.CompareTag(CharacterType.Wall.ToString()) || hit.collider.CompareTag(CharacterType.AttackWall.ToString()))
                {
                    //end = hit.collider.transform.position;
                    //beamEnd.transform.position = hit.collider.transform.position - 0.5f * (dir.normalized * beamEndOffset);
                    //line.SetPosition(1, end - 0.5f * (dir.normalized * beamEndOffset));

                    end = hit.point - 0.3f * (dir.normalized * beamEndOffset);
                    beamEnd.transform.position = end;
                    line.SetPosition(1, end);
                }
                //if (hit.collider.CompareTag(CharacterType.Drop.ToString()))
                //{
                //    end = transform.position + offsetY + dir;
                //}else if (hit.collider.CompareTag(CharacterType.Terrain.ToString()))
                //{
                //    end = transform.position + offsetY + dir;
                //}
                else
                {
                    //Debug.Log("else1");
                    end = startSendPos.position  + transform.forward.normalized * saveSkill.flySpeed * saveSkill.flyTime; ;
                    beamEnd.transform.position = end;
                    line.SetPosition(1, end);
                }
            }
            else
            {
                //Debug.Log(joyPositionX);
                end = startSendPos.position  + transform.forward.normalized * saveSkill.flySpeed * saveSkill.flyTime;
                beamEnd.transform.position = end;
                line.SetPosition(1, end);
            }
        }

        //if (hit.collider != null)
        //{
        //    beamEnd.transform.position = hit.collider.transform.position;
        //    line.SetPosition(1, hit.collider.transform.position);
        //}
        //else {
        //    beamEnd.transform.position = end;
        //    line.SetPosition(1, end);
        //}
        //line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);
        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }


    private Skill saveSkill;
    public void SendBeam(Skill s)
    {
        saveSkill = s;
        if (saveSkill != null)
        {
            if (saveSkill.skillId[0] == 13)
            {
                beamIndex = 1;
            }
            else
            {
                beamIndex = 0;
            }
        }
    }
}
