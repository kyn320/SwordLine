﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;
namespace Astar2DPathFinding.Mika
{

    public class CountPath : MonoBehaviour, Pathfinding
    {
        private Transform startPos;
        private Vector2 endPos;
        private Vector2[] pathArray;


        private IEnumerator currentPath;
        private Vector2 endPosition;
        private bool readyToCountPath = true;

        [HideInInspector]
        public float moveSpeed;

        //Interval time between pathfinding
        [SerializeField]
        private bool usePathSmooting;
        [SerializeField]
        private bool showPathSmoothing;

        public int GetPathLenght()
        {
            if (pathArray == null)
                return -1;
            else
                return pathArray.Length;
        }

        public void FindPath(Transform _seeker, Vector2 _endPos)
        {
            if (!readyToCountPath)
            {
                return;
            }

            else if (_seeker == null)
            {
                UnityEngine.Debug.LogError("Missing seeker!", this);
                return;
            }

            startPos = _seeker;
            endPos = _endPos;
            //Basic raycast if can move directly to end target

            //bool cantSeeTarget = Physics2D.Linecast(_seeker.transform.position, _endPos, grid.unwalkableMask);
            //if (cantSeeTarget == false)
            //{
            //    Vector2[] newPath = new Vector2[1];
            //    newPath[0] = _endPos;
            //    OnPathFound(newPath);
            //    sw.Stop();
            //    print("Time took to find path: " + sw.ElapsedMilliseconds);
            //    StartCoroutine(PathCountDelay());
            //    return;
            //}

            if (_endPos != endPosition)
            {
                endPosition = _endPos;
                ThreadController.SearchPathRequest(this, _seeker.position, endPosition);

            }
        }

        public void RestartMovement()
        {
            if (currentPath != null)
            {
                StopCoroutine(currentPath);
            }
            currentPath = movepath(pathArray);
            StartCoroutine(currentPath);
        }

        //This has not been tested
        public void StopMovement()
        {
            if (currentPath != null)
            {
                StopCoroutine(currentPath);
            }

        }

        public void OnPathFound(Vector2[] newPath)
        {
            if (currentPath != null)
            {
                StopCoroutine(currentPath);

            }
            currentPath = movepath(newPath);
            pathArray = newPath;
            StartCoroutine(currentPath);

        }

        public IEnumerator movepath(Vector2[] pathArray)
        {
            if (pathArray == null)
            {
                yield break;
            }
            for (int i = 0; i < pathArray.Length; i++)
            {
                while (((Vector2)startPos.transform.position - pathArray[i]).sqrMagnitude > 0.1f)
                {

                    ////if (Physics2D.Linecast(startPos.transform.position, endPosition, Grid.instance.unwalkableMask) == false)
                    ////{
                    ////    UnityEngine.Debug.DrawLine(startPos.transform.position, endPosition, Color.black, 10);
                    ////    break;
                    ////}

                    if (usePathSmooting && i < pathArray.Length - 1)
                    {
                        bool cantSeeTarget = Physics2D.Linecast(startPos.transform.position, pathArray[i + 1], PathfindingGrid.Instance.unwalkableMask);
                        if (cantSeeTarget == false)
                        {
                            if (showPathSmoothing)
                            {
                                UnityEngine.Debug.DrawLine(startPos.transform.position, pathArray[i + 1], Color.white, 10);
                            }
                            i++;


                        }
                    }
                    else
                    {
                        bool cantSeeTarget = Physics2D.Linecast(startPos.transform.position, endPos, PathfindingGrid.Instance.unwalkableMask);
                        if (cantSeeTarget == false)
                        {
                            if (showPathSmoothing)
                            {
                                UnityEngine.Debug.DrawLine(startPos.transform.position, endPos, Color.white, 10);

                            }
                            break;
                        }

                    }
                    Vector2 target_pos = pathArray[i];
                    Vector2 my_pos = transform.position;
                    target_pos.x = target_pos.x - my_pos.x;
                    target_pos.y = target_pos.y - my_pos.y;
                    //float angle = Mathf.Atan2(target_pos.y, target_pos.x) * Mathf.Rad2Deg;
                    //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                    startPos.transform.position = Vector2.MoveTowards(startPos.transform.position, pathArray[i], Time.deltaTime * moveSpeed);
                    //Vector2 direction = (pathArray[i] - startPos.transform.position).normalized * 100;
                    //startPos.GetComponent<Rigidbody2D>().velocity = direction * Time.deltaTime * movespeed;

                    yield return null;
                }
            }
            while (true)
            {
                Vector2 target_pos = endPos;
                Vector2 my_pos = transform.position;
                target_pos.x = target_pos.x - my_pos.x;
                target_pos.y = target_pos.y - my_pos.y;
                //float angle = Mathf.Atan2(target_pos.y, target_pos.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                startPos.transform.position = Vector2.MoveTowards(startPos.transform.position, endPos, Time.deltaTime * moveSpeed);
                //Vector2 direction = (endPosition - startPos.transform.position).normalized * 100; ;
                //startPos.GetComponent<Rigidbody2D>().velocity = direction * Time.deltaTime * movespeed;
                yield return null;
            }
        }

        //Draw path to gizmoz
        public void OnDrawGizmos()
        {
            if (pathArray != null)
            {
                for (int i = 0; i < pathArray.Length - 1; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(pathArray[i], Vector2.one);
                    Gizmos.DrawLine(pathArray[i], pathArray[i + 1]);
                }
            }
        }



    }
}