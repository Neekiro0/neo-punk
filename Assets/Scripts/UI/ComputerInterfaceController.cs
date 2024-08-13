using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class ComputerInterfaceController : MonoBehaviour
{
    private VisualElement InterfaceRoot;
    private GameObject MainUserInterface;
    public bool isShown;
    private Button startMissionButton;
    private Button closeButton;
    private MissionData SelectedMission;
    
    /*
     * UI elements
     */
    private Label nameLabel;
    private Label objectiveLabel;
    private Label descriptionLabel;
    private Label codeLabel;
    private VisualElement MissionUi;
    private VisualElement MissionUiPlaceholder;


    private void Awake()
    {
        MainUserInterface = GameObject.Find("UserInterface").transform.Find("Main User Interface").gameObject;
    }

    private void Start()
    {
        HideComputerInterface();
    }

    private void OnEnable()
    {
        // Ładujemy UXML
        var uiDocument = GetComponent<UIDocument>();
        InterfaceRoot = uiDocument.rootVisualElement;
        
        /*
         * ładowanie pól informacji o misji
         */
        nameLabel = InterfaceRoot.Q<Label>("NameLabel");
        objectiveLabel = InterfaceRoot.Q<Label>("ObjectiveLabel");
        descriptionLabel = InterfaceRoot.Q<Label>("DescriptionLabel");
        codeLabel = InterfaceRoot.Q<Label>("CodeLabel");
        MissionUi = InterfaceRoot.Q<VisualElement>("Main");
        MissionUiPlaceholder = InterfaceRoot.Q<VisualElement>("Placeholder");

        // Znajdź przyciski w interfejsie użytkownika
        startMissionButton = InterfaceRoot.Q<Button>("StartMissionButton");
        closeButton = InterfaceRoot.Q<Button>("CloseButton");
        closeButton.clicked += OnCloseButtonClick;
        startMissionButton.clicked += StartMission;
        
        // Pobierz wszystkie przyciski misji i dodaj im event click
        var missionButtons = InterfaceRoot.Query<Button>(className: "MissionsTreeMissionButton").ToList();
        foreach (var button in missionButtons)
        {
            button.clicked += () =>
            {
                ShowMissionDetails(button.text);
            };
        }

        StartCoroutine(CodeAnmiation(0.5f));
    }

    private void Update()
    {
        if(isShown && Input.GetKeyDown( KeyCode.Escape ) )
        {
            HideComputerInterface();
        }
    }

    public void ShowComputerInterface()
    {
        isShown = true;
        gameObject.SetActive(true);
        MainUserInterface.SetActive(false);
        Cursor.visible = true;
    }
    
    public void HideComputerInterface()
    {
        isShown = false;
        gameObject.SetActive(false);
        MainUserInterface.SetActive(true);
        Cursor.visible = false;
    }

    private void OnCloseButtonClick() { HideComputerInterface(); }

    private void StartMission()
    {
        if (null != SelectedMission)
        {
            SceneManager.LoadScene(SelectedMission.SceneName);
        }
    }

    public void ShowMissionDetails(string MissionName)
    {
        SelectedMission = null;
        SelectedMission = Resources.Load<MissionData>("Missions/"+MissionName+"/MissionData");
        if (null != SelectedMission)
        {
            MissionUi.style.display = DisplayStyle.Flex;
            MissionUiPlaceholder.style.display = DisplayStyle.None;

            StartCoroutine(PrintName(SelectedMission.MissionName, 0.1f));
            StartCoroutine(PrintObjective(SelectedMission.Objective, 0.05f));
            StartCoroutine(PrintDescription(SelectedMission.Description, 0.01f));
        }
        else
        {
            MissionUi.style.display = DisplayStyle.None;
            MissionUiPlaceholder.style.display = DisplayStyle.Flex;
            Debug.LogError("Not found mission named: "+MissionName);
        }
    }

    IEnumerator PrintName(string name, float letterPause)
    {
        nameLabel.text = "";
        for (int i = 0; i < name.Length; i++)
        {
            nameLabel.text += name[i];
            yield return new WaitForSeconds(letterPause);
        }
    }
    
    IEnumerator PrintObjective(string name, float letterPause)
    {
        objectiveLabel.text = "";
        for (int i = 0; i < name.Length; i++)
        {
            objectiveLabel.text += name[i];
            yield return new WaitForSeconds(letterPause);
        }
    }
    
    IEnumerator PrintDescription(string name, float letterPause)
    {
        descriptionLabel.text = "";
        for (int i = 0; i < name.Length; i++)
        {
            descriptionLabel.text += name[i];
            yield return new WaitForSeconds(letterPause);
        }
    }
    IEnumerator CodeAnmiation(float blinkingBreak)
    {
        while (true)
        {
            codeLabel.text += " |";
            yield return new WaitForSeconds(blinkingBreak);
            codeLabel.text = codeLabel.text.Remove(codeLabel.text.Length - 2);
            yield return new WaitForSeconds(blinkingBreak);
        }
    }
}
