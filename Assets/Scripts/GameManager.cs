using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TerrainManager terrainManager;
    public GameObject player;
    private bool playerInstantiated = false;

    void Start(){
        this.terrainManager.playerTransform = this.player.transform;

        // create the sun
        GameObject sun = new GameObject("Sun");
        Light sunLight = sun.AddComponent<Light>();
        sunLight.type = LightType.Directional;
        sunLight.intensity = 0.3f;
        sunLight.color = Color.white;
        sun.transform.position = new Vector3(0, 100, 0);
        sun.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    void Update(){
        if(!playerInstantiated){
            // check if the chunk that the player is standing in has been generated
            Vector3Int playerChunkCoords = this.terrainManager.getPlayerChunkCoords();
            playerChunkCoords[1] -= 1;
            if(this.terrainManager.generatedChunks.ContainsKey(playerChunkCoords) && this.terrainManager.generatedChunks[playerChunkCoords].isGenerated){
                // find the ground at the x and z values that the player is in by casting a ray
                RaycastHit hit;
                if (Physics.Raycast(this.player.transform.position, Vector3.down, out hit, Mathf.Infinity)){
                    // set the player's y value to the y value of the ground plus a bit of padding
                    this.player.transform.position = new Vector3(this.player.transform.position.x, hit.point.y + 0.5f, this.player.transform.position.z);
                }
                this.player = Instantiate(this.player);
                this.playerInstantiated = true;
                this.terrainManager.playerTransform = this.player.transform;
            }
        }
    }
}
