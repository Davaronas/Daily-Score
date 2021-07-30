using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    [System.Serializable]
    public struct TipSaveData
    {
        public int id;
        public bool isSaved;
        public string lastShown;
   
        public TipSaveData(int _id, bool _isSaved, string _lastShown)
        {
            id = _id;
            isSaved = _isSaved;
            lastShown = _lastShown;
        }

        public DateTime GetLastShownDate()
        {
            return  Convert.ToDateTime(lastShown);
        }
    }

    private Dictionary<TipSaveData, Tip> currentTips = new Dictionary<TipSaveData, Tip>();
    private TipManager tipManager = null;
    private int maxId = 0;
    private int firstTipId = 0;
    private int secondTipId = 0;

    private void CreateTip(int _id, string _h, string _c, bool _isSaved)
    {
        currentTips.Add(new TipSaveData(_id, _isSaved, DateTime.MinValue.ToString()), new Tip(_h, _c));
    }

    public void RemoteCall_SaveFirstTip()
    {
        SetTipSaved(firstTipId, true);
        tipManager.BlockFirstTipSaveButton();
    }

    public void RemoteCall_SaveSecondTip()
    {
        SetTipSaved(secondTipId, true);
        tipManager.BlockSecondTipSaveButton();
    }

    public void SetTipSaved(int _id, bool _state)
    {
        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            if (_tip.Key.id == _id)
            {

                currentTips.Remove(_tip.Key);
                KeyValuePair<TipSaveData, Tip> _mod = new KeyValuePair<TipSaveData, Tip>
                    (new TipSaveData(_tip.Key.id, _state, _tip.Key.lastShown), new Tip(_tip.Value.header, _tip.Value.content));
                currentTips.Add(_mod.Key, _mod.Value);
                if (_state)
                {
                    tipManager.AddSavedTip(_tip.Key.id, _tip.Value.header);
                }
                return;
            }
        }

        SaveTips();
    }

    public bool IsIdSaved(int _id)
    {
        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            if(_tip.Key.id == _id && _tip.Key.isSaved)
            {
                return true;
            }
        }

        return false;
    }

    private TipSaveData[] ExtractTipSavedDatas()
    {
        List<TipSaveData> _savedTips = new List<TipSaveData>();
        foreach(KeyValuePair<TipSaveData,Tip> _tip in currentTips)
        {
            _savedTips.Add(_tip.Key);
            print(_tip.Key.lastShown + " " + _tip.Key.id);
        }


        return _savedTips.ToArray();
    }


    private void Awake()
    {
        AppManager.OnLanguageChanged += Initialize;
        tipManager = FindObjectOfType<TipManager>();



    }

    private void OnDestroy()
    {
        AppManager.OnLanguageChanged -= Initialize;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.SetInt("firstTipId", -1);
            PlayerPrefs.SetInt("secondTipId", -1);
        }
    }

  

    private void Start()
    {

        if (AppManager.lastLogin.Date != DateTime.Now.Date)
        {
            PlayerPrefs.SetInt("firstTipId", -1);
            PlayerPrefs.SetInt("secondTipId", -1);
        }
        Initialize(AppManager.currentLanguage);
    }

   
    private void LoadMainTip()
    {
        firstTipId = PlayerPrefs.GetInt("firstTipId", -1);
        print("First player prefs: " + firstTipId);

        // find oldest first (DateTIme.minvalue)

        if (AppManager.lastLogin.Date == DateTime.Now.Date)
        {
            if (firstTipId != -1)
            {
                foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
                {
                    if (_tip.Key.id == firstTipId)
                    {
                        tipManager.LoadDailyTip(_tip.Key.id, _tip.Value.header, _tip.Value.content);
                        currentTips.Remove(_tip.Key);
                        KeyValuePair<TipSaveData, Tip> _mod = new KeyValuePair<TipSaveData, Tip>
                            (new TipSaveData(_tip.Key.id, _tip.Key.isSaved, DateTime.Now.Date.ToString()), new Tip(_tip.Value.header, _tip.Value.content));
                        currentTips.Add(_mod.Key, _mod.Value);

                        print(_tip.Key.lastShown + " changed from" + _tip.Key.id);

                        print(_mod.Key.lastShown + " new last shown " + _mod.Key.id);


                        PlayerPrefs.SetInt("firstTipId", _tip.Key.id);
                        print("Set first already picked earlier: " + _tip.Key.id);
                        return;
                    }
                }
            }
        }

        LoadRandomTip();
    }

    private void LoadRandomTip()
    {
        firstTipId = UnityEngine.Random.Range(0, maxId + 1);
        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            if(_tip.Key.GetLastShownDate() == DateTime.MinValue)
            {
                tipManager.LoadDailyTip(_tip.Key.id, _tip.Value.header, _tip.Value.content);
                currentTips.Remove(_tip.Key);
                KeyValuePair<TipSaveData, Tip> _mod = new KeyValuePair<TipSaveData, Tip>
                    (new TipSaveData(_tip.Key.id, _tip.Key.isSaved, DateTime.Now.Date.ToString()), new Tip(_tip.Value.header, _tip.Value.content));
                currentTips.Add(_mod.Key, _mod.Value);


                print(_tip.Key.lastShown + " changed from" + _tip.Key.id);
                print(_mod.Key.lastShown + " new last shown " + _mod.Key.id);

                PlayerPrefs.SetInt("firstTipId", _tip.Key.id);
                print("Set first equals minvalue: " + _tip.Key.id);
                return;
            }

            if (_tip.Key.id == firstTipId)
            {
                tipManager.LoadDailyTip(_tip.Key.id, _tip.Value.header, _tip.Value.content);
                currentTips.Remove(_tip.Key);
                KeyValuePair<TipSaveData, Tip> _mod = new KeyValuePair<TipSaveData, Tip>
                    (new TipSaveData(_tip.Key.id, _tip.Key.isSaved, DateTime.Now.Date.ToString()), new Tip(_tip.Value.header, _tip.Value.content));
                currentTips.Add(_mod.Key, _mod.Value);


                print(_tip.Key.lastShown + " changed from" + _tip.Key.id);
                print(_mod.Key.lastShown + " new last shown " + _mod.Key.id);

                PlayerPrefs.SetInt("firstTipId", _tip.Key.id);
                print("Set first: " + _tip.Key.id);
                return;
            }
        }
    }

    public int AskForSecondTip(out string _h, out string _c)
    {

        secondTipId = PlayerPrefs.GetInt("secondTipId", -1);
        print("Second player prefs: " + secondTipId);
        _h = "";
        _c = "";

        if (AppManager.lastLogin.Date == DateTime.Now.Date)
        {
            if (secondTipId != -1)
            {
                foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
                {
                    if(_tip.Key.id == secondTipId)
                    {
                        _h = _tip.Value.header;
                        _c = _tip.Value.content;
                        currentTips.Remove(_tip.Key);
                        KeyValuePair<TipSaveData, Tip> _mod = new KeyValuePair<TipSaveData, Tip>
                            (new TipSaveData(_tip.Key.id, _tip.Key.isSaved, DateTime.Now.Date.ToString()), new Tip(_tip.Value.header, _tip.Value.content));
                        currentTips.Add(_mod.Key, _mod.Value);


                        print(_tip.Key.lastShown + " changed from" + _tip.Key.id);
                        print(_mod.Key.lastShown + " new last shown " + _mod.Key.id);

                        PlayerPrefs.SetInt("secondTipId", _tip.Key.id);
                        print("Set second already picked earlier: " + _tip.Key.id);
                        return secondTipId;    
                    }
                }
            }
        }

        roll:
        secondTipId = UnityEngine.Random.Range(0, maxId + 1);
          if (secondTipId == firstTipId) goto roll;
        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            if (_tip.Key.GetLastShownDate() == DateTime.MinValue)
            {
                _h = _tip.Value.header;
                _c = _tip.Value.content;
                currentTips.Remove(_tip.Key);
                KeyValuePair<TipSaveData, Tip> _mod = new KeyValuePair<TipSaveData, Tip>
                    (new TipSaveData(_tip.Key.id, _tip.Key.isSaved, DateTime.Now.Date.ToString()), new Tip(_tip.Value.header, _tip.Value.content));
                currentTips.Add(_mod.Key, _mod.Value);


                print(_tip.Key.lastShown + " changed from" + _tip.Key.id);
                print(_mod.Key.lastShown + " new last shown " + _mod.Key.id);


                PlayerPrefs.SetInt("secondTipId", _tip.Key.id);
                print("Set second equals minvalue: " + _tip.Key.id);
                return _tip.Key.id;
            }

            if (_tip.Key.id == secondTipId)
            {
                _h = _tip.Value.header;
                _c = _tip.Value.content;
                currentTips.Remove(_tip.Key);
                KeyValuePair<TipSaveData, Tip> _mod = new KeyValuePair<TipSaveData, Tip>
                    (new TipSaveData(_tip.Key.id, _tip.Key.isSaved, DateTime.Now.Date.ToString()), new Tip(_tip.Value.header, _tip.Value.content));
                currentTips.Add(_mod.Key, _mod.Value);


                print(_tip.Key.lastShown + " changed from" + _tip.Key.id);
                print(_mod.Key.lastShown + " new last shown " + _mod.Key.id);

                PlayerPrefs.SetInt("secondTipId", _tip.Key.id);
                print("Set second: " + _tip.Key.id);
                return _tip.Key.id;
                
            }
        }

        SaveTips();

        return -1;

    }

    private void Initialize(AppManager.Languages _l)
    {
        currentTips.Clear();
       

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

        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            if(_tip.Key.id > maxId)
            {
                maxId = _tip.Key.id;
            }
        }


        LoadSavedTips();
        LoadMainTip();

        if (PlayerPrefs.GetInt("SecondTipUnlocked", 0) == 1)
        {
            tipManager.UnlockSecondTip();
        }

        SaveTips();
        tipManager.AlreadyContainsTipsCheck();
    }

    private void LoadSavedTips()
    {
        string path = Path.Combine(Application.persistentDataPath, "tipdata");
        if (File.Exists(path))
        {
            FileInfo fileInfo = new FileInfo(path);
            fileInfo.IsReadOnly = false;
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            TipSaveData[] _savedTips = formatter.Deserialize(stream) as TipSaveData[];

            /*
            foreach (TipSaveData _tsd in _savedTips)
            {
                if(_tsd.id > maxId)
                {
                    maxId = _tsd.id;
                    print(maxId);
                }
            }
            */

            SetSavedStateForTips(_savedTips);

            stream.Close();
            fileInfo.IsReadOnly = true;
        }
    }

    private void SetSavedStateForTips(TipSaveData[] _savedTips)
    {
        List<KeyValuePair<TipSaveData, Tip>> _remove = new List<KeyValuePair<TipSaveData, Tip>>();
        List<KeyValuePair<TipSaveData, Tip>> _add = new List<KeyValuePair<TipSaveData, Tip>>();
        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            for (int i = 0; i < _savedTips.Length; i++)
            {
                if (_tip.Key.id == _savedTips[i].id)
                {
                    _remove.Add(_tip);
                    _add.Add(new KeyValuePair<TipSaveData, Tip>(new TipSaveData(_tip.Key.id, _savedTips[i].isSaved, _savedTips[i].lastShown), new Tip(_tip.Value.header, _tip.Value.content)));
                }
            }
        }

        for (int i = 0; i < _remove.Count; i++)
        {
            currentTips.Remove(_remove[i].Key);
        }

        for (int i = 0; i < _add.Count; i++)
        {
            currentTips.Add(_add[i].Key, _add[i].Value);
        }

        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            if(_tip.Key.isSaved == true)
            {
                tipManager.AddSavedTip(_tip.Key.id, _tip.Value.header);
            }
        }
    }

    private void TipSaved(int _id)
    {
        foreach (KeyValuePair<TipSaveData, Tip> _tip in currentTips)
        {
            if (_tip.Key.id == _id)
            {
               // new KeyValuePair<TipSaveData, Tip>(new TipSaveData(_tip.Key.id,_tip.Key.isSaved), new Tip(_tip.Value.header,_tip.Value.content));
                currentTips.Remove(_tip.Key);
                currentTips.Add(new TipSaveData(_tip.Key.id, _tip.Key.isSaved, _tip.Key.lastShown), new Tip(_tip.Value.header, _tip.Value.content));
            }
        }
    }

    private void SaveTips()
    {
        string path = Path.Combine(Application.persistentDataPath, "tipdata");
        if (File.Exists(path))
        {
            FileInfo fileInfoIfAlreadyExists = new FileInfo(path);
            fileInfoIfAlreadyExists.IsReadOnly = false;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        TipSaveData[] _tips = ExtractTipSavedDatas();
        formatter.Serialize(stream, _tips);
        stream.Close();
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.IsReadOnly = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            SaveTips();
        }
    }
}
