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
        Time.timeScale = 0;
        Cursor.visible = true;
    }
    
    public void HideComputerInterface()
    {
        isShown = false;
        gameObject.SetActive(false);
        MainUserInterface.SetActive(true);
        Time.timeScale = 1;
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
            nameLabel.text = SelectedMission.MissionName;
            objectiveLabel.text = SelectedMission.Objective;
            descriptionLabel.text = SelectedMission.Description;
            MissionUi.style.display = DisplayStyle.Flex;
            MissionUiPlaceholder.style.display = DisplayStyle.None;
        }
        else
        {
            MissionUi.style.display = DisplayStyle.None;
            MissionUiPlaceholder.style.display = DisplayStyle.Flex;
            Debug.LogError("Not found mission named: "+MissionName);
        }
    }
}
