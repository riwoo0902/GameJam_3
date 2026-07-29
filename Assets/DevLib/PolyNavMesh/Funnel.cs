using System.Collections.Generic;
using UnityEngine;

namespace DevLib.PolyNavMesh
{
    public static class Funnel
    {
        public static List<Vector2> StringPull(Vector2 start, Vector2 end, List<(Vector2 left, Vector2 right)> portals)
        {
            List<Vector2> path = new List<Vector2>{ start }; //시작점 하나만 넣고 시작.

            if (portals.Count == 0)
            {
                path.Add(end);
                return path; //경로상에 포탈이 없다는건 목적지가 그냥 같은 폴리곤 안이라는거
            }
            
            var pts = new List<(Vector2 left, Vector2 right)>(portals.Count + 2); //시작과 끝점을 포함하여 
            pts.Add((start, start)); 
            pts.AddRange(portals);
            pts.Add((end, end));

            Vector2 apex = start;
            Vector2 portalLeft = start;
            Vector2 portalRight = start;
            int apexIdx = 0;
            int leftIdx = 0;
            int rightIdx = 0;

            //시작 부터 완료지점까지의 포탈들을 나아가면서 왼쪽 오른쪽으로 깔대기를 좁혀나간다.
            for (int i = 0; i < pts.Count; i++)
            {
                (Vector2 newLeft, Vector2 newRight) = pts[i];
                //오른쪽 경계를 갱신하는 작업을 수행할거고
                if (TriArea2(apex, portalRight, newRight) >= 0f)
                {
                    if (apex == portalRight || TriArea2(apex, portalLeft, newRight) < 0f)
                    {
                        portalRight = newRight;
                        rightIdx = i;
                    }
                    else //위 2가지가 모두 해당이 안되면 왼쪽 모서리에 실이 걸린거다.
                    {
                        if(portalLeft != apex && portalLeft != end)
                            path.Add(portalLeft); //포탈의 왼쪽이 새로운 경로로 들어간다.

                        apex = portalLeft;
                        apexIdx = leftIdx;
                        portalLeft = apex;
                        portalRight = apex; //꼭지점을 다시설정하고 거기서부터 뻗어 나간다.
                        leftIdx = apexIdx;
                        rightIdx = apexIdx;
                        i = apexIdx;
                        continue;
                    }
                }
                
                //왼쪽 경계를 갱신하는 작업을 수행한다.

                if (TriArea2(apex, portalLeft, newLeft) <= 0f)
                {
                    if (apex == portalLeft || TriArea2(apex, portalRight, newLeft) > 0f)
                    {
                        portalLeft = newLeft;
                        leftIdx = i;
                    }
                    else
                    {
                        //오른쪽 모서리에 실이 걸린것
                        if(portalRight != apex && portalRight != end)
                            path.Add(portalRight);

                        apex = portalRight;
                        apexIdx = rightIdx;
                        portalLeft = apex;
                        portalRight = apex;
                        leftIdx = apexIdx;
                        rightIdx = apexIdx;
                        i = apexIdx;
                        continue;
                    }
                }
            }
            
            if(path.Count == 0 || path[^1] != end)
                path.Add(end); //마지막 포인트 추가한다.
            
            return path;
        }

        /// <summary>
        /// 2D 벡터 외적을 이용해서 ba벡터와 ca 벡터간에 누가 시계방향왼쪽인지 오른쪽인지를 알아낸다.
        /// 양수 -> C가 벡터 a->b 의 왼쪽에 있다.(CCW)
        /// 음수 -> C가 벡터 a->b의 오른쪽에 있다.(CW) 
        /// </summary>
        private static float TriArea2(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ba = b - a;
            Vector2 ca = c - a;
            return ba.x * ca.y - ba.y * ca.x;
        }
    }
}