using System.Windows.Controls;

namespace KinectWPF.Node
{
    class Node
    {
        public enum NodeType { Click };

        public float x, y;
        public float radius;
        public NodeType nodeType;

        public Node(Image image, float x, float y, MainWindow mainWindow)
        {
            this.x = x;
            this.y = y;
        }

        public void Update()
        {

        }

        public void Destroy()
        {

        }
    }
}
