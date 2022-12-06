using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Lab11
{
    static class Reflector<T>
    {
        public static Type myType = typeof(Person);
        public static Assembly myAssembly=myType.Assembly;
        public static ConstructorInfo[] myInfo = myType.GetConstructors();
        public static MethodInfo[] myMethodInfo = myType.GetMethods();
        public static FieldInfo[] myFieldInfo = myType.GetFields();
        public static PropertyInfo[] myPropertyInfo = myType.GetProperties();
        public static Type[]myInterfacesInfo=myType.GetInterfaces();
        public static IEnumerable<string> GetPublicMethods(string ClassName)
        {
            return Type.GetType(ClassName).GetMethods().Select(t => t.Name);
        }
        public static IEnumerable<string> GetFields(string ClassName)
        {
            return Type.GetType(ClassName).GetFields().Select(t => t.Name).Union(Type.GetType(ClassName).GetProperties().Select(t => t.Name));
        }
        public static IEnumerable<string> GetInterfaces(string ClassName)
        {
            return Type.GetType(ClassName).GetInterfaces().Select(n => n.FullName);
        }
        public static void Invoke(T t, string  methodName)
        {
            Type type = t.GetType();
            try
            {

                string[] _params =File.ReadAllLines(@"C:\Пацей\Lab11\Lab11\info.txt");
                Console.WriteLine("Результат выполнения метода : ");
                for (int i = 0; i < _params.Length; i++)
                {
                   
                    MethodInfo method = type.GetMethod(methodName);
                    methodName = _params[i];
                    Console.WriteLine(method?.Invoke(t, parameters: null));
                 
                  
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }

        }
        public static object? Create(T ClassName)
        {
           Type ObjType=ClassName.GetType();
            return Activator.CreateInstance(ObjType);
            
        }


    }
    public class Person : IEater, IMovable
    {
        public Person(string name) => Name = name;
        public string Name { get; }
        public Person() { }
        static Person() { }
        public void Eat() => Console.WriteLine($"{Name} eats");

        public void Move() => Console.WriteLine($"{Name} moves");

    }
    interface IEater
    {
        void Eat();
    }
    interface IMovable
    {
        void Move();
    }
    class Program
    {
        static  void Main(string[] args)
        {
            Person person = new Person();
            string path = @"C:\Пацей\Lab11\Lab11\info.txt";
            Console.WriteLine($"Имя сборки, в которой определен класс:{Reflector<string>.myAssembly}");
            Console.WriteLine();

            Console.Write("Публичный конструкторы:");
            for (int i = 0; i < Reflector<string>.myInfo.Length; i++)
                Console.Write($"{Reflector<string>.myInfo[i]}, ");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Публичные методы класса:");
            var met = Reflector<Person>.GetPublicMethods(person.GetType().FullName);
            File.AppendAllLines(path, met);
            foreach (string s in met)
            {
                Console.WriteLine(s);
               
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Публичные поля и свойства класса:");
            var fields = Reflector<Person>.GetFields(person.GetType().FullName);
            File.AppendAllLines(path, fields);
            foreach (string s in fields)
            {
                Console.WriteLine(s);
              

            }      
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Реализованные классом интерфейсы:");
            var interfaces = Reflector<Person>.GetInterfaces(person.GetType().FullName);
            File.AppendAllLines(path, interfaces);
            foreach (string s in interfaces)
            {
                Console.WriteLine(s);
            }
            Console.Write("Введите имя класса:");
            string Name = Console.ReadLine();
            var met1 = Reflector<Person>.GetPublicMethods(Name.GetType().FullName);
            foreach (string s in met1)
            {
                Console.WriteLine(s);

            }
            Console.WriteLine();
       
            Reflector<Person>.Invoke(person,"Eat");
            Console.WriteLine();
            Console.WriteLine($"{Reflector<Person>.Create(person)}");
       


        }
    }
   
}