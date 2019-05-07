using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    public Light lgt;
    [Header("Minuti necessari a far passsare 12 ore in gioco")]
    public  float duration;
    public  static bool Night = false;
    private static int[] date = new int[2]{ 1, 1};//date[0]<-Giorni date[1]<-Mesi
    private static int[] time = new int[2] { 0, 0 }; //time[0]<-Ore time[1]<-Minuti
    private int tmp = 0;
    private float COST_TIME;
    private static string[] month = new string[12]{ "Exitium" , "Origo","Hiems","Vigil","Flos","Core","Solaris","Sacris","Aestus","Mundus","Lunaris","Vacuum"};
    private static string[] dayname = new string[7] { "Sul", "Lun", "Meurzh", "Merc'her", "Yaou", "Gwener", "Sardon"};
    private static string[] season = new string[4] { "Autunno", "Inverno", "Primavera", "Estate" };
    Vector3 direction;

    public static int CurrentDay() 
    {
        return date[0];
    }
    public static int CurrentMonth()
    {
        return date[1];
    }
    public static int CurrentSeason()
    {
        return (date[1] % 4);
    }
    public static string GetMonthName( int n_month )
    {
        return month[n_month];
    }
    public static string GetDayName(int n_day)
    {
        return dayname[n_day % 7];
    }
    public static string GetSeasonName(int n_month)
    {
        return season[n_month % 4];
    }
    public static int GetHour()
    {
        return time[0];
    }
    public static int GetMinute()
    {
        return time[1];
    }

    // Use this for initialization
    void Start()
    {
        COST_TIME = duration;
    }

    // Update is called once per frame
    void Update()
    {
        //Durata 6 ore
        if (duration < 1) duration = 1;
        direction = new Vector3(3 / duration, 0, 0);
        //Rotazione
        double xpos = transform.eulerAngles.x;
        transform.Rotate(direction*Time.deltaTime);

        //tramonto
        if (xpos > 180 && Night == false)  Night = true;
        //Alba
        if (xpos > 0 && xpos < 180 && Night == true)
        {
            Night = false; 
            //mesi 31 giorni
            if      (date[0] < 31 && (date[1] == 1 || date[1] == 2 || date[1] == 7 || date[1] == 11)) date[0] += 1;
            else if (date[0] > 31 && (date[1] == 1 || date[1] == 2 || date[1] == 7 || date[1] == 11))
            {
                date[0] = 1;
                date[1] += 1;
            }
            //mesi 30 giorni
            if      (date[0] < 30 && (date[1] == 4 || date[1] == 5 || date[1] == 6 || date[1] == 8 || date[1] == 10)) date[0] += 1;
            else if (date[0] > 30 && (date[1] == 4 || date[1] == 5 || date[1] == 6 || date[1] == 8 || date[1] == 10))
            {
                date[0] = 1;
                date[1] += 1;
            }
            //mesi 29 giorni
            if      (date[0] < 29 && (date[1] == 9 || date[1] == 12)) date[0] += 1;
            else if (date[0] > 29 && (date[1] == 9 || date[1] == 12))
            {
                date[0] = 1;
                date[1] += 1;
            }
            //mese 28 giorni
            if      (date[0] < 28 && date[1] == 3) date[0] += 1;
            else if (date[0] > 28 && date[1] == 3)
            {
                date[0] = 1;
                date[1] += 1;
            }
            if (date[1] > 12) date[1] = 1; //Reset Mesi
        }
        //Allungamento ed accorciamento giornate in base alle stagioni
        if (CurrentSeason() == 1) duration = COST_TIME + COST_TIME / 7;
        if (CurrentSeason() == 2) duration = COST_TIME + COST_TIME/7;
        if (CurrentSeason() == 3) duration = COST_TIME + COST_TIME/5;
        if (CurrentSeason() == 0) duration = COST_TIME + COST_TIME/7;

        //Gestione intensità luminosa
        if (Night && lgt.intensity > 0)  lgt.intensity -= duration / 100;
        if (!Night && lgt.intensity < 1) lgt.intensity += duration / 100;
        if (lgt.intensity > 1) lgt.intensity = 1;
        //Orologio
        if(tmp != (int)Time.time)
        {
            tmp = (int)Time.time;
            if (time[1] < 60 - 60/(int)(((COST_TIME * 60) * 2) / 24)) time[1] += 60/(int)(((COST_TIME * 60) * 2) / 24);
            else
            {
                time[1] = 0;
                if (time[0] < 23) time[0] += 1;
                else time[0] = 0;
            }
        }
       
      

        Debug.Log("Giorno: " + date[0] + " - " + GetDayName(date[0]) + "  /  Mese: " + month[date[1] - 1] + "  /  Stagione: " + GetSeasonName(date[1]) + "  /  Ore " + GetHour() + " : " + GetMinute() + "DBG: " + duration);
    }
}
