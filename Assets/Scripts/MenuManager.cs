using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yaSingleton;

[CreateAssetMenu(fileName ="MenuManager", menuName="Systems/MenuManager")]
public class MenuManager : Singleton<MenuManager>
{
    //public GameObject pauseMenuPrefab;
    public GameObject pauseMenuPrefab;
    public GameObject saveMenuPrefab;
    public GameObject canvasPrefab;
    private Canvas _canvas;
    private List<GameMenu> activeMenus;

    protected override void Initialize()
    {
        base.Initialize();
        activeMenus = new List<GameMenu>();
    }

    private Canvas SceneCanvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = FindObjectOfType<Canvas>();

                if (_canvas == null)
                {
                    _canvas = Instantiate(canvasPrefab).GetComponent<Canvas>();
                }
            }

            return _canvas;
        }
    }

    

    public void CreateMenu(GameMenu menu)
    {
        
            GameObject newMenu = Instantiate(menu.gameObject, SceneCanvas.transform);
            GameMenu menuComponent = newMenu.GetComponent<GameMenu>();
            System.Type type = menuComponent.GetType();

            if (type == typeof(PauseMenu))
            {
                AddPauseMenuFunctionality((PauseMenu) menuComponent);
            }
            else if (type == typeof(SaveMenu))
            {
                AddSaveMenuFunctionality((SaveMenu) menuComponent);
            }

            menuComponent.destroyMenuButton.onClick.AddListener(
                delegate { DestroyMenu(menuComponent); }
                );

            activeMenus.Add(menu);
        
    }

    public void AddPauseMenuFunctionality(PauseMenu pauseMenu)
    {
        pauseMenu.openSaveMenuButton.onClick.AddListener(
                delegate { ValidateAndCreateMenuPrefab(saveMenuPrefab); }
                );
        pauseMenu.openSaveMenuButton.onClick.AddListener(
                delegate { DestroyMenu(pauseMenu); }
                );
        pauseMenu.hintsButton.onClick.AddListener(
                delegate { Debug.Log("Hints Button Pressed!.."); }
                );
        pauseMenu.quitButton.onClick.AddListener(
                delegate { Application.Quit(); }
                );
    }

    public void AddSaveMenuFunctionality(SaveMenu saveMenu)
    {

    }

    public void DestroyMenu(GameMenu menu)
    {
        if (menu != null)
        {
            Destroy(menu.gameObject);
            activeMenus.RemoveAt(0);
            
        }
    }

    private void ValidateAndCreateMenuPrefab(GameObject prefab)
    {
        GameMenu menu = prefab.GetComponent<GameMenu>();
        if (menu != null)
        {
            CreateMenu(menu);
        }
        else
        {
            Debug.LogError("Pause Menu Prefab has no GameMenu script attached to it.");
        }
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool exists = false;

            foreach (GameMenu menu in activeMenus)
            {
                if (menu.GetType() == typeof(PauseMenu))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                ValidateAndCreateMenuPrefab(pauseMenuPrefab);
            }
        }
        
    }
}
