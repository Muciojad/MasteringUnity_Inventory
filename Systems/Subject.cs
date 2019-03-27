using System.Collections.Generic;
using System;
using UnityEngine;


[System.Serializable]
public class Subject
{
    //A list with observers that are waiting for something to happen
    List<Observer> observers = new List<Observer>();

    //Send notifications if something has happened
    
    public void Notify(object @event)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            //Notify all observers even though some may not be interested in what has happened
            //Each observer should check if it is interested in this event
            observers[i].OnNotify(@event);
        }
    }

    //Add observer to the list
    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    //Remove observer from the list
    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }
}