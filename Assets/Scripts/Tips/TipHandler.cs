using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipHandler : MonoBehaviour
{
   public struct Tip
   {
        public string header;
        public string content;

        public Tip (string _h,string _c)
        {
            header = _h;
            content = _c;
        }
   }

    public struct TipSaveData
    {
        public int id;
        public bool isSaved;

   
        public TipSaveData(int _id, bool _isSaved)
        {
            id = _id;
            isSaved = _isSaved;
        }
    }

    private void CreateTip(int _id, string _h, string _c, bool _isSaved)
    {
        currentTips.Add(new TipSaveData(_id, _isSaved), new Tip(_h, _c));
    }

    private void SetTipSaved()
    {

    }

    private Dictionary<TipSaveData, Tip> currentTips = new Dictionary<TipSaveData, Tip>();

    private void Awake()
    {
        AppManager.OnLanguageChanged += Initialize;
    }

    private void OnDestroy()
    {
        AppManager.OnLanguageChanged -= Initialize;
    }


    private void Start()
    {

        Initialize(AppManager.currentLanguage);
        LoadSavedTips();
       
    }

    private void Initialize(AppManager.Languages _l)
    {
        TextAsset textAsset;
        switch (_l)
        {
            case AppManager.Languages.English:
                textAsset = (TextAsset)Resources.Load("tips_en");
                if (textAsset == null)
                {
                    Debug.LogError("There are no tips en");
                }

                string[] _lines_en = textAsset.ToString().Split('\n');
                //    print(_lines.Length);

                foreach (string _line in _lines_en)
                {
                    string[] _parts = _line.Split('_');
                    CreateTip(int.Parse(_parts[0]), _parts[1], _parts[2], false);
                }
                break;
            case AppManager.Languages.Magyar:
                textAsset = (TextAsset)Resources.Load("tips_hu");
                if (textAsset == null)
                {
                    Debug.LogError("There are no tips hu");
                }

                string[] _lines_hu = textAsset.ToString().Split('\n');
                //    print(_lines.Length);

                foreach (string _line in _lines_hu)
                {
                    string[] _parts = _line.Split('_');
                    CreateTip(int.Parse(_parts[0]), _parts[1], _parts[2], false);
                }
                break;
            default:

                break;
        }
    }

    private void LoadSavedTips()
    {

    }
}
