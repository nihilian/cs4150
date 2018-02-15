using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kattis.IO;

namespace RumorMill
{
    class Program
    {
        private class Student
        {
            public string Name { private set; get; }

            public bool visited;

            private HashSet<Student> friends;
            public Student(string name)
            {
                Name = name;
                friends = new HashSet<Student>();
                visited = false;
            }

            public void AddFriend(ref Student friend)
            {
                friends.Add(friend);
            }

            public string GetFriends()
            {
                string friendNames = this.Name;
                this.visited = true;
                Queue<Student> queue = new Queue<Student>();
                Queue<Student> queue2 = new Queue<Student>();
                List<string> immediateFriends = new List<string>();
                foreach(Student friend in friends)
                {
                    if (!friend.visited)
                    {
                        immediateFriends.Add(friend.Name);
                        friend.visited = true;
                        queue.Enqueue(friend);
                    }
                }
                immediateFriends.Sort();
                foreach (string immediateFriend in immediateFriends)
                    friendNames = friendNames + " " + immediateFriend;

                List<string> friendsOfFriends = new List<string>();
                while (queue.Count > 0)
                {
                    Student friend = queue.Dequeue();
                    foreach(Student friendOfFriend in friend.friends)
                    {
                        if(!friendOfFriend.visited)
                        {
                            friendsOfFriends.Add(friendOfFriend.Name);
                            friendOfFriend.visited = true;
                            queue2.Enqueue(friendOfFriend);
                        }
                    }
                    if (queue.Count == 0)
                    {
                        friendsOfFriends.Sort();
                        foreach (string studName in friendsOfFriends)
                        {
                            friendNames = friendNames + " " + studName;
                        }
                        friendsOfFriends.Clear();
                        while (queue2.Count > 0)
                            queue.Enqueue(queue2.Dequeue());
                    }
                }

                return friendNames;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

        }

        static void Main(string[] args)
        {
            Dictionary<string, Student > dictionary = new Dictionary<string, Student>();
            string output = "";
            Scanner scanner = new Scanner();

            int n = scanner.NextInt();
            for (int i = 0; i < n; i++)
            {
                string name = scanner.Next();
                Student student = new Student(name);
                dictionary.Add(name, student);
            }

            int f = scanner.NextInt();
            for (int i = 0; i < f; i++)
            {
                string student = scanner.Next();
                string friend = scanner.Next();
                Student s1, s2;
                dictionary.TryGetValue(student, out s1);
                dictionary.TryGetValue(friend, out s2);
                s1.AddFriend(ref s2);
                s2.AddFriend(ref s1);
            }

            int r = scanner.NextInt();
            for (int i = 0; i < r; i++)
            {
                string rumorStart = scanner.Next();
                Student rumorStudent;
                dictionary.TryGetValue(rumorStart, out rumorStudent);
                string friendNames = rumorStudent.GetFriends();
                List<string> otherNames = new List<string>();
                foreach(Student stud in dictionary.Values)
                {
                    if (stud.visited)
                        stud.visited = false;
                    else
                        otherNames.Add(stud.Name);
                }
                otherNames.Sort();
                foreach (string otherName in otherNames)
                    friendNames = friendNames + " " + otherName;
                output = output + friendNames;
                if (i + 1 != r)
                    output = output + "\n";
            }

            Console.WriteLine(output);
            Console.Read();
        }
    }
}
