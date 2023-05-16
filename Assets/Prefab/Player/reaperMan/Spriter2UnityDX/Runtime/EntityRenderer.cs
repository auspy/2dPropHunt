using UnityEngine;
using System;
using System.Collections.Generic;

namespace Spriter2UnityDX
{
    [DisallowMultipleComponent, ExecuteInEditMode, AddComponentMenu("")]
    public class EntityRenderer : MonoBehaviour
    {
        private SpriteRenderer[] renderers = new SpriteRenderer[0];
        private SortingOrderUpdater[] updaters = new SortingOrderUpdater[0];
        private SpriteRenderer _first;
        private string[] SkipTags = { "Player" };
        private SpriteRenderer first
        {
            get
            {
                if (_first == null && renderers.Length > 0)
                    _first = renderers[0];
                return _first;
            }
        }
        public Color Color
        {
            get { return (first != null) ? first.color : default(Color); }
            set { DoForAll(x => x.color = value); }
        }

        public Material Material
        {
            get { return (first != null) ? first.sharedMaterial : null; }
            set { DoForAll(x => x.sharedMaterial = value); }
        }

        public int SortingLayerID
        {
            get { return (first != null) ? first.sortingLayerID : 0; }
            set { DoForAll(x => x.sortingLayerID = value + 1); }
        }

        public string SortingLayerName
        {
            get { return (first != null) ? first.sortingLayerName : null; }
            set { DoForAll(x => x.sortingLayerName = value); }
        }

        [SerializeField, HideInInspector]
        private int sortingOrder = 0;
        public int SortingOrder
        {
            get { return sortingOrder; }
            set
            {
                sortingOrder = value;
                if (applySpriterZOrder)
                    for (var i = 0; i < updaters.Length; i++)
                        updaters[i].SortingOrder = value;
                else
                    DoForAll(x => x.sortingOrder = value);
            }
        }

        [SerializeField, HideInInspector]
        private bool applySpriterZOrder = false;
        public bool ApplySpriterZOrder
        {
            get { return applySpriterZOrder; }
            set
            {
                applySpriterZOrder = value;
                if (applySpriterZOrder)
                {
                    var list = new List<SortingOrderUpdater>();
                    var spriteCount = renderers.Length;
                    foreach (var renderer in renderers)
                    {
                        var updater = renderer.GetComponent<SortingOrderUpdater>();
                        if (updater == null)
                            updater = renderer.gameObject.AddComponent<SortingOrderUpdater>();
                        updater.SortingOrder = sortingOrder;
                        updater.SpriteCount = spriteCount;
                        list.Add(updater);
                    }
                    updaters = list.ToArray();
                }
                else
                {
                    for (var i = 0; i < updaters.Length; i++)
                    {
                        if (Application.isPlaying)
                            Destroy(updaters[i]);
                        else
                            DestroyImmediate(updaters[i]);
                    }
                    updaters = new SortingOrderUpdater[0];
                    DoForAll(x => x.sortingOrder = sortingOrder);
                }
            }
        }

        private void Awake()
        {
            RefreshRenders();
        }

        private void OnEnable()
        {
            DoForAll(x => x.enabled = true);
        }

        private void OnDisable()
        {
            DoForAll(x => x.enabled = false);
        }

        private void DoForAll(Action<SpriteRenderer> action)
        {
            //Debug.Log(action);
            //Debug.Log(renderers.Length);
            for (var i = 0; i < renderers.Length; i++)
            {
                action(renderers[i]);

                //Debug.Log(renderers[i].name);
            }
            ;
        }

        public void RefreshRenders()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>(true);
            updaters = GetComponentsInChildren<SortingOrderUpdater>(true);
            var length = updaters.Length;
            for (var i = 0; i < length; i++)
                updaters[i].SpriteCount = length;
            _first = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Array.IndexOf(SkipTags, other.gameObject.tag) > -1)
                return;
            // print(gameObject.name + " collided with " + other.gameObject.name);

            // get top edge of collision object
            float topY = other.bounds.max.y;

            // if is in collison and y is above the y of collision object

            // print("topY " + topY);
            // print(gameObject.transform.position.y);
            // Debug.Log("Collider type: " + other.GetType());
            // print(other is UnityEngine.CapsuleCollider2D);
            if (topY > gameObject.transform.position.y)
            {
                int otherSortingOrder = other.gameObject.GetComponent<Renderer>()
                    ? other.gameObject.GetComponent<Renderer>().sortingOrder
                    : sortingOrder;
                SortingOrder = otherSortingOrder + 1;
                // print("new SortingOrder " + SortingOrder);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (Array.IndexOf(SkipTags, other.gameObject.tag) > -1)
                return;
            // print(gameObject.name + " exited collision with " + other.gameObject.name);
            int otherSortingOrder = other.gameObject.GetComponent<Renderer>()
                ? other.gameObject.GetComponent<Renderer>().sortingOrder
                : sortingOrder;
            SortingOrder = otherSortingOrder;
            // print("new SortingOrder " + SortingOrder);
        }

        // private void OnCollisionStay2D(Collision2D other)
        // {
        //     print(gameObject.name + " is staying in collision with " + other.gameObject.name);
        // }
    }
}

// if player legs are above the graass ten rest part should also be above the grass.

// option 1
// if a player is in colliioson hten rest of the body should be a layer above
// if not in collision the body should not be a layer above
// if player bottom-y is above the obejct top-y then rest should be same later as well
