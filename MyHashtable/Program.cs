using System;

namespace MyHashtable
{
    class GameObjectData
    {
        private string name;
        private int level;
        private int hp;
        public GameObjectData(string name, int level, int hp)
        {
            this.name = name;
            this.level = level;
            this.hp = hp;
        }
        public void PrintData()
        {
            Console.WriteLine($"{name}(Lv.{level}, HP:{hp})");
        }
    }
    class MyHashtable
    {
        private Node[] bucket;
        public MyHashtable()
        {
            this.bucket = new Node[100];
        }
        private class Node
        {
            public string key;
            public GameObjectData value;
            public Node Next;
            public int count;
            public Node(string key, GameObjectData value)
            {
                this.key = key;
                this.value = value;
                this.Next = null;
                count = 0;
            }
        }
        public int MyHashCode(string key)
        {
            int hash = 0;
            for(int i = 0; i < key.Length; i++)
            {
                hash = 997 * hash + key[i]; //997 : prime number
            }
            hash = hash >> 16;
            if(hash < 0)
            {
                hash *= -1;
            }
            return hash;
        }
        public int MyHashCode2(string key)
        {
            int hash = 0;
            for(int i = 0; i < key.Length; i++)
            {
                hash = 733 * hash + key[i]; //733 : prime number
            }
            hash = hash >> 5;
            if(hash < 0)
            {
                hash *= -1;
            }
            return hash;
        }
        private bool AddNode(int functionType, string key, GameObjectData value)
        {
            int code;
            if(functionType == 1)
                code = MyHashCode(key) % 100;
            else
                code = MyHashCode2(key) % 100;

            if(bucket[code] == null)
            {
                bucket[code] = new Node(key, value);
                return true;
            }
            else
            {
                if(bucket[code].count < 4)
                {
                    Node newNode = new Node(key, value);
                    newNode.count = bucket[code].count + 1; 
                    newNode.Next = bucket[code];
                    bucket[code] = newNode;
                    return true;
                }
            }
            return false;
        }
        private GameObjectData RemoveNode(ref Node node, string key)
        {
            GameObjectData value = null;
            if(node != null)
            {
                if(node.key == key)
                {
                    value = node.value;
                    node = node.Next;
                }
            }
            return value;
        }
        public void Add(string key, GameObjectData value)
        {
            if(AddNode(1, key, value) == false)
            {
                if(AddNode(2, key, value) == false)
                {
                    Console.WriteLine("해당 해시코드에는 이미 5개의 노드가 존재합니다.");
                }
            }
        }
        public GameObjectData Delete(string key)
        {
            int code = MyHashCode(key) % 100;
            GameObjectData value = null;

            value = RemoveNode(ref bucket[code], key);
            if(value == null)
                value = RemoveNode(ref bucket[code].Next, key);
            if(value == null)
                value = RemoveNode(ref bucket[code].Next.Next, key);
            if(value == null)
                value = RemoveNode(ref bucket[code].Next.Next.Next, key);
            if(value == null)
                value = RemoveNode(ref bucket[code].Next.Next.Next.Next, key);
            if(value == null)
            {
                code = MyHashCode2(key) % 100;
                value = RemoveNode(ref bucket[code], key);
            }
            if(value == null)
                value = RemoveNode(ref bucket[code].Next, key);
            if(value == null)
                value = RemoveNode(ref bucket[code].Next.Next, key);
            if(value == null)
                value = RemoveNode(ref bucket[code].Next.Next.Next, key);
            if(value == null)
                value = RemoveNode(ref bucket[code].Next.Next.Next.Next, key);

            return value;
        }
        public GameObjectData Search(string key)
        {
            int code = MyHashCode(key) % 100;
            GameObjectData value = null;
            if(bucket[code] != null)
            {
                for(Node node = bucket[code]; node != null; node = node.Next)
                {
                    if(node.key == key)
                    {
                        value = node.value;
                    }
                }
            }
            if(value == null)
            {
                code = MyHashCode2(key) % 100;
                if(bucket[code] != null)
                {
                    for(Node node = bucket[code]; node != null; node = node.Next)
                    {
                        if(node.key == key)
                        {
                            value = node.value;
                        }
                    }
                }
            }
            return value;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MyHashtable Mht = new MyHashtable();
            Mht.Add("Slime", new GameObjectData("Slime", 1, 10));
            Mht.Add("Goblin", new GameObjectData("Goblin", 3, 15));
            Mht.Add("Ogre", new GameObjectData("Ogre", 5, 20));
            Mht.Add("Golem", new GameObjectData("Golem", 8, 50));
            Mht.Add("Balrog", new GameObjectData("Balrog", 10, 100));

            Mht.Delete("Slime");

            //Slime검색
            if(Mht.Search("Slime") != null)
                Mht.Search("Slime").PrintData();
            else
                Console.WriteLine("찾기 못했습니다.");

            //Golem검색
            if(Mht.Search("Golem") != null)
                Mht.Search("Golem").PrintData();
            else
                Console.WriteLine("찾기 못했습니다.");
                
            //Ogre검색
            if(Mht.Search("Ogre") != null)
                Mht.Search("Ogre").PrintData();
            else
                Console.WriteLine("찾기 못했습니다.");
        }
    }
}
