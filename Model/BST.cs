namespace CompanyHierarchy.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int ManagerId { get; set; }
    }

    public class BSTNode
    {
        public Employee Data { get; set; }
        public BSTNode? Left { get; set; }
        public BSTNode? Right { get; set; }

        public BSTNode(Employee data)
        {
            Data = data;
        }
    }

    public class BST
    {
        public BSTNode? Root { get; set; }

        public void Insert(Employee data)
        {
            Root = InsertRec(Root, data);
        }

        private BSTNode InsertRec(BSTNode? root, Employee data)
        {
            if (root == null)
                return new BSTNode(data);

            if (data.Id < root.Data.Id)
                root.Left = InsertRec(root.Left, data);
            else if (data.Id > root.Data.Id)
                root.Right = InsertRec(root.Right, data);

            return root;
        }

        // ? DELETE Operation
        public void Delete(int id)
        {
            Root = DeleteRec(Root, id);
        }

        private BSTNode? DeleteRec(BSTNode? root, int id)
        {
            if (root == null) return root;

            if (id < root.Data.Id)
                root.Left = DeleteRec(root.Left, id);
            else if (id > root.Data.Id)
                root.Right = DeleteRec(root.Right, id);
            else
            {
                // Node found

                // Case 1: No child
                if (root.Left == null && root.Right == null)
                    return null;

                // Case 2: One child
                if (root.Left == null)
                    return root.Right;
                if (root.Right == null)
                    return root.Left;

                // Case 3: Two children
                BSTNode successor = FindMin(root.Right);
                root.Data = successor.Data;
                root.Right = DeleteRec(root.Right, successor.Data.Id);
            }
            return root;
        }

        private BSTNode FindMin(BSTNode node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        // Traversals
        public void InOrder(BSTNode? root)
        {
            if (root == null) return;
            InOrder(root.Left);
            Console.Write($"{root.Data.Name} ");
            InOrder(root.Right);
        }

        public void PreOrder(BSTNode? root)
        {
            if (root == null) return;
            Console.Write($"{root.Data.Name} ");
            PreOrder(root.Left);
            PreOrder(root.Right);
        }

        public void PostOrder(BSTNode? root)
        {
            if (root == null) return;
            PostOrder(root.Left);
            PostOrder(root.Right);
            Console.Write($"{root.Data.Name} ");
        }
    }
}
