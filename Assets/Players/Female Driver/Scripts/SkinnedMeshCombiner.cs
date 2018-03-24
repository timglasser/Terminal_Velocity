
// original script "SkinnedMeshCombiner" by Ian Smithers of Australia
// posted (2012) on wiki.unity3d.com under the creative commons license 
// modified by Ranjeet "Rungy" Singhal Dec 6,2015 (SexySideKicks.com)
// The "Content is available under Creative Commons Attribution Share Alike."
// which grants free use, even commercial, the license and requires contributors 
// names, stated above, be kept on the work  >>in this script<<

using UnityEngine;
using System.Collections.Generic;

public class SkinnedMeshCombiner : MonoBehaviour 
{
 	Material baseMat;
	SkinnedMeshRenderer newSkin;
	List<SkinnedMeshRenderer> smRenderers;
	
    void Start() 
	{        
		smRenderers = new List<SkinnedMeshRenderer>();
        List<Transform> bones = new List<Transform>();        
        List<BoneWeight> boneWeights = new List<BoneWeight>();        
        List<CombineInstance> combineInstances = new List<CombineInstance>();
		
		//get all the skinned mesh renderers
		SkinnedMeshRenderer[] allRenderers =GetComponentsInChildren<SkinnedMeshRenderer>();
		print (allRenderers.Length);
		
		// go through all the skinned mesh renderers
		// get only the ones...
		// 		whose material is the same as the first skinned mesh
		// 		and whose renderer is enabled
		foreach(SkinnedMeshRenderer smr in allRenderers)
		{
			if(smr.enabled ==true)
			{
				if(baseMat==null)
				{
					print (smr.name);
					baseMat = smr.sharedMaterial;
				}
				
				if(smr.sharedMaterial == baseMat)
				{
					print (smr.name);
					smRenderers.Add(smr);
				}
			}
		}
		
        int numSubs = 0;
        foreach(SkinnedMeshRenderer smr in smRenderers) numSubs += smr.sharedMesh.subMeshCount;
 		
        int[] meshIndex = new int[numSubs];
        for( int s = 0; s < smRenderers.Count; s++ ) 
		{
            SkinnedMeshRenderer smr = smRenderers[s];
			
			// Make a new list of bones adding new ones as we find them
            foreach( Transform bone in smr.bones )
			{
				if(!bones.Contains(bone))
				{
					bones.Add( bone );
				}
			}
			
			//bone indices are are not the same for all skinned meshes
			//the skinning data requires an index for each bone
			//since our new mesh needs to share a combined bone list
			//we need to find the IndexOf the new indices
            BoneWeight[] meshBoneweights = smr.sharedMesh.boneWeights;
            foreach( BoneWeight bw in meshBoneweights ) 
			{
                BoneWeight bWeight = bw;
				bWeight.boneIndex0 = bones.IndexOf(smr.bones[bw.boneIndex0]); 
                bWeight.boneIndex1 = bones.IndexOf(smr.bones[bw.boneIndex1]);
				bWeight.boneIndex2 = bones.IndexOf(smr.bones[bw.boneIndex2]);
				bWeight.boneIndex3 = bones.IndexOf(smr.bones[bw.boneIndex3]);
                boneWeights.Add( bWeight );
            }
 
            CombineInstance ci = new CombineInstance();
			ci.transform = smr.transform.localToWorldMatrix;
            ci.mesh = smr.sharedMesh;
            meshIndex[s] = ci.mesh.vertexCount;
            combineInstances.Add( ci );
 
            //Object.Destroy( smr.gameObject );
			smr.enabled = false;
        }
 
        List<Matrix4x4> bindposes = new List<Matrix4x4>();
         for( int b = 0; b < bones.Count; b++ ) 
		{
            bindposes.Add( bones[b].worldToLocalMatrix);
        }
		
		
		newSkin = gameObject.AddComponent<SkinnedMeshRenderer>();
        newSkin.sharedMesh = new Mesh();
        newSkin.sharedMesh.CombineMeshes( combineInstances.ToArray(), true, true );
        newSkin.bones = bones.ToArray();
		newSkin.material = baseMat;
        newSkin.sharedMesh.boneWeights = boneWeights.ToArray();
        newSkin.sharedMesh.bindposes = bindposes.ToArray();
        newSkin.sharedMesh.RecalculateBounds();
    }
}
