using System;
using System.CodeDom;
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

    public class BinaryTree
    {
        public int TreeLevel { get; private set; } = -1;
        public int NodesNum { get; private set; }
        public Node Root { get; private set; } = null!;
        public List<Node[]> Levels { get; set; } = null!;
        public void Add(string value)
        {
            if (!value.EndsWith("@gmail.com"))
                return;

            if (TreeLevel == -1)
            {
                TreeLevel++;
                NodesNum++;
                Root = new Node(value, TreeLevel, 0);
                Levels = new List<Node[]>();
                Levels.Add(new Node[] { Root });
            }
            else
            {
                AddNode(value);
            }
        }
        private void AddNode(string value)
        {
            int lastArray = Levels.Count - 1;
            int LastIndexValue = Levels[lastArray].Length - 1;

            if (Levels[lastArray][LastIndexValue] == null)
            {
                for (int i = 0; i <= LastIndexValue + 1; i++)
                {
                    if (Levels[lastArray][i] == null)
                    {
                        Levels[lastArray][i] = new Node(value, TreeLevel, i);
                        if (i % 2 == 0)
                        {
                            Levels[TreeLevel - 1][i / 2].Left = Levels[lastArray][i];
                        }
                        else
                        {
                            Levels[TreeLevel - 1][i / 2].Right = Levels[lastArray][i];
                        }
                        HeapSort(value, i);
                        NodesNum++;
                        return;
                    }
                }
            }
            else
            {
                TreeLevel++;

                int newLevelLength = (Int16)Math.Pow(2, TreeLevel);
                Node[] newLevel = new Node[newLevelLength];
                Levels.Add(newLevel);

                AddNode(value);
                return;
            }
        }
        private void HeapSort(string Value, int nodeIndex)
        {
            int ValueSize = GetSize(Value);
            int CheckLevel = TreeLevel;
            bool flipped = false;

            for (int i = nodeIndex; i >= 0; i--)
            {
                if (flipped)
                {
                    i++;
                    flipped = false;
                }
                if (i == 0)
                {
                    int UpperLastIndex = Levels[CheckLevel - 1].Length - 1;
                    Node UpperNode = Levels[CheckLevel - 1][UpperLastIndex];

                    if (ValueSize < UpperNode.ValueSize) // <> ASC DISC
                    {
                        Levels[CheckLevel][0].SetValue(UpperNode.Value);
                        UpperNode.SetValue(Value);

                        i = Levels[--CheckLevel].Length - 1;
                        flipped = true;
                        continue;
                    }
                    break;
                }

                else if (i > 0)
                {
                    Node BrotherNode = Levels[CheckLevel][i - 1];

                    if (ValueSize < BrotherNode.ValueSize) // <> ASC DISC
                    {
                        Levels[CheckLevel][i].SetValue(BrotherNode.Value);
                        Levels[CheckLevel][i - 1].SetValue(Value);
                        continue;
                    }
                    break;
                }
            }
        }
        protected int GetSize(string Value)
        {
            int ValueSize = 0;

            if (int.TryParse(Value, out ValueSize))
            {
                ValueSize = int.Parse(Value);
            }
            else
            {
                int i = 0;
                while (!Value[i].Equals('@'))
                {
                    ValueSize += (int)Value[i];
                    if (Value.Length == ++i)
                        break;
                }
            }
            return ValueSize;
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
        private void Print(Node node, string space = " ")
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
        public void Search(string value)
        {
            if (TreeLevel == -1)
            {
                Console.WriteLine("Tree is Empty");
                return;
            }
            int SearchSize = GetSize(value);
            int ILevel = 0;
            int LastNodeIndex;
            bool result = false;
            foreach (var Lvl in Levels)
            {
                if (ILevel == TreeLevel)
                {
                    int MaxNodes = (int)Math.Pow(2, TreeLevel + 1) - 1;
                    int NullNodes = MaxNodes - NodesNum;
                    LastNodeIndex = Lvl.Length - NullNodes - 1;
                }
                else
                {
                    LastNodeIndex = Lvl.Length - 1;
                }

                if (Lvl[0].ValueSize <= SearchSize && SearchSize <= Lvl[LastNodeIndex].ValueSize)
                {

                    result = BinarySearch(value, SearchSize, Lvl, LastNodeIndex);
                }
                ILevel++;
            }
            if (!result)
            {
                Console.WriteLine("Not Found!");
            }
        }
        private bool BinarySearch(string value, int size, Node[] nodesArr, int LastNodeIndex)
        {
            int Start = 0;
            int End = LastNodeIndex;
            int Mid = (End + Start) / 2;
            int Ptr;

            while (End >= Start)
            {
                if (size > nodesArr[Mid].ValueSize)
                {
                    Start = Mid + 1;
                    Mid = (End + Start) / 2;
                }
                if (size < nodesArr[Mid].ValueSize)
                {
                    End = Mid - 1;
                    Mid = (End + Start) / 2;
                }

                if (size == nodesArr[Mid].ValueSize)
                {
                    if (nodesArr[Mid].Value == value)
                    {
                        Console.WriteLine("Found in Level " + (nodesArr[Mid].NodeLevel + 1) + " on Index number: " + nodesArr[Mid].NodeinArr);
                        return true;
                    }

                    Ptr = Mid;

                    while (Ptr < LastNodeIndex)
                    {
                        if (size == nodesArr[++Ptr].ValueSize)
                            if (nodesArr[Ptr].Value == value)
                            {
                                Console.WriteLine("Found in Level " + (nodesArr[Mid].NodeLevel + 1) + " on Index number: " + nodesArr[Mid].NodeinArr);
                                return true;
                            }
                    }

                    Ptr = Mid;
                    while (Ptr > 0)
                    {
                        if (size == nodesArr[--Ptr].ValueSize)
                            if (nodesArr[Ptr].Value == value)
                            {
                                Console.WriteLine("Found in Level " + (nodesArr[Mid].NodeLevel + 1) + " on Index number: " + nodesArr[Mid].NodeinArr);
                                return true;
                            }
                    }
                    return false;
                }
            }
            return false;
        }
    }

    public class Node : BinaryTree
    {
        //public int LeftIndex { get; set; }
        //public int RightIndex { get; set; }

        public int ValueSize { get; set; }
        public string Value { get; set; }
        public int IndexOfNode { get; private set; }
        public int NodeinArr { get; private set; }
        public int ParentIndex { get; private set; }
        public Node Left { get; set; } = null!;
        public Node Right { get; set; } = null!;
        public int NodeLevel { get; private set; }
        public void SetValue(string val)
        {
            Value = val;
            ValueSize = base.GetSize(val);
        }
        public Node(string value, int Level, int index)
        {

            //LeftIndex = 2 * IndexOfNode + 1;
            //RightIndex = 2 * IndexOfNode + 2;

            SetValue(value);
            NodeLevel = Level;
            NodeinArr = index;

            IndexOfNode = NodeinArr + (int)Math.Pow(2, NodeLevel) - 1;
            ParentIndex = IndexOfNode % 2 == 0 ? IndexOfNode / 2 - 1 : IndexOfNode / 2;



        }
    }
}
