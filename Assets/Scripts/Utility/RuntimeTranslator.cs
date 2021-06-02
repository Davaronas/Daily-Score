using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RuntimeTranslator
{
    private static class English
    {
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
                    return AppManager.TaskMetricType.Other.ToString();

                case AppManager.TaskMetricType.Mile:
                    return AppManager.TaskMetricType.Mile.ToString();

                case AppManager.TaskMetricType.Pound:
                    return AppManager.TaskMetricType.Pound.ToString();
                    
                default:
                    Debug.LogError($"AppManager.TaskMetricType doesn't contain this type: {_type}");
                    return "AppManager.TaskMetricType doesn't contain this type";
                    
            }
        }
    }

    private static class Hungarian
    {
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

                default:
                    Debug.LogError($"AppManager.TaskMetricType doesn't contain this type: {_type}");
                    return "AppManager.TaskMetricType doesn't contain this type";

            }
        }
    }

    private static class German
    {
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


                default:
                    Debug.LogError($"AppManager.TaskMetricType doesn't contain this type: {_type}");
                    return "AppManager.TaskMetricType doesn't contain this type";

            }
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
                Debug.LogError($"AppManager.Languages doesn't contain this type: {_type}");
                return "AppManager.Languages doesn't contain this type";

        }   
   }


}
