using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class ComputerInterfaceController : MonoBehaviour
{
    private VisualElement InterfaceRoot;
    private GameObject MainUserInterface;
    public bool isShown;
    private Button startMissionButton;
    private Button closeButton;


    private void Awake()
    {
        InterfaceRoot = gameObject.GetComponent<UIDocument>().rootVisualElement;
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

        // Znajdź przyciski w interfejsie użytkownika
        startMissionButton = InterfaceRoot.Q<Button>("StartMissionButton");
        closeButton = InterfaceRoot.Q<Button>("CloseButton");

        // Przypisz zdarzenia kliknięcia do przycisków
        closeButton.clicked += OnCloseButtonClick;
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
        Debug.Log("Działa!");
        isShown = false;
        gameObject.SetActive(false);
        MainUserInterface.SetActive(true);
        Time.timeScale = 1;
        Cursor.visible = false;
    }

    private void OnCloseButtonClick() { HideComputerInterface(); }
}
