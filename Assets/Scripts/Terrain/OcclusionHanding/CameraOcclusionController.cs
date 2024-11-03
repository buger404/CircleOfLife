using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CircleOfLife
{
    public enum CameraFollower
    {
        Player
    }
    public class CameraOcclusionController : MonoBehaviour
    {
        private Transform player;
        private SpriteRenderer playerSpriteRenderer;
        private RaycastHit hitInfo;
        private ConcurrentDictionary<GameObject, bool> occlusionsDict;
        private List<GameObject> lastOcclusions;
        private float colorAlpha = 0.3f;

        void Start()
        {
            player = GameObject.Find(CameraFollower.Player.ToString()).transform;
            playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
            occlusionsDict = new ConcurrentDictionary<GameObject, bool>();

        }

        void FixedUpdate()
        {
            if (playerSpriteRenderer == null)
            {
                return;
            }
            CheckRayCastCollider(player.position);
            RegisterOcclusion();
            UpdateOcclusions();
        }
        //向跟随的目标发射射线，获取射线触碰的所有碰撞体


        //将所有在currentOcclusions的碰撞体Alpha设至为0.5
        public void CheckRayCastCollider(Vector3 originPosOnWorld)
        {
            lastOcclusions = new List<GameObject>();
            Vector3 playerPos = Camera.main.WorldToScreenPoint(originPosOnWorld);
            Vector2 cameraPos = Camera.main.ScreenToWorldPoint(new Vector3(playerPos.x, playerPos.y, 0));
            //Debug.Log(playerPos + " " + cameraPos);
            //修正cameraPos的坐标
            //Vector2 playerPos = new Vector2(player.position.x, player.position.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(cameraPos, -Vector2.zero);
            foreach (RaycastHit2D hit in hits)
            {
                //Debug.Log(hit.collider.name);
                lastOcclusions.Add(hit.collider.gameObject);
            }
        }
        //注册除了玩家以外所有碰撞体的SpriteRenderer
        public void RegisterOcclusion()
        {
            foreach (GameObject occlusion in lastOcclusions)
            {
                if (occlusion == null || occlusion.tag == "Player")
                {
                    continue;
                }
                if (occlusionsDict.ContainsKey(occlusion))
                {
                    occlusionsDict[occlusion] = true;
                }
                else
                {
                    occlusionsDict.TryAdd(occlusion, true);
                }
            }
        }
        //更新currentOcclusions，将不在lastOcculusions的碰撞体Alpha设至为1后,bool设为false
        private void UpdateOcclusions()
        {
            foreach (KeyValuePair<GameObject, bool> occlusion in occlusionsDict)
            {
                if (lastOcclusions.Contains(occlusion.Key) == false)
                {
                    occlusionsDict[occlusion.Key] = false;
                    SetAlphaToOcclusion(occlusion.Key, 1f);
                }
                else if (occlusion.Value == false)
                {
                    SetAlphaToOcclusion(occlusion.Key, 1f);
                }
                else
                {
                    SetAlphaToOcclusion(occlusion.Key, colorAlpha);
                }
            }
        }
        private void SetAlphaToOcclusion(GameObject gameObject, float a)
        {
            SpriteRenderer spriteRenderer;
            Tilemap tilemap;
            if (gameObject.TryGetComponent(out spriteRenderer))
            {
                if (a < 0.9f && spriteRenderer.sortingOrder < playerSpriteRenderer.sortingOrder)
                {
                    return;
                }
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, a);

            }
            else if (gameObject.TryGetComponent(out tilemap))
            {
                TilemapRenderer tilemapRenderer = tilemap.GetComponent<TilemapRenderer>();
                if (a < 0.9 && tilemapRenderer.sortingOrder < playerSpriteRenderer.sortingOrder)
                {
                    return;
                }
                tilemap.color = new Color(tilemap.color.r, tilemap.color.b, tilemap.color.g, a);
            }
        }
    }
}
