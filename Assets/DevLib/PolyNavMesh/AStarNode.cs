namespace DevLib.PolyNavMesh
{
    public class AStarNode
    {
        // F = G + H
        public readonly NavPolygon polygon;
        public float G;
        public float F;
        public AStarNode parent;
        
        public AStarNode(NavPolygon polygon) => this.polygon = polygon;
    }
}