/********************************************************
 * 														*
 * ScriptName:   DestroyableObject.cs					*
 * 														*
 * Copyright(c): Victor Klepikov						*
 * Support: 	 ialucard4@gmail.com					*
 * 														*
 * MyAssets:     http://goo.gl/8ncIsT					*
 * MyTwitter:	 http://twitter.com/VictorKlepikov		*
 * MyFacebook:	 http://www.facebook.com/vikle4 		*
 * 														*
 ********************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyableObject : MonoBehaviour
{

    #region DestroyableObject Parameter Vars

#if UNITY_EDITOR
    public bool DV_Explode_Debug_Mode = false;
#endif

    public DestroyableModes CurrentDestroyableMode;

    public float DetonateTimeAndZone = 1.5f;

    public float ExplodeDamageZone = 3f;
    public float FragmentsMass = 1f;
    public float FragmentsSpeed = 100f;

    private float ReadyTime = 0f;

    public GameObject DecalGO = null;
    private GameObject DecalClone = null;

    public Texture2D texExlpode = null;
    public AudioClip sndExlpode = null;
    public ParticleSystem prtExlpode = null;
    private ParticleSystem prtExlpodeClone = null;

    private float ToTargetDistance = 0f;
    private float PersentFragmentsDamage = 0f;
    private float PersentFragmentsMass = 0f;
    private float PersentFragmentsSpeed = 0f;

    private RaycastHit MyHit;
    private Ray MyRay;
    private Vector3 TargetPosition;
    private Vector3 TargetDirection;

    private List<GameObject> AllTargets = new List<GameObject>();
    private List<HealthShooter> AllTargetsHealth = new List<HealthShooter>();

    private List<GameObject> AllPhysicsObjects = new List<GameObject>();

    private Transform MyTransform = null;
    private GameObject Player = null;
  //  private PlayerCamera ThisPlayerCamera;

    private Collider TargetCollider = null;

    private Vector3 CalculatedPosition;
    private Quaternion CalculatedRotation;
    private bool Recording = true;


    #endregion

    #region Start	
    void Start()
    {
        MyTransform = transform;

  //      if (CurrentDestroyableMode != DestroyableModes.DestroyableObject)
  //          ThisPlayerCamera = GameObject.FindGameObjectWithTag(TagsManager.MainCamera).GetComponent<PlayerCamera>();

        Player = GameObject.FindGameObjectWithTag(TagsManager.Player);
        AllTargets.Add(Player);
        AllTargets.AddRange(GameObject.FindGameObjectsWithTag(TagsManager.Enemy));
        AllTargets.AddRange(GameObject.FindGameObjectsWithTag(TagsManager.DestroyableObject));

        AllPhysicsObjects.AddRange(GameObject.FindGameObjectsWithTag(TagsManager.PhysicsObject));

#if UNITY_EDITOR
        ErrorChecker();
#endif

   //     GameObject LEVCache = GameObject.FindGameObjectWithTag(TagsManager.LEVELCache);

        if (CurrentDestroyableMode != DestroyableModes.Grenade_v2)
        {
            DecalClone = (GameObject)Instantiate(DecalGO, MyTransform.position, MyTransform.rotation) as GameObject;
            DecalClone.transform.parent = GameObject.FindGameObjectWithTag(TagsManager.LEVELCache).transform;
            DecalClone.SetActive(false);
        }
   
        prtExlpodeClone = (ParticleSystem)Instantiate(prtExlpode, transform.position, transform.rotation) as ParticleSystem;
    //    prtExlpodeClone.transform.parent = LEVCache.transform;

        for (int cnt = 0; cnt < AllTargets.Count; cnt++)
        {
            if (AllTargets[cnt] != null) AllTargetsHealth.Add(AllTargets[cnt].GetComponent<HealthShooter>());
            else AllTargets.RemoveAt(cnt);
        }

        //**ForExplosiveObjects
        MyRay = new Ray(MyTransform.position, Vector3.down);
        Physics.Raycast(MyRay, out MyHit);
        SpawnCalculation();
    }
    #endregion

    #region Update
    void Update()
    {
        switch (CurrentDestroyableMode)
        {
            case DestroyableModes.Mine: MineActivity(); break;

            case DestroyableModes.Grenade_v1:
                MyRay = new Ray(MyTransform.position, Vector3.down);
                Physics.Raycast(MyRay, out MyHit);
                SpawnCalculation();
                break;

            case DestroyableModes.DestroyableObject:
            case DestroyableModes.Grenade_v2:
                CalculatedPosition = MyTransform.position;
                CalculatedRotation = MyTransform.rotation;
                break;

            case DestroyableModes.Rocket:

                MyTransform.Translate(Vector3.forward * Time.deltaTime * 25f);
                MyRay = new Ray(MyTransform.position, MyTransform.forward);
                Physics.Raycast(MyRay, out MyHit);
                ToTargetDistance = Vector3.Distance(MyTransform.position, MyHit.point);

                if (ToTargetDistance > 1.45f) Recording = true;
                else
                {
                    Recording = false;
                    Explode();
                }
                if (Recording) SpawnCalculation();

                break;

            default: break;
        }

        if (CurrentDestroyableMode == DestroyableModes.Grenade_v1
            || CurrentDestroyableMode == DestroyableModes.Grenade_v2
            || CurrentDestroyableMode == DestroyableModes.Rocket)
            GetLifeTime();

#if UNITY_EDITOR
        if (DV_Explode_Debug_Mode) ExplodeDEBBUGING();
#endif
    }
    #endregion

    #region OnCollisionEnter
    void OnCollisionEnter(Collision collision)
    {
        if (CurrentDestroyableMode == DestroyableModes.Grenade_v2)
        {
            TargetCollider = collision.collider;
            Explode();
        }
    }
    #endregion

    #region GetLifeTime
    private void GetLifeTime()
    {
        ReadyTime += Time.deltaTime;
        if (ReadyTime > DetonateTimeAndZone)
        {
            ReadyTime = 0f;
            Explode();
        }
    }
    #endregion

    #region SpawnCalculation
    private void SpawnCalculation()
    {
        CalculatedPosition = MyHit.point + (MyTransform.position - MyHit.point) * 0.05f;
        CalculatedRotation = Quaternion.FromToRotation(Vector3.forward, -MyHit.normal);
    }
    #endregion

    #region MineActivity
    private void MineActivity()
    {
        for (int cnt = 0; cnt < AllTargets.Count; cnt++)
        {
            if (AllTargets[cnt] == null)
            {
                AllTargets.RemoveAt(cnt);
                AllTargetsHealth.RemoveAt(cnt);
            }

            if (AllTargets[cnt] != null && AllTargets[cnt].tag != TagsManager.DestroyableObject)
            {
                TargetPosition = AllTargets[cnt].transform.position;
                TargetDirection = TargetPosition - MyTransform.position;

                ToTargetDistance = Vector3.Distance(MyTransform.position, TargetPosition);
                if (ToTargetDistance < DetonateTimeAndZone) Explode();
            }
        }
    }
    #endregion

    #region DestroyRun
    public void DestroyRun()
    {
        switch (CurrentDestroyableMode)
        {
            case DestroyableModes.DestroyableObject:
                EffectsActivation();
                break;

            case DestroyableModes.ExplosiveObject:
            case DestroyableModes.Mine:
                StartCoroutine(ExplodeRun());
                break;
        }
    }
    #endregion

    #region ExplodeRun
    private IEnumerator ExplodeRun()
    {
        yield return new WaitForSeconds(0.13f);
        Explode();
    }
    #endregion

    #region Explode
    private void Explode()
    {
        TargetCollider = MyHit.collider;

        for (int cnt = 0; cnt < AllTargets.Count; cnt++)
        {
            if (AllTargets[cnt] == null)
            {
                AllTargets.RemoveAt(cnt);
                AllTargetsHealth.RemoveAt(cnt);
            }

            if (AllTargets[cnt] != null && AllTargets[cnt].activeSelf)
            {
                TargetPosition = AllTargets[cnt].transform.position;
                TargetDirection = TargetPosition - MyTransform.position;

                ToTargetDistance = Vector3.Distance(MyTransform.position, TargetPosition);

                MyRay = new Ray(MyTransform.position, TargetDirection.normalized);

                if (Physics.Raycast(MyRay, out MyHit))
                {
                    if (ToTargetDistance < ExplodeDamageZone)
                    {
                        PersentFragmentsDamage = 100f - (ToTargetDistance / ExplodeDamageZone * 100f);
                        PersentFragmentsMass = FragmentsMass * PersentFragmentsDamage / 100f;
                        PersentFragmentsSpeed = FragmentsSpeed * PersentFragmentsDamage / 100f;
                        
                            if ( MyHit.collider.tag == TagsManager.Enemy || MyHit.collider.tag == TagsManager.Player || MyHit.collider.tag == TagsManager.DestroyableObject )
                            {							
                                AllTargetsHealth[ cnt ].DamageHealth( 20 );	// get hit points instead of magic number										
                            }
                            
                    }
                }
            }
        }

        for (int cnt = 0; cnt < AllPhysicsObjects.Count; cnt++)
        {
            if (AllPhysicsObjects[cnt] == null)
            {
                AllPhysicsObjects.RemoveAt(cnt);
            }

            if (AllPhysicsObjects[cnt] != null && AllPhysicsObjects[cnt].activeSelf)
            {
                TargetPosition = AllPhysicsObjects[cnt].transform.position;
                TargetDirection = TargetPosition - MyTransform.position;

                ToTargetDistance = Vector3.Distance(MyTransform.position, TargetPosition);

                if (ToTargetDistance < ExplodeDamageZone)
                {
                    PersentFragmentsDamage = 100f - (ToTargetDistance / ExplodeDamageZone * 100f);
                    PersentFragmentsMass = FragmentsMass * PersentFragmentsDamage / 100f;
                    PersentFragmentsSpeed = FragmentsSpeed * PersentFragmentsDamage / 100f;

                    if (AllPhysicsObjects[cnt].GetComponent<Rigidbody>())
                        AllPhysicsObjects[cnt].GetComponent<Rigidbody>().AddForce(TargetDirection.normalized * ((PersentFragmentsMass * PersentFragmentsSpeed) / AllPhysicsObjects[cnt].GetComponent<Rigidbody>().mass));
                }
            }
        }

        EffectsActivation();
    }
    #endregion

    #region EffectsActivation
    private void EffectsActivation()
    {
        if (CurrentDestroyableMode != DestroyableModes.Grenade_v2)
            if (TargetCollider != null)
                if (TargetCollider.tag != TagsManager.Enemy
                            && TargetCollider.tag != TagsManager.Player
                            && TargetCollider.tag != TagsManager.DestroyableObject
                            && TargetCollider.tag != TagsManager.PhysicsObject)
                {
                    DecalClone.transform.position = CalculatedPosition;
                    DecalClone.transform.rotation = CalculatedRotation;
                    DecalClone.GetComponent<Renderer>().material.mainTexture = texExlpode;
                    DecalClone.SetActive(true);
                }

        if (CurrentDestroyableMode == DestroyableModes.DestroyableObject)
        {
            DecalClone.transform.position = CalculatedPosition;
            DecalClone.transform.rotation = CalculatedRotation;
            DecalClone.SetActive(true);
        }

        prtExlpodeClone.transform.position = CalculatedPosition;
        prtExlpodeClone.transform.rotation = CalculatedRotation;

        prtExlpodeClone.Play();
        AudioSource.PlayClipAtPoint(sndExlpode, MyTransform.position);

   //     if (CurrentDestroyableMode != DestroyableModes.DestroyableObject)
   //         ThisPlayerCamera.CameraShake(Vector3.Distance(MyTransform.position, Player.transform.position), ExplodeDamageZone);

        Recording = true;
        ToTargetDistance = 0f;

        gameObject.SetActive(false);
    }
    #endregion


#if UNITY_EDITOR

    #region ExplodeDEBBUGING
    private void ExplodeDEBBUGING()
    {
        for (int cnt = 0; cnt < AllTargets.Count; cnt++)
        {
            if (AllTargets[cnt] == null)
            {
                AllTargets.RemoveAt(cnt);
                AllTargetsHealth.RemoveAt(cnt);
            }

            if (AllTargets[cnt] != null && AllTargets[cnt].activeSelf)
            {
                Vector3 TPosition = AllTargets[cnt].transform.position;
                Vector3 TDirection = TPosition - MyTransform.position;

                float TDistance = Vector3.Distance(MyTransform.position, AllTargets[cnt].transform.position);

                Ray MRay = new Ray(MyTransform.position, TDirection.normalized);
                RaycastHit MHit;

                if (Physics.Raycast(MRay, out MHit))
                {
                    if (TDistance < ExplodeDamageZone)
                    {
                        if (MHit.collider.tag == TagsManager.Enemy || MHit.collider.tag == TagsManager.Player || MHit.collider.tag == TagsManager.DestroyableObject)
                        {
                            Debug.DrawLine(TPosition, MyTransform.position, Color.red);
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region ErrorChecker
    private void ErrorChecker()
    {
        /*
		if ( !DecalGO )
		{
			if ( CurrentDestroyableMode != DestroyableModes.DestroyableObject ) Debug.LogError( "ERROR: GO Fragments is not assigned! ERROR in: " + transform.name );
			else Debug.LogError( "ERROR: GO Decal is not assigned! ERROR in: " + transform.name );
		}
		
		if ( !texExlpode && CurrentDestroyableMode != DestroyableModes.DestroyableObject && CurrentDestroyableMode != DestroyableModes.Grenade_v2 ) 
			Debug.LogError( "ERROR: texExlpode is not assigned! ERROR in: " + transform.name );
		if ( !sndExlpode ) Debug.LogError( "ERROR: SFX Exlpode is not assigned! ERROR in: " + transform.name );
		if ( !prtExlpode ) Debug.LogError( "ERROR: FX Exlpode is not assigned! ERROR in: " + transform.name );

		if ( !GameObject.FindGameObjectWithTag( TagsManager.LEVELCache ) ) Debug.LogError( "ERROR: LEVELCache is not finded! ERROR in: " + transform.name );
	*/
    }
    #endregion

#endif
}
