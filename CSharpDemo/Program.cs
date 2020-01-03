using System;
using System.Collections.Generic;

namespace CSharpDemo
{

    class LivingCreature { 

        //NOTICE the "virtual"
        public virtual void SayHello() {
            Console.WriteLine("Wave the hand");       
        }
    }

    class Human : LivingCreature
    {
        //Versus override
        public override void SayHello()
        {
            Console.WriteLine("Hello!");
            base.SayHello();
        }
    }

    class Dog : LivingCreature
    {
        public override void SayHello()
        {
            Console.WriteLine("Wouf!");
            base.SayHello();
        }
    }

    /**
     
        LivingCreature
            + virtual SayHello()
         /\
         |
        Human
            + override SayHello();       
         
         
         
         */

    class Worm : LivingCreature {
        public override void SayHello()
        {
            Console.WriteLine("aslkjdlakjsdlkajsd");
            //base.SayHello(); //Because worm have no hand, i dont want to call the BaseCall SayHello implementation so i just ingore it
        }
    }



    class Program
    {
        /*
      
            Virtual & Override Example

            Method overriding, in OOP, is a language feature that allows a subclass or child class to provide a specific implementation of a method
            that is already provided by one of its superclasses or parent classes.

            virtual method = A method that CAN OR SHOULD be override by Inherited type
            override method = A method that OVERRIDE the fonctionality of the Base type
            base.method() = Call the base type virtual method, more precisely, call the Parent type method implementation

            Override
            what override stand for? 
                override, exceed, go beyond, re-write, re-define


            Output

             The Human:
             Hello!  (the human implementation of Say hello outputed Hello!
             Wave the hand (the living creature implementation of Say Hello output Wave the hand )

             The Dog:
             Wouf!  (the Dog implementation of Say hello outputed Hello!
             Wave the hand (the living creature implementation of Say Hello output Wave the hand )

             The Worm:
             aslkjdlakjsdlkajsd (the worm implementation of Say hello outputed Hello!
             
         */

        static void Main(string[] args)
        {
            var human = new Human();
            var dog = new Dog();
            var worm = new Worm();

            Console.WriteLine("The Human:");
            human.SayHello();

            Console.WriteLine();
            Console.WriteLine("The Dog:");
            dog.SayHello();

            Console.WriteLine();
            Console.WriteLine("The Worm:");
            worm.SayHello();

            //OOP Polymorphism

            LivingCreature human1 = new Human();
            LivingCreature dog1 = new Dog(); //But wait ? what? why is LivingCreature AND DOG on the same line? Beucase Dog IS A LivingCreature, thecnicaly Dog is also a LivingCreature, Dog EXTEND LivingCreature

            dog1.SayHello(); // will produce Wouf! + Wave the hand.... why? because in memory dog1 is an instance of Dog

            //what is cool about that is the following...

            List<LivingCreature> creatures = new List<LivingCreature>();

            creatures.Add(new Human());
            creatures.Add(new Dog());
            creatures.Add(new Worm());

            //Notice that i dont need to KNOW about the FINAL implementation of the class of the instance
            foreach (var creature in creatures)
                creature.SayHello(); //Depending on the FINAL implementation of the instance, the output will be different,  therefore i can code engine that work with LivingCreature


        }
    }
}
