using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RuntimeTranslator
{
    private static class English
    {
        public static string TranslateTaskType(AppManager.TaskType _type)
        {
            switch (_type)
            {
                case AppManager.TaskType.Maximum:
                    return "Maximum";

                case AppManager.TaskType.Minimum:
                    return "Minimum";

                case AppManager.TaskType.Boolean:
                    return "Done/Not done";

                case AppManager.TaskType.Optimum:
                    return "Optimum";

                case AppManager.TaskType.Interval:
                    return "Interval";

                default:
                    return "";
            }
        }
        public static string TranslateDayOfWeek(DayOfWeek _day)
        {
            switch(_day)
            {
                case DayOfWeek.Monday:
                    return _day.ToString();
                
                case DayOfWeek.Tuesday:
                    return _day.ToString();

                case DayOfWeek.Wednesday:
                    return _day.ToString();

                case DayOfWeek.Thursday:
                    return _day.ToString();

                case DayOfWeek.Friday:
                    return _day.ToString();

                case DayOfWeek.Saturday:
                    return _day.ToString();

                case DayOfWeek.Sunday:
                    return _day.ToString();

                default:

                    return "";
            }
        }

        public static string TranslateTaskMetricType(AppManager.TaskMetricType _type)
        {
            switch(_type)
            {
                case AppManager.TaskMetricType.Pieces:
                    return AppManager.TaskMetricType.Pieces.ToString();
                    
                case AppManager.TaskMetricType.Grams:
                    return AppManager.TaskMetricType.Grams.ToString();
                    
                case AppManager.TaskMetricType.Kilometres:
                    return AppManager.TaskMetricType.Kilometres.ToString();
                   
                case AppManager.TaskMetricType.Minutes:
                    return AppManager.TaskMetricType.Minutes.ToString();
                    
                case AppManager.TaskMetricType.Other:
                    return  AppManager.TaskMetricType.Other.ToString();

                case AppManager.TaskMetricType.Mile:
                    return AppManager.TaskMetricType.Mile.ToString();

                case AppManager.TaskMetricType.Pound:
                    return AppManager.TaskMetricType.Pound.ToString();

                case AppManager.TaskMetricType.Calories:
                    return AppManager.TaskMetricType.Calories.ToString();

                default:
                    Debug.LogError($"AppManager.TaskMetricType doesn't contain this type: {_type}");
                    return "AppManager.TaskMetricType doesn't contain this type";
                    
            }
        }

        public static string TranslatePointsWord()
        {
            return "Points";
        }

        public static string TranslateIntervalWord()
        {
            return "Interval";
        }
    }

    private static class Hungarian
    {
        public static string TranslateTaskType(AppManager.TaskType _type)
        {
            switch (_type)
            {
                case AppManager.TaskType.Maximum:
                    return "Maximum";

                case AppManager.TaskType.Minimum:
                    return "Minimum";

                case AppManager.TaskType.Boolean:
                    return "Kész/Nincs kész";

                case AppManager.TaskType.Optimum:
                    return "Optimum";

                case AppManager.TaskType.Interval:
                    return "Intervallum";

                default:
                    return "";
            }
        }

        public static string TranslateDayOfWeek(DayOfWeek _day)
        {
           
                switch (_day)
                {
                    case DayOfWeek.Monday:
                        return "Hétfõ";


                    case DayOfWeek.Tuesday:
                        return "Kedd";


                    case DayOfWeek.Wednesday:
                        return "Szerda";


                    case DayOfWeek.Thursday:
                        return "Csütörtök";

                    case DayOfWeek.Friday:
                        return "Péntek";

                    case DayOfWeek.Saturday:
                        return "Szombat";

                    case DayOfWeek.Sunday:
                        return "Vasárnap";

                    default:
                        return "";
                }
            
        }

        public static string TranslateTaskMetricType(AppManager.TaskMetricType _type)
        {
            switch (_type)
            {
                case AppManager.TaskMetricType.Pieces:
                    return "Darab";

                case AppManager.TaskMetricType.Grams:
                    return "Gramm";

                case AppManager.TaskMetricType.Kilometres:
                    return "Kilométer";

                case AppManager.TaskMetricType.Minutes:
                    return "Perc";

                case AppManager.TaskMetricType.Other:
                    return "Egyéb";

                case AppManager.TaskMetricType.Mile:
                    return "Mérföld";

                case AppManager.TaskMetricType.Pound:
                    return "Font";

                case AppManager.TaskMetricType.Calories:
                    return "Kalória";

                default:
                    Debug.LogError($"AppManager.TaskMetricType doesn't contain this type: {_type}");
                    return "AppManager.TaskMetricType doesn't contain this type";

            }
        }

        public static string TranslatePointsWord()
        {
            return "Pont";
        }

        public static string TranslateIntervalWord()
        {
            return "Intervallum";
        }
    }



    private static class German
    {

        
        public static string TranslateTaskType(AppManager.TaskType _type)
        {
            switch (_type)
            {
                case AppManager.TaskType.Maximum:
                    return "Maximum";

                case AppManager.TaskType.Minimum:
                    return "Minimum";

                case AppManager.TaskType.Boolean:
                    return "Getan/Nicht getan";

                case AppManager.TaskType.Optimum:
                    return "Optimum";

                case AppManager.TaskType.Interval:
                    return "Intervall";

                default:
                    return "";
            }
        }
        

        public static string TranslateDayOfWeek(DayOfWeek _day)
        {
            switch (_day)
            {
                case DayOfWeek.Monday:
                    return "Montag";


                case DayOfWeek.Tuesday:
                    return "Dienstag";


                case DayOfWeek.Wednesday:
                    return "Mittwoch";


                case DayOfWeek.Thursday:
                    return "Donnerstag";

                case DayOfWeek.Friday:
                    return "Freitag";

                case DayOfWeek.Saturday:
                    return "Samstag";

                case DayOfWeek.Sunday:
                    return "Sonnstag";

                default:
                    return "";
            }
        }


        public static string TranslateTaskMetricType(AppManager.TaskMetricType _type)
        {
            switch (_type)
            {
                case AppManager.TaskMetricType.Pieces:
                    return "Stück";

                case AppManager.TaskMetricType.Grams:
                    return "Gramm";

                case AppManager.TaskMetricType.Kilometres:
                    return "Kilometer";

                case AppManager.TaskMetricType.Minutes:
                    return "Minute";

                case AppManager.TaskMetricType.Other:
                    return "Andere";

                case AppManager.TaskMetricType.Mile:
                    return "Meile";

                case AppManager.TaskMetricType.Pound:
                    return "Pfund";

                case AppManager.TaskMetricType.Calories:
                    return "Kalorie";


                default:
                    Debug.LogError($"AppManager.TaskMetricType doesn't contain this type: {_type}");
                    return "AppManager.TaskMetricType doesn't contain this type";

            }
        }

        public static string TranslatePointsWord()
        {
            return "Punkte";
        }

        public static string TranslateIntervalWord()
        {
            return "Intervall";
        }
    }


    
    public static string TranslateTaskType(AppManager.TaskType _type)
    {
        


        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.TranslateTaskType(_type);
            case AppManager.Languages.Magyar:
                return Hungarian.TranslateTaskType(_type);
            case AppManager.Languages.Deutsch:
                return German.TranslateTaskType(_type);
            default:

                return "AppManager.Languages doesn't contain this language";

        }
    }
    

   public static string TranslateTaskMetricType(AppManager.TaskMetricType _type)
   {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.TranslateTaskMetricType(_type);
            case AppManager.Languages.Magyar:
                return Hungarian.TranslateTaskMetricType(_type);
            case AppManager.Languages.Deutsch:
                return German.TranslateTaskMetricType(_type);
            default:
              
                return "AppManager.Languages doesn't contain this language";

        }   
   }
    public static string TranslatePointsWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.TranslatePointsWord();
            case AppManager.Languages.Magyar:
                return Hungarian.TranslatePointsWord();
            case AppManager.Languages.Deutsch:
                return German.TranslatePointsWord();
            default:
                return "AppManager.Languages doesn't contain this language";

        }
    }

    public static string TranslateIntervalWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.TranslateIntervalWord();
            case AppManager.Languages.Magyar:
                return Hungarian.TranslateIntervalWord();
            case AppManager.Languages.Deutsch:
                return German.TranslateIntervalWord();
            default:
                return "AppManager.Languages doesn't contain this language";

        }
    }

    public static string TranslateDayOfWeek(DayOfWeek _day)
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return English.TranslateDayOfWeek(_day);
            case AppManager.Languages.Magyar:
                return Hungarian.TranslateDayOfWeek(_day);
            case AppManager.Languages.Deutsch:
                return German.TranslateDayOfWeek(_day);
            default:
                return "AppManager.Languages doesn't contain this language";

        }
    }

    public static string TranslateWeeklyWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return "Weekly";
            case AppManager.Languages.Magyar:
                return "Heti";
            case AppManager.Languages.Deutsch:
                return "Wöchentlich";
            default:
                return "AppManager.Languages doesn't contain this language";

        }
    }

    public static string TranslateMonthlyWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return "Monthly";
            case AppManager.Languages.Magyar:
                return "Havi";
            case AppManager.Languages.Deutsch:
                return "Monatlich";
            default:
                return "AppManager.Languages doesn't contain this language";

        }
    }

    public static string TranslateDailyWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return "Daily";
            case AppManager.Languages.Magyar:
                return "Napi";
            case AppManager.Languages.Deutsch:
                return "Täglich";
            default:
                return "AppManager.Languages doesn't contain this language";
        }
    }

    public static string TranslateAllTimeWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return "All Time";
            case AppManager.Languages.Magyar:
                return "Összesített";
            case AppManager.Languages.Deutsch:
                return "Alle Zeit";
            default:
                return "AppManager.Languages doesn't contain this language";
        }
    }

    public static string TranslateOverallWord()
    {
        switch(AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return "Overall";
            case AppManager.Languages.Magyar:
                return "Összes";
            case AppManager.Languages.Deutsch:
                return "";
            default:
                return "AppManager.Languages doesn't contain this language";
        }
    }

    public static string TranslateEveryWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return "Every";
            case AppManager.Languages.Magyar:
                return "Minden";
            case AppManager.Languages.Deutsch:
                return "";
            default:
                return "AppManager.Languages doesn't contain this language";
        }
    }

    public static string TranslateDayWord()
    {
        switch (AppManager.currentLanguage)
        {
            case AppManager.Languages.English:
                return "day";
            case AppManager.Languages.Magyar:
                return "nap";
            case AppManager.Languages.Deutsch:
                return "";
            default:
                return "AppManager.Languages doesn't contain this language";
        }
    }

}
