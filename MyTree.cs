using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataTest
{

    public class BinaryTree<T>
    {

        public int NodesNum { get; private set; }
        public Node<T> Root { get; set; } = null!;
        public List<Node<T>[]> Levels { get; set; } = null!;
        public int TreeLevel { get; private set; } = -1;

        public void Add(T value)
        {
            if (TreeLevel == -1)
            {
                TreeLevel++;
                NodesNum++;
                Root = new Node<T>(value, TreeLevel, 0);
                Levels = new List<Node<T>[]>();
                Levels.Add(new Node<T>[] { Root });
            }
            else
            {
                AddNode(value);
            }
        }
        private void AddNode(T value)
        {
            int lastArray = Levels.Count - 1;
            int LastIndexValue = Levels[lastArray].Length - 1;
            //int LastIndexValue = (Int16)Math.Pow(2, TreeLevel) - 1;

            if (Levels[lastArray][LastIndexValue] == null)
            {
                for (int i = 0; i <= LastIndexValue + 1; i++)
                {
                    if (Levels[lastArray][i] == null)
                    {
                        NodesNum++;
                        Levels[lastArray][i] = new Node<T>(value, TreeLevel, i);
                        if (i % 2 == 0)
                        {
                            Levels[TreeLevel - 1][i / 2].Left = Levels[lastArray][i];
                        }
                        else
                        {
                            Levels[TreeLevel - 1][i / 2].Right = Levels[lastArray][i];
                        }
                        return;
                    }

                }
            }
            else
            {
                TreeLevel++;
                int newArrSize = (Int16)Math.Pow(2, TreeLevel);
                Node<T>[] newArr = new Node<T>[newArrSize];
                Levels.Add(newArr);
                lastArray = Levels.Count - 1;
                NodesNum++;
                Levels[lastArray][0] = new Node<T>(value, TreeLevel, 0);
                Levels[lastArray - 1][0].Left = Levels[lastArray][0];
                return;

            }
        }
        public void PrintTree()
        {
            if (TreeLevel == -1)
            {
                Console.WriteLine("Tree is Empty");
                return;
            }
            Print(Root);
        }
        private void Print(Node<T> node, string space = " ")
        {
            Console.WriteLine(space + node.Value);

            if (node.Left != null)
                Print(node.Left, space + "  ");
            if (node.Right != null)
                Print(node.Right, space + "  ");
        }
        public void PrintLevels()
        {
            if (TreeLevel == -1)
            {
                Console.WriteLine("Tree is Empty");
                return;
            }
            int level = 1;
            foreach (var Lvl in Levels)
            {
                Console.WriteLine("Level:" + level++);
                foreach (var Node in Lvl)
                {
                    if (Node == null)
                        return;
                    Console.WriteLine("  " + Node.Value);
                }
                Console.WriteLine("==============");
            }
        }
    }

    public class Node<T>
    {
        //public int LeftIndex { get; set; }
        //public int RightIndex { get; set; }
        public T Value { get; private set; }
        public int IndexOfNode { get; private set; }
        public int NodeinArr { get; private set; }
        public int ParentIndex { get; private set; }
        public Node<T> Left { get; set; } = null!;
        public Node<T> Right { get; set; } = null!;
        public int NodeLevel { get; private set; }



        public Node(T value, int Level, int inArr)
        {
            //LeftIndex = 2 * IndexOfNode + 1;
            //RightIndex = 2 * IndexOfNode + 2;
            NodeLevel = Level;
            Value = value;
            NodeinArr = inArr;

            IndexOfNode = NodeinArr + (int)Math.Pow(2, NodeLevel) - 1;
            ParentIndex = IndexOfNode % 2 == 0 ? IndexOfNode / 2 - 1 : IndexOfNode / 2;


        }
    }
}
