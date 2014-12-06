using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PVK.Control.Presenter
{
    public interface Strategy
     {
     void DoAlgo();
     }
 
    class Context
     {
     private Strategy strategy;
     public Context(Strategy strategy)
     {
     this.strategy = strategy;
     }
     public void Execute()
     {
     this.strategy.DoAlgo();
     }
     }
 
    public class Sort: Strategy
     {
     public void DoAlgo()
     {
     Console.WriteLine("I am sorting, really!!! xDDD");
     }
     }
 
    public class Compute: Strategy
     {
     public void DoAlgo()
     {
     Console.WriteLine("I am computing, yah-yah!!! xDDD");
     }
     }
}
