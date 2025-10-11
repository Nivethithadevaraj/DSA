namespace CompanyHierarchy.Model
{
    public class AVLNode
    {
        public Employee Data { get; set; }
        public AVLNode? Left { get; set; }
        public AVLNode? Right { get; set; }
        public int Height { get; set; } = 1;

        public AVLNode(Employee data)
        {
            Data = data;
        }
    }

    public class AVL
    {
        public AVLNode? Root { get; set; }

        private int HeightOf(AVLNode? n) => n?.Height ?? 0;
        private int GetBalance(AVLNode? n) => n == null ? 0 : HeightOf(n.Left) - HeightOf(n.Right);

        private AVLNode RotateRight(AVLNode y)
        {
            Console.WriteLine($"?? Right Rotation on {y.Data.Name}");
            var x = y.Left!;
            var T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(HeightOf(y.Left), HeightOf(y.Right)) + 1;
            x.Height = Math.Max(HeightOf(x.Left), HeightOf(x.Right)) + 1;

            return x;
        }

        private AVLNode RotateLeft(AVLNode x)
        {
            Console.WriteLine($"?? Left Rotation on {x.Data.Name}");
            var y = x.Right!;
            var T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(HeightOf(x.Left), HeightOf(x.Right)) + 1;
            y.Height = Math.Max(HeightOf(y.Left), HeightOf(y.Right)) + 1;

            return y;
        }

        public void Insert(Employee emp)
        {
            Root = InsertRec(Root, emp);
        }

        private AVLNode InsertRec(AVLNode? node, Employee emp)
        {
            if (node == null)
                return new AVLNode(emp);

            if (emp.Id < node.Data.Id)
                node.Left = InsertRec(node.Left, emp);
            else if (emp.Id > node.Data.Id)
                node.Right = InsertRec(node.Right, emp);
            else
                return node; // duplicate not allowed

            node.Height = 1 + Math.Max(HeightOf(node.Left), HeightOf(node.Right));

            int balance = GetBalance(node);

            // LL
            if (balance > 1 && emp.Id < node.Left!.Data.Id)
            {
                Console.WriteLine($"?? LL Imbalance at {node.Data.Name}");
                return RotateRight(node);
            }

            // RR
            if (balance < -1 && emp.Id > node.Right!.Data.Id)
            {
                Console.WriteLine($"?? RR Imbalance at {node.Data.Name}");
                return RotateLeft(node);
            }

            // LR
            if (balance > 1 && emp.Id > node.Left!.Data.Id)
            {
                Console.WriteLine($"?? LR Imbalance at {node.Data.Name}");
                node.Left = RotateLeft(node.Left!);
                return RotateRight(node);
            }

            // RL
            if (balance < -1 && emp.Id < node.Right!.Data.Id)
            {
                Console.WriteLine($"?? RL Imbalance at {node.Data.Name}");
                node.Right = RotateRight(node.Right!);
                return RotateLeft(node);
            }

            return node;
        }

        public void InOrder(AVLNode? node)
        {
            if (node == null) return;
            InOrder(node.Left);
            Console.Write($"{node.Data.Name} ");
            InOrder(node.Right);
        }

        public void PreOrder(AVLNode? node)
        {
            if (node == null) return;
            Console.Write($"{node.Data.Name} ");
            PreOrder(node.Left);
            PreOrder(node.Right);
        }

        public void PostOrder(AVLNode? node)
        {
            if (node == null) return;
            PostOrder(node.Left);
            PostOrder(node.Right);
            Console.Write($"{node.Data.Name} ");
        }
    }
}
